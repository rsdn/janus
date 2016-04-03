namespace Rsdn.Janus.Mssql
{
	internal class MssqlSqlFormatter : SqlFormatterBase
	{
		/// <summary>
		/// ограничение на кол-во элементов в предложении IN (...)
		/// в некоторых случаях длинное предложение mid in (...)
		/// приводит к переполнению стека парсера MsSql сервера для сложных запросов (больше 8000 элементов в in).
		/// Firebird не любит больше 1500 элементов в in.
		/// </summary>
		public override int MaxInClauseElements => 4000;

		public override string Take(string query, string count) => $"SELECT TOP {count} {query}";

		public override string UpdateWithAlias(string tableName, string alias, string expression) =>
			$"UPDATE {alias} SET {expression} FROM {tableName} {alias}";

		public override string TruncateTable(string tableName) => $"TRUNCATE TABLE {tableName}";
	}
}
