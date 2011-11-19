using BLToolkit.DataAccess;
using BLToolkit.Mapping;

namespace Rsdn.Janus.DataModel
{
	[TableName("users")]
	public interface IUser
	{
		[MapField("uid")]
		int ID { get; }

		string HomePage { get; }

		string Origin { get; }

		string RealName { get; }

		string Spec { get; }

		UserClass UserClass { get; }

		[MapField("username")]
		string Name { get; }

		[MapField("usernick")]
		string Nick { get; }

		string WhereFrom { get; }
	}
}