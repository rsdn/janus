using System;

namespace Rsdn.Janus
{
	public class ApplicationInfo
	{
		public const string ApplicationName = "RSDN@Home";

		public const string VersionMajor = "1";
		public const string VersionMinor = "0";
		public const string VersionBuild = "0";
		public const string VersionRevision = "0";
		public const string VersionString = VersionMajor + "." + VersionMinor + "." + VersionBuild + "." + VersionRevision;
		public const string Release = "alpha 5";
		public const string Company = "RSDN Team";
		public const string Copyright = "Copyright © by RSDN Team (http://rsdn.ru), 2002-2012";

		public const string ResourcesNamespace = "Rsdn.Janus.Resources.";
		
		public const string MachineConfigFolder = "Janus";
		public const string AppDataFolder = "Janus";

		public static readonly Version Version = new Version(VersionString);

		public static string FullVersion =>
			$"{Version.Major}.{Version.Minor}.{Version.Build} {Release} {Resources.Revision} {Version.Revision}";

		public static string NameWithVersion => $"{ApplicationName} {Resources.Version.ToLower()} {FullVersion}";

		public static string NameWithVersionAndCopyright => $"{NameWithVersion}, {Copyright}";
	}
}