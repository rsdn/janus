using System;
using System.Collections.Generic;

using Microsoft.Win32;
using Rsdn.Janus.Core.TextMacros.ResharperMacros;

namespace Rsdn.Janus
{
	/// <summary>
	/// Макрос версии решарпера.
	/// </summary>
	[TextMacrosProvider]
	internal sealed class ResharperMacrosProvider : ITextMacrosProvider
	{
		private const string _registry2003Key =
			@"SOFTWARE\Microsoft\VisualStudio\7.1\AddIns\ReSharperAddIn2003";
		private const string _registry2005Key =
			@"SOFTWARE\Microsoft\VisualStudio\8.0\AddIns\ReSharperAddIn2005";
		private const string _keyName = "FriendlyName";

		private static string GetValue(IServiceProvider serviceProvider)
		{
			var versions = new List<string>(2);

			var key1 = Registry.LocalMachine.OpenSubKey(_registry2003Key);
			if (key1 != null)
			{
				var keyValue = key1.GetValue(_keyName, string.Empty).ToString();
				if (!String.IsNullOrEmpty(keyValue))
					versions.Add(keyValue + " (VS2003);");
			}
			var key2 = Registry.LocalMachine.OpenSubKey(_registry2005Key);
			if (key2 != null)
			{
				var keyValue = key2.GetValue(_keyName, string.Empty).ToString();
				if (!String.IsNullOrEmpty(keyValue))
					versions.Add(keyValue + " (VS2005);");
			}

			return versions.Count != 0 ? versions.JoinStrings(" ") : "...";
		}

		public IEnumerable<ITextMacros> CreateTextMacroses()
		{
			return new[] { new TextMacros("resharper", ResharperMacrosResources.ResharperVersion, GetValue) };
		}
	}
}