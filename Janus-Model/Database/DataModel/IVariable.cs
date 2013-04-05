using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("vars")]
	public interface IVariable
	{
		[Column]
		string Name { get; }

		[Column("varvalue")]
		string Value { get; }
	}
}