using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Rsdn.Janus.DataModel
{
	[TableName("vars")]
	public interface IVariable
	{
		string Name { get; }

		[MapField("varvalue")]
		string Value { get; }
	}
}