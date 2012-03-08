using System;

namespace Rsdn.Janus.Sqlite
{
	[MenuProvider]
	internal sealed class SqliteMenuProvider : ResourceMenuProvider
	{
		public SqliteMenuProvider(IServiceProvider serviceProvider)
			: base(
				serviceProvider,
				"Rsdn.Janus.Sqlite.Menu.SqliteMenu.xml",
				"Rsdn.Janus.Sqlite.Resources") { }
	}
}