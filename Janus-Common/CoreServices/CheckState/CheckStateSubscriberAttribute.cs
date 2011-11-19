using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[MeansImplicitUse]
	public class CheckStateSubscriberAttribute : CheckStateMethodAttribute
	{
		public CheckStateSubscriberAttribute([NotNull] string name)
			: base(name) { }
	}
}