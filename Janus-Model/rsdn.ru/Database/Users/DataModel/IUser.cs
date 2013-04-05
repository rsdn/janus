using LinqToDB.Mapping;

namespace Rsdn.Janus.DataModel
{
	[Table("users")]
	public interface IUser
	{
		[Column("uid")]
		int ID { get; }

		string HomePage { get; }

		string Origin { get; }

		string RealName { get; }

		string Spec { get; }

		UserClass UserClass { get; }

		[Column("username")]
		string Name { get; }

		[Column("usernick")]
		string Nick { get; }

		string WhereFrom { get; }
	}
}