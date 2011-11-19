using System;

namespace Rsdn.Janus
{
	public sealed class ConfigChangedEventArgs : EventArgs
	{
		public ConfigChangedEventArgs(Config config)
		{
			Config = config;
		}

		public Config Config { get; private set; }
	}
}
