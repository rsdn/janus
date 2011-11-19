namespace Rsdn.Janus.Firebird
{
	[ExtensionInfoProvider]
	internal class FBExtensionInfoProvider : ExtensionInfoProviderBase
	{
		public FBExtensionInfoProvider()
			: base("Rsdn.Janus.Firebird.FbExtensionIcon.png")
		{}

		protected override string GetDisplayName()
		{
			return Resources.ExtensionDisplayName;
		}
	}
}