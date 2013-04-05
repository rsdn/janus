using System;
using System.Collections.Generic;

using LinqToDB;

namespace Rsdn.Janus
{
	/// <summary>
	/// Temp service. Must be eliminated after config extensibility refactoring complete.
	/// </summary>
	public interface IRsdnForumService
	{
		IRsdnForumConfig GetConfig();
		bool IsForumSubscribed(int forumId);
		void UpdateForumAggregates(IServiceProvider provider, IDataContext db, IEnumerable<int> tidList);
	}
}