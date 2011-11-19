using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class ForumEntryChangedEventArgs
	{
		private readonly ReadOnlyCollection<ForumEntryIds> _entries;
		private readonly ForumEntryChangeType _changeType;

		public ForumEntryChangedEventArgs(
			[NotNull] IEnumerable<ForumEntryIds> entries,
			ForumEntryChangeType changeType)
		{
			if (entries == null)
				throw new ArgumentNullException("entries");

			_entries = Array.AsReadOnly(entries.ToArray());
			_changeType = changeType;
		}

		public ReadOnlyCollection<ForumEntryIds> Entries
		{
			get { return _entries; }
		}

		public ForumEntryChangeType ChangeType
		{
			get { return _changeType; }
		}
	}
}