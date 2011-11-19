namespace Rsdn.Janus.Jet
{
	[ExtensionInfoProvider]
	internal class JetExtensionInfoProvider : ExtensionInfoProviderBase
	{
		private const string _iconResource = "Rsdn.Janus.Jet.JetExtensionIcon.png";

		public JetExtensionInfoProvider() : base(_iconResource)
		{}

		protected override string GetDisplayName()
		{
			return Resources.ExtensionDisplayName;
		}
	}
}