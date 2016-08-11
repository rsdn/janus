using System;
using System.Collections.Generic;
using System.Linq;

using LinqToDB;

using Rsdn.Janus.AT;
using Rsdn.Janus.Framework;
using Rsdn.Janus.org.rsdn;
using Rsdn.Janus.Properties;

namespace Rsdn.Janus
{
	internal class UsersSyncTask : RsdnSyncTask<UserRequest, UserResponse>
	{
		public UsersSyncTask(IServiceProvider provider, string name)
			: base(provider, name, () => Resources.QueryNewUsers)
		{}

		public override bool IsTaskActive()
		{
			return GetSyncConfig().DownloadUsers;
		}

		protected override UserRequest PrepareRequest(ISyncContext context)
		{
			var cfg = GetSyncConfig();
			return
				new UserRequest
				{
					userName = cfg.Login,
					password = cfg.Password,
					lastRowVersion = context.DBVars()["LastUserRowVersion"].FromHexString(),
					maxOutput = cfg.MaxUsersPerSession
				};
		}

		protected override UserResponse MakeRequest(ISyncContext context, JanusAT svc, UserRequest rq)
		{
			return svc.GetNewUsers(rq);
		}

		protected override void ProcessResponse(ISyncContext context, UserRequest request, UserResponse response)
		{
			AddNewUsers(context, response.users);
			context.DBVars()["LastUserRowVersion"] = response.lastRowVersion.ToHexString();

			var receiveUsers = response.users.Length;
			context.StatisticsContainer.AddValue(JanusATInfo.UsersStats, receiveUsers);
		}

		private static void AddNewUsers(
			IServiceProvider provider,
			IEnumerable<JanusUserInfo> users)
		{
			using (var db = provider.CreateDBContext())
			using (var tx = db.BeginTransaction())
			{
				DeleteUsers(provider, db, users.Select(user => user.userId));
				foreach (var user in users)
				{
					var locUser = user;
					db
						.Users()
							.Value(_ => _.ID,        locUser.userId)
							.Value(_ => _.Name,      locUser.userName)
							.Value(_ => _.Nick,      locUser.userNick)
							.Value(_ => _.RealName,  locUser.realName)
							.Value(_ => _.Spec,      locUser.specialization)
							.Value(_ => _.HomePage,  locUser.homePage)
							.Value(_ => _.Origin,    locUser.origin)
							.Value(_ => _.WhereFrom, locUser.whereFrom)
							.Value(_ => _.UserClass, (UserClass)locUser.userClass)
						.Insert();
				}
				tx.Commit();
			}
		}

		private static void DeleteUsers(
			IServiceProvider provider,
			IDataContext db,
			IEnumerable<int> userIds)
		{
			foreach (var series in userIds.SplitToSeries(provider.MaxInClauseElements()))
				// ReSharper disable AccessToForEachVariableInClosure
				db.Users(u => series.Contains(u.ID)).Delete();
				// ReSharper restore AccessToForEachVariableInClosure
		}
	}
}
