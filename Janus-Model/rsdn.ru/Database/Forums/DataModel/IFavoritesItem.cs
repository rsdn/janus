using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("favorites")]
	public interface IFavoritesItem
	{
		[Column]        int    ID { get; }
		[Column("mid")] int    MessageID { get; }
		[Column("fid")] int    FolderID { get; }
		[Column]        string Comment { get; }
		[Column]        string Url { get; }

		[Association(ThisKey = "MessageID", OtherKey = "ID", CanBeNull = true)]
		IForumMessage Message { get; }
	}
}