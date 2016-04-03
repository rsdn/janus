using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

using CodeJam.Extensibility;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public static class TextMacrosHelper
	{
		private const string _macroPrefix = "@@";
		private static readonly Regex _macroRegex =
			new Regex(_macroPrefix + @"(?'id'\w+)(\W|$)", RegexOptions.Compiled);

		/// <summary>
		/// Заменяет макросы в строке на результат их выполнения.
		/// </summary>
		public static string ReplaceMacroses(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] string text)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));
			if (text == null)
				throw new ArgumentNullException(nameof(text));

			var macrosSvc = serviceProvider.GetService<ITextMacrosService>();
			if (macrosSvc == null)
				return text;

			var shift = 0;
			var match = _macroRegex.Match(text);
			var res = new StringBuilder(text);


			while (match.Success)
			{
				var macrosText = match.Groups["id"].Value;
				var macros = macrosSvc.GetTextMacros(macrosText);

				if (macros != null)
				{
					var value = macros.GetResult(serviceProvider);
					var len = macrosText.Length + 2;

					res.Remove(match.Index + shift, len);
					res.Insert(match.Index + shift, value);

					shift += value.Length - len;
				}

				match = match.NextMatch();
			}

			return res.ToString();
		}
		
		/// <summary>
		/// Ищет в строке макросы.
		/// </summary>
		public static IDictionary<int, int> FindMacroses(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] string text)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));
			if (text == null)
				throw new ArgumentNullException(nameof(text));

			var res = new Dictionary<int, int>();

			var macrosSvc = serviceProvider.GetService<ITextMacrosService>();
			if (macrosSvc == null)
				return res;

			var match = _macroRegex.Match(text);

			while (match.Success)
			{
				if (macrosSvc.GetTextMacros(match.Groups["id"].Value) != null)
					res[match.Index] = match.Groups["id"].Length + 2;

				match = match.NextMatch();
			}

			return res;
		}
	}
}