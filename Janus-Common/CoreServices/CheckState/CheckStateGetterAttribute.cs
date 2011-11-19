using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[MeansImplicitUse]
	public class CheckStateGetterAttribute : CheckStateMethodAttribute
	{
		public CheckStateGetterAttribute([NotNull] string name)
			: base(name) { }
	}
}