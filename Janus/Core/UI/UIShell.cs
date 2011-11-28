using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

using JetBrains.Annotations;

using Rsdn.Janus.ObjectModel;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	internal class UIShell : IUIShell
	{
		private readonly Func<IWin32Window> _parentWindowGetter;
		private readonly Action<bool> _uiFreezer;
		private readonly AsyncOperation _ctorAsyncOperation;
		private readonly HashSet<UIFreezer> _freezers = new HashSet<UIFreezer>();
		private readonly SynchronizationContext _uiSyncContext = SynchronizationContext.Current;

		public UIShell(Func<IWin32Window> parentWindowGetter, [NotNull] Action<bool> uiFreezer)
		{
			if (parentWindowGetter == null)
				throw new ArgumentNullException("parentWindowGetter");
			if (uiFreezer == null) throw new ArgumentNullException("uiFreezer");
			_parentWindowGetter = parentWindowGetter;
			_uiFreezer = uiFreezer;
			_ctorAsyncOperation = AsyncHelper.CreateOperation();
		}

		#region IUIShell Members
		/// <summary>
		/// Создать <see cref="AsyncOperation"/>, привязанную к UI потоку.
		/// </summary>
		public AsyncOperation CreateUIAsyncOperation()
		{
			AsyncOperation asyncOp = null;
			_ctorAsyncOperation.Send(() => asyncOp = AsyncHelper.CreateOperation());
			if (asyncOp == null)
				throw new ApplicationException("Error creating async operation.");
			return asyncOp;
		}

		public SynchronizationContext UISyncContext
		{
			get { return _uiSyncContext; }
		}

		/// <summary>
		/// Получить главного родителя окон.
		/// </summary>
		public IWin32Window GetMainWindowParent()
		{
			return _parentWindowGetter();
		}

		/// <summary>
		/// Заморозить UI приложения.
		/// </summary>
		public IDisposable FreezeUI(IServiceProvider provider)
		{
			lock (_freezers)
			{
				if (_freezers.Count == 0)
				{
					_ctorAsyncOperation.Post(
						() =>
						{
							_uiFreezer(false);
							var notifyIconSvc = provider.GetService<INotifyIconService>();
							if (notifyIconSvc != null)
								notifyIconSvc.Enabled = false;
						}
					);
				}
				var freezer = new UIFreezer(
					f =>
					{
						lock (_freezers)
						{
							_freezers.Remove(f);
							if (_freezers.Count == 0)
							{
								_ctorAsyncOperation.Post(
									() =>
										{
											_uiFreezer(true);
										var notifyIconSvc = provider.GetService<INotifyIconService>();
										if (notifyIconSvc != null)
											notifyIconSvc.Enabled = true;
									}
								);
							}
						}
					});
				_freezers.Add(freezer);
				return freezer;
			}
		}

		public void RefreshData()
		{
			Forums.Instance.Refresh();
			Features.Instance.Refresh();
			if (ApplicationManager.Instance != null)
				ApplicationManager.Instance.Navigator.RefreshData();
		}

		#endregion

		#region UIFreezer class
		private class UIFreezer : IDisposable
		{
			private readonly Action<UIFreezer> _unfreezeHandler;
			private bool _disposed;

			public UIFreezer(Action<UIFreezer> unfreezeHandler)
			{
				_unfreezeHandler = unfreezeHandler;
			}

			#region IDisposable Members
			///<summary>
			///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			///</summary>
			public void Dispose()
			{
				if (_disposed)
					return;
				_unfreezeHandler(this);
				_disposed = true;
			}
			#endregion
		}
		#endregion
	}
}
