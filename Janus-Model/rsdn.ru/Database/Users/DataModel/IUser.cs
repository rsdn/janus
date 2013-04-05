using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("users")]
	public interface IUser
	{
		[Column("uid")]
		int ID { get; }

		[Column]
		string HomePage { get; }

		[Column]
		string Origin { get; }

		[Column]
		string RealName { get; }

		[Column]
		string Spec { get; }

		[Column]
		UserClass UserClass { get; }

		[Column("username")]
		string Name { get; }

		[Column("usernick")]
		string Nick { get; }

		[Column]
		string WhereFrom { get; }
	}
}