using Rsdn.SmartApp;

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
		public override int MaxInClauseElements
		{
			get { return 4000; }
		}

		public override string Take(string query, string count)
		{
			return "SELECT TOP {0} {1}".FormatStr(count, query);
		}

		public override string UpdateWithAlias(string tableName, string alias, string expression)
		{
			return string.Format("UPDATE {1} SET {2} FROM {0} {1}", tableName, alias, expression);
		}

		public override string TruncateTable(string tableName)
		{
			return string.Format("TRUNCATE TABLE {0}", tableName);
		}
	}
}
