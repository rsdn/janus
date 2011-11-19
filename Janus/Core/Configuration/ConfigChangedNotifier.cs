using System;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[Service(typeof(ConfigChangedNotifier))]
	public sealed class ConfigChangedNotifier
	{
		public ConfigChangedNotifier(IServiceProvider provider) { }

		public event EventHandler<object, ConfigChangedEventArgs> ConfigChanged;

		public void FireConfigChanged(Config config)
		{
			if (ConfigChanged != null)
				ConfigChanged(
					this,
					new ConfigChangedEventArgs(config));
		}
	}
}
