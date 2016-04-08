using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class AutocompleteItem
	{
		public AutocompleteItem([NotNull] string itemText, [CanBeNull] Func<string, int> caretShiftCalculator)
		{
			if (itemText == null) throw new ArgumentNullException(nameof(itemText));

			ItemText = itemText;
			CaretShiftCalculator = caretShiftCalculator;
		}

		public AutocompleteItem(string itemText) : this(itemText, null)
		{}

		[NotNull]
		public string ItemText { get; }

		[CanBeNull]
		public Func<string, int> CaretShiftCalculator { get; }
	}
}