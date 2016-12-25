using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("tags")]
	public interface ITag
	{
		[Identity]
		[Column("id")]
		int ID { get; }

		[Identity]
		[Column("tag_value")]
		string TagValue { get; }
	}
}