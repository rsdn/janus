using System;
using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class TextMacros : ITextMacros
	{
		private readonly string _displayName;
		private readonly string _macrosText;
		private readonly Func<IServiceProvider, string> _resultGetter;

		public TextMacros(
			[NotNull] string macrosText, 
			[NotNull] string displayName, 
			[NotNull] Func<IServiceProvider, string> resultGetter)
		{
			if (macrosText == null) 
				throw new ArgumentNullException("macrosText");
			if (displayName == null) 
				throw new ArgumentNullException("displayName");
			if (resultGetter == null) 
				throw new ArgumentNullException("resultGetter");

			_macrosText = macrosText;
			_resultGetter = resultGetter;
			_displayName = displayName;
		}

		public string DisplayName
		{
			get { return _displayName; }
		}

		public string MacrosText
		{
			get { return _macrosText; }
		}

		public string GetResult(IServiceProvider serviceProvider)
		{
			return _resultGetter(serviceProvider);
		}
	}
}