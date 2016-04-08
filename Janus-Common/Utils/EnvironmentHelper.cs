using System;
using System.Linq;
using System.Reflection;
using System.Web;

using CodeJam.Services;

namespace Rsdn.Janus
{
	/// <summary>
	/// Вспомогательные функции для получения информации о среде выполения.
	/// </summary>
	public static class EnvironmentHelper
	{
		private const string _unknownOs = "Unknown Win32 compatible OS";

		// ReSharper disable once PossibleNullReferenceException
		public static Assembly JanusAssembly { get; } = Type.GetType("Rsdn.Janus.Janus, Janus").Assembly;

		public static string GetOSName(this OperatingSystem os)
		{
			switch (os.Platform)
			{
				case PlatformID.Win32Windows:
					if (os.Version.Major == 4)
						switch (os.Version.Minor)
						{
							case 0:
								return "Windows 95";
							case 10:
								return "Windows 98";
							case 90:
								return "Windows ME";
						}
					return _unknownOs;

				case PlatformID.WinCE:
					return "Windows CE";

				// TODO: Windows 2003 R2, Windows 2008, Windows 2008 R2 determination required
				case PlatformID.Win32NT:
					switch (os.Version.Major)
					{
						case 3:
							return "Windows NT 3.51";
						case 4:
							return "Windows NT 4.0";
						case 5:
							switch (os.Version.Minor)
							{
								case 0:
									return "Windows 2000";
								case 1:
									return "Windows XP";
								case 2:
									return "Windows 2003";
							}
							break;
						case 6:
							switch (os.Version.Minor)
							{
								case 0:
									return "Windows Vista";
								case 1:
									return "Windows 7";
								case 2:
									return "Windows 8";
							}
							break;
					}

					return _unknownOs;

				default:
					return _unknownOs;
			}
		}

		public static string GetOSNameWithVersion(this OperatingSystem os)
		{
			return GetOSName(os) + " " + os.Version;
		}

		public static bool IsModernOS(this OperatingSystem os)
		{
			return os.Platform == PlatformID.Win32NT && os.Version.Major > 4;
		}

		public static string GetAssemblyDir(this Assembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException(nameof(assembly));
			var uri = new Uri(assembly.CodeBase);
			// Склеиваем все сегменты, кроме первого и последнего.
			return
				uri
					.Segments
					.Skip(1)
					.Take(uri.Segments.Length - 2)
					.Select(HttpUtility.UrlDecode)
					.JoinStrings();
		}

		public static string GetJanusRootDir()
		{
			return Assembly.GetEntryAssembly().GetAssemblyDir();
		}

		public static void SetSplashMessage(this IServiceProvider provider, string message)
		{
			var infSvc = provider.GetService<IBootTimeInformer>();
			infSvc?.SetText(message);
		}

		public static bool IsSplashAvailable(this IServiceProvider provider)
		{
			var infSvc = provider.GetService<IBootTimeInformer>();
			return infSvc != null;
		}
	}
}