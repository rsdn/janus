using System;

using CodeJam.Extensibility;

namespace Rsdn.Janus
{
	[Service(typeof(ConfigChangedNotifier))]
	public sealed class ConfigChangedNotifier
	{
		public ConfigChangedNotifier(IServiceProvider provider) { }

		public event EventHandler<object, ConfigChangedEventArgs> ConfigChanged;

		public void FireConfigChanged(Config config)
		{
			ConfigChanged?.Invoke(this, new ConfigChangedEventArgs(config));
		}
	}
}
