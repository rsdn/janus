using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Редактор оценок.
	/// </summary>
	internal class RateMarkEditor : IOutboxItemEditor
	{
		#region IOutboxItemEditor Members
		public bool AllowEdit(object item)
		{
			return false;
		}

		public bool AllowDelete(object item)
		{
			return true;
		}

		public void Edit(object item)
		{
			throw new NotSupportedException();
		}

		public void Delete(object item)
		{
			((RateMark)item).Delete();
		}
		#endregion
	}
}
