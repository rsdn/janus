using System;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	public class NavigationPageProviderInfo : ElementInfo
	{
		private readonly string _pathMask;

		public NavigationPageProviderInfo([NotNull] Type type, [NotNull] string pathMask)
			: base(type)
		{
			if (pathMask == null)
				throw new ArgumentNullException("pathMask");

			_pathMask = pathMask;
		}

		public string PathMask
		{
			get { return _pathMask; }
		}
	}
}