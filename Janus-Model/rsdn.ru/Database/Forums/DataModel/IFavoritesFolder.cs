using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("favorites_folders")]
	public interface IFavoritesFolder
	{
		int ID { get; }
		string Name { get; }
		[Column("pid")]
		int ParentID { get; }
		string Comment { get; }
	}
}