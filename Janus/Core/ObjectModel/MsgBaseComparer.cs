using System.Collections.Generic;

using Rsdn.Janus.ObjectModel;

namespace Rsdn.Janus
{
	internal class MsgBaseComparer : IEqualityComparer<MsgBase>
	{
		public static readonly MsgBaseComparer Instance = new MsgBaseComparer();

		#region Implementation of IEqualityComparer<MsgBase>
		public bool Equals(MsgBase x, MsgBase y)
		{
			return x.ID == y.ID;
		}

		public int GetHashCode(MsgBase obj)
		{
			return obj.ID;
		}
		#endregion
	}
}
