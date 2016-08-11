using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Обработчик команд для работы с БД.
	/// </summary>
	[CommandTarget]
	internal sealed class DatabaseCommandTarget : CommandTarget
	{
		public DatabaseCommandTarget(IServiceProvider provider)
			: base(provider) { }

		[CommandExecutor("Janus.Database.DbMigration")]
		public void ExecuteDbMigration(ICommandContext context)
		{
			DBSchemaManager.Migrate(context);
		}

		[CommandExecutor("Janus.Database.RebuildAggregates")]
		public void ExecuteRebuildAggregates(ICommandContext context)
		{
			DatabaseManager.InitDbAggr(context, true, null);
		}
	}
}