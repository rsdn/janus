using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("favorites")]
	public interface IFavoritesItem
	{
		int ID { get; }
		[Column("mid")]
		int MessageID { get; }
		[Column("fid")]
		int FolderID { get; }
		string Comment { get; }
		string Url { get; }

		[Association(ThisKey = "MessageID", OtherKey = "ID", CanBeNull = true)]
		IForumMessage Message { get; }
	}
}