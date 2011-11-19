namespace Rsdn.Janus.Mssql
{
	[ExtensionInfoProvider]
	internal class MssqlExtensionInfoProvider : ExtensionInfoProviderBase
	{
		private const string _iconResource = "Rsdn.Janus.Mssql.MssqlExtensionIcon.png";

		public MssqlExtensionInfoProvider() : base(_iconResource)
		{}

		protected override string GetDisplayName()
		{
			return Resources.ExtensionDisplayName;
		}
	}
}