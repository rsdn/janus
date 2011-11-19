using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Подержка редактирования запрошенных топиков.
	/// </summary>
	internal class DownloadTopicEditor : IOutboxItemEditor
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
			throw new InvalidOperationException();
		}

		public void Delete(object item)
		{
			((DownloadTopic)item).Delete();
		}

		#endregion
	}
}
