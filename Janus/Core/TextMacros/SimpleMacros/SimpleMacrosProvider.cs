using System;
using System.Collections.Generic;
using Rsdn.Janus.Core.TextMacros.SimpleMacros;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Простые статические макросы.
	/// </summary>
	[TextMacrosProvider]
	internal sealed class SimpleMacrosProvider : ITextMacrosProvider
	{
		public IEnumerable<ITextMacros> CreateTextMacroses()
		{
			return new[]
			{
				new TextMacros(
					"appname",
					SimpleMacrosResources.ApplicationName,
					serviceProvider => ApplicationInfo.ApplicationName),
				new TextMacros(
					"version",
					SimpleMacrosResources.ApplicationVersion,
					serviceProvider =>
					"{0}.{1}.{2}".FormatStr(
						ApplicationInfo.Version.Major,
						ApplicationInfo.Version.Minor,
						ApplicationInfo.Version.Build)),
				new TextMacros(
					"release",
					SimpleMacrosResources.ApplicationRelease,
					serviceProvider => ApplicationInfo.Release),
				new TextMacros(
					"revision",
					SimpleMacrosResources.ApplicationRevision,
					serviceProvider => ApplicationInfo.Version.Revision.ToString()),
				new TextMacros(
					"copyright",
					SimpleMacrosResources.ApplicationCopyright,
					serviceProvider => ApplicationInfo.Copyright),
				new TextMacros(
					"nick",
					SimpleMacrosResources.Nick,
					serviceProvider => Config.Instance.Login),
				new TextMacros(
					"osname",
					SimpleMacrosResources.OSName,
					serviceProvider => Environment.OSVersion.GetOSName()),
				new TextMacros(
					"osversion",
					SimpleMacrosResources.OSVersion,
					serviceProvider => Environment.OSVersion.Version.ToString()),
				new TextMacros(
					"framework",
					SimpleMacrosResources.FrameworkVersion,
					serviceProvider => Environment.Version.ToString())
			};
		}
	}
}