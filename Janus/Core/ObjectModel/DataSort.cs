using System;

using Rsdn.Janus.ObjectModel;
using System.Collections.Generic;

namespace Rsdn.Janus
{
	public static class DataSorter
	{
		public static string SortCriteria(SortType sort, SortType def)
		{
			if (def == SortType.ByDefault)
				throw new ApplicationException("def == SortType.ByDefault");

			switch(sort)
			{
				case SortType.ByLastUpdateDateAsc:
					return "ORDER BY [answers_last_update_date] ASC";
				case SortType.ByLastUpdateDateDesc:
					return "ORDER BY [answers_last_update_date] DESC";
				case SortType.ByIdAsc:
					return "ORDER BY m.[mid] ASC";
				case SortType.ByIdDesc:
					return "ORDER BY m.[mid] DESC";
				case SortType.BySubjectAsc:
					return "ORDER BY m.[subject] ASC";
				case SortType.BySubjectDesc:
					return "ORDER BY m.[subject] DESC";
				case SortType.ByAuthorAsc:
					return "ORDER BY m.[usernick] ASC";
				case SortType.ByAuthorDesc:
					return "ORDER BY m.[usernick] DESC";
				case SortType.ByDateAsc:
					return "ORDER BY m.[dte] ASC";
				case SortType.ByDateDesc:
					return "ORDER BY m.[dte] DESC";
				case SortType.ByForumAsc:
					return "ORDER BY m.[gid] ASC";
				case SortType.ByForumDesc:
					return "ORDER BY m.[gid] DESC";
				default:
					return SortCriteria(def, def);
			}
		}
	}

	internal class MsgComparer<T> : IComparer<T>
	{
		private readonly SortType _sort;

		public MsgComparer(SortType sort)
		{
			_sort = sort;
		}

		public int Compare(T msg1, T msg2)
		{
			return CompareMsgs(_sort, msg1 as IMsg, msg2 as IMsg);
		}

		private static int CompareMsgs(SortType sort, IMsg msg1, IMsg msg2)
		{
			if(msg1 == null && msg2 == null)
				return 0;

			if(msg1 == null)
				return -1;

			if(msg2 == null)
				return 1;

			switch(sort)
			{
				case SortType.ByIdAsc:
					return msg1.ID.CompareTo(msg2.ID);

				case SortType.ByIdDesc:
					return msg2.ID.CompareTo(msg1.ID);

				case SortType.BySubjectAsc:
					return msg1.Subject.CompareTo(msg2.Subject);

				case SortType.BySubjectDesc:
					return msg2.Subject.CompareTo(msg1.Subject);

				case SortType.ByAuthorAsc:
					return msg1.UserNick.CompareTo(msg2.UserNick);

				case SortType.ByAuthorDesc:
					return msg2.UserNick.CompareTo(msg1.UserNick);

				case SortType.ByDateAsc:
					return msg1.Date.CompareTo(msg2.Date);

				case SortType.ByDateDesc:
					return msg2.Date.CompareTo(msg1.Date);

				case SortType.ByForumAsc:
					return msg1.ForumID.CompareTo(msg2.ForumID);

				case SortType.ByForumDesc:
					return msg2.ForumID.CompareTo(msg1.ForumID);

				default:
					throw new NotSupportedException(
						"This sorting type " + sort + " is not supported");
			}
		}
	}
}
