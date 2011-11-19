using System;
using System.Linq;
using System.Windows.Forms;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[Service(typeof(ICheckStateService))]
	public class CheckStateService : ICheckStateService
	{
		private readonly ICheckStateSource[] _checkStateSources;

		public CheckStateService([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");

			_checkStateSources = 
				new ExtensionsCache<CheckStateSourceInfo, ICheckStateSource>(serviceProvider).GetAllExtensions();
		}

		#region ICheckStateService Members

		public CheckState GetCheckState(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] string name)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");
			if (name == null)
				throw new ArgumentNullException("name");

			CheckState? result = null;
			foreach (var source in _checkStateSources)
			{
				var state = source.GetCheckState(serviceProvider, name);

				if (result != null)
				{
					if (state != null)
						result = state.Value.Combine(result.Value);
				}
				else
					result = state;

				if (result == CheckState.Indeterminate)
					return CheckState.Indeterminate;
			}
			return result ?? CheckState.Indeterminate;
		}

		public IDisposable SubscribeCheckStateChanged(
			[NotNull] IServiceProvider serviceProvider, 
			[NotNull] CheckStateChangedEventHandler handler)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");
			if (handler == null)
				throw new ArgumentNullException("handler");

			return
				_checkStateSources
					.Select(target => target.SubscribeCheckStateChanged(serviceProvider, handler))
					.Merge();
		}

		#endregion
	}
}