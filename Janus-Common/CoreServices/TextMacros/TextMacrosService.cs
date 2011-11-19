using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	[Service(typeof(ITextMacrosService))]
	public class TextMacrosService : ITextMacrosService
	{
		private readonly IServiceProvider _serviceProvider;
		private Dictionary<string, ITextMacros> _textMacroses;

		public TextMacrosService([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");

			_serviceProvider = serviceProvider;
		}

		#region Implementation of ITextMacrosService

		public ICollection<ITextMacros> TextMacroses
		{
			get { return GetTextMacroses().Values; }
		}

		public ITextMacros GetTextMacros([NotNull] string macrosText)
		{
			if (macrosText == null)
				throw new ArgumentNullException("macrosText");

			ITextMacros result;
			return GetTextMacroses().TryGetValue(macrosText, out result) ? result : null;
		}

		#endregion

		private Dictionary<string, ITextMacros> GetTextMacroses()
		{
			if (_textMacroses == null)
			{
				_textMacroses = new Dictionary<string, ITextMacros>();
				foreach (var macros in
					new ExtensionsCache<TextMacrosProviderInfo, ITextMacrosProvider>(_serviceProvider)
						.GetAllExtensions()
						.SelectMany(provider => provider.CreateTextMacroses()))
				{
					if (_textMacroses.ContainsKey(macros.MacrosText))
						throw new ApplicationException(
							"Текстовый макрос '{0}' определен более одного раза.".FormatStr(macros.DisplayName));
					_textMacroses.Add(macros.MacrosText, macros);
				}
			}
			return _textMacroses;
		}
	}
}