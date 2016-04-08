using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
	[BaseTypeRequired(typeof(INavigationPageProvider))]
	[MeansImplicitUse]
	public class NavigationPageProviderAttribute : Attribute
	{
		public NavigationPageProviderAttribute([NotNull] string pathMask)
		{
			if (pathMask == null)
				throw new ArgumentNullException(nameof(pathMask));

			PathMask = pathMask;
		}

		public string PathMask { get; }
	}
}