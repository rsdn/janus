using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class AutocompleteItem
	{
		private readonly string _itemText;
		private readonly Func<string, int> _caretShiftCalculator;

		public AutocompleteItem([NotNull] string itemText, [CanBeNull] Func<string, int> caretShiftCalculator)
		{
			if (itemText == null) throw new ArgumentNullException("itemText");

			_itemText = itemText;
			_caretShiftCalculator = caretShiftCalculator;
		}

		public AutocompleteItem(string itemText) : this(itemText, null)
		{}

		[NotNull]
		public string ItemText
		{
			get { return _itemText; }
		}

		[CanBeNull]
		public Func<string, int> CaretShiftCalculator
		{
			get { return _caretShiftCalculator; }
		}
	}
}