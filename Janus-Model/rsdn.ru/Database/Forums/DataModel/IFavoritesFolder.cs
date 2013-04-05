using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("favorites_folders")]
	public interface IFavoritesFolder
	{
		[Column]        int ID { get; }
		[Column]        string Name { get; }
		[Column("pid")] int ParentID { get; }
		[Column]        string Comment { get; }
	}
}