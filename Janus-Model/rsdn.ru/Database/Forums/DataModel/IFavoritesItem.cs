using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Rsdn.Janus.DataModel
{
	[TableName("favorites")]
	public interface IFavoritesItem
	{
		int ID { get; }
		[MapField("mid")]
		int MessageID { get; }
		[MapField("fid")]
		int FolderID { get; }
		string Comment { get; }
		string Url { get; }

		[Association(ThisKey = "MessageID", OtherKey = "ID", CanBeNull = true)]
		IForumMessage Message { get; }
	}
}