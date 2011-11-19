using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Rsdn.Janus.DataModel
{
	[TableName("favorites_folders")]
	public interface IFavoritesFolder
	{
		int ID { get; }
		string Name { get; }
		[MapField("pid")]
		int ParentID { get; }
		string Comment { get; }
	}
}