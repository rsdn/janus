using System;

using Rsdn.Janus.AT;
using Rsdn.Janus.Framework;
using Rsdn.Janus.Properties;

using BLToolkit.Data.Linq;

namespace Rsdn.Janus
{
	internal class ViolationsSyncTask : RsdnSyncTask<ViolationRequest, ViolationResponse>
	{
		private const string _rvVarSlotName = "LastViolationsRowVersion";

		public ViolationsSyncTask(IServiceProvider provider, string name)
			: base(provider, name, () => Resources.QueryNewViolations)
		{}

		public override bool IsTaskActive()
		{
			return true;
		}

		protected override ViolationRequest PrepareRequest(ISyncContext context)
		{
			var cfg = GetSyncConfig();
			using (var db = context.CreateDBContext())
				return
					new ViolationRequest
					{
						UserName = cfg.Login,
						Password = cfg.Password,
						LastRowVersion = context.DBVars()[_rvVarSlotName].FromHexString(),
						SubscribedForums = db.GetSubscribedForums()
					};
		}

		protected override ViolationResponse MakeRequest(ISyncContext context, JanusAT svc, ViolationRequest rq)
		{
			return svc.GetNewViolations(rq);
		}

		private static long RVValue(byte[] rv)
		{
			return
				rv[7]
					+ (rv[6] << 8)
					+ (rv[5] << 16)
					+ (rv[4] << 24)
					+ ((long)rv[3] << 32)
					+ ((long)rv[2] << 40)
					+ ((long)rv[1] << 48)
					+ ((long)rv[0] << 56);
		}

		protected override void ProcessResponse(
			ISyncContext context,
			ViolationRequest request,
			ViolationResponse response)
		{
			using (var db = context.CreateDBContext())
			using (var tx = db.BeginTransaction())
			{
				foreach (var violation in response.Violations)
					db
						.Violations()
							// ReSharper disable AccessToModifiedClosure
							.Value(_ => _.MessageID, () => violation.MessageID)
							.Value(_ => _.PenaltyType, () => (PenaltyType) violation.PenaltyType)
							.Value(_ => _.Reason, () => violation.Reason)
							.Value(_ => _.Create, () => violation.CreatedOn)
							// ReSharper restore AccessToModifiedClosure
						.Insert();
				tx.Commit();
			}

			if (response.Violations.Length > 0)
			{
				JanusViolationInfo max = null;
				foreach (var v in response.Violations)
					if (max == null || RVValue(max.RowVersion) < RVValue(v.RowVersion))
						max = v;
				if (max != null)
					context.DBVars()[_rvVarSlotName] = max.RowVersion.ToHexString();
			}
		}
	}
}