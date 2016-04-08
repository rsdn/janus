using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public class ForumEntryChangedEventArgs
	{
		public ForumEntryChangedEventArgs(
			[NotNull] IEnumerable<ForumEntryIds> entries,
			ForumEntryChangeType changeType)
		{
			if (entries == null)
				throw new ArgumentNullException(nameof(entries));

			Entries = Array.AsReadOnly(entries.ToArray());
			ChangeType = changeType;
		}

		public ReadOnlyCollection<ForumEntryIds> Entries { get; }

		public ForumEntryChangeType ChangeType { get; }
	}
}