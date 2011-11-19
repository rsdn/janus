using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	/// <summary>
	/// Сервис, отражающий специфику реализации UI.
	/// </summary>
	public interface IUIShell
	{
		/// <summary>
		/// Создать <see cref="AsyncOperation"/>, привязанную к UI потоку.
		/// </summary>
		AsyncOperation CreateUIAsyncOperation();

		/// <summary>
		/// Synchronization context for UI thread.
		/// </summary>
		SynchronizationContext UISyncContext { get; }

		/// <summary>
		/// Получить главного родителя окон.
		/// </summary>
		IWin32Window GetMainWindowParent();

		/// <summary>
		/// Заморозить UI приложения.
		/// </summary>
		IDisposable FreezeUI(IServiceProvider provider);

		/// <summary>
		/// Обновляет данные, связанные с UI.
		/// </summary>
		void RefreshData();
	}
}