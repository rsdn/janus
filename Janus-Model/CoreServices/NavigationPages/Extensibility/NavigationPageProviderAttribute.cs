using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	[BaseTypeRequired(typeof(INavigationPageProvider))]
	[MeansImplicitUse]
	public class NavigationPageProviderAttribute : Attribute
	{
		private readonly string _pathMask;

		public NavigationPageProviderAttribute([NotNull] string pathMask)
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