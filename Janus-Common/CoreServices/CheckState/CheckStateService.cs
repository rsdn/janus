using System;
using System.Linq;
using System.Windows.Forms;

using CodeJam;
using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	[Service(typeof(ICheckStateService))]
	public class CheckStateService : ICheckStateService
	{
		private readonly ICheckStateSource[] _checkStateSources;

		public CheckStateService([JetBrains.Annotations.NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			_checkStateSources = 
				new ExtensionsCache<CheckStateSourceInfo, ICheckStateSource>(serviceProvider).GetAllExtensions();
		}

		#region ICheckStateService Members

		public CheckState GetCheckState(
			[JetBrains.Annotations.NotNull] IServiceProvider serviceProvider,
			[JetBrains.Annotations.NotNull] string name)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));
			if (name == null)
				throw new ArgumentNullException(nameof(name));

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
			[JetBrains.Annotations.NotNull] IServiceProvider serviceProvider, 
			[JetBrains.Annotations.NotNull] CheckStateChangedEventHandler handler)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));
			if (handler == null)
				throw new ArgumentNullException(nameof(handler));

			return
				_checkStateSources
					.Select(target => target.SubscribeCheckStateChanged(serviceProvider, handler))
					.Merge();
		}

		#endregion
	}
}