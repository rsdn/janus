using System;

namespace Rsdn.Janus.Sqlite
{
	internal class SqliteSqlFormatter : SqlFormatterBase
	{
		/// <summary>
		/// ограничение на кол-во элементов в предложении IN (...)
		/// в некоторых случаях длинное предложение mid in (...)
		/// приводит к переполнению стека парсера MsSql сервера для сложных запросов (больше 8000 элементов в in).
		/// Firebird не любит больше 1500 элементов в in.
		/// </summary>
		public override int MaxInClauseElements => 10000;

		public override string StrLen(string value) => $"LENGTH({value})";

		public override string IfNull(string value, string trueResult) => $"IFNULL({value}, {trueResult})";

		public override string Take(string query, string count) => $"SELECT {count} LIMIT {query}";

		public override string FormatDateTime(DateTime dateTime) => $"\'{dateTime:yyyy-MM-dd}\'";

		public override string UpdateWithAlias(string tableName, string alias, string expression) =>
			string.Format("UPDATE {1} SET {2} FROM {0} AS {1}", tableName, alias, expression);
	}
}
