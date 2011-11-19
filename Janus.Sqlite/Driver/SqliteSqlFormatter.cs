using System;

using Rsdn.SmartApp;

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
		public override int MaxInClauseElements
		{
			get { return 10000; }
		}

		public override string StrLen(string value)
		{
			return string.Format("LENGTH({0})", value);
		}

		public override string IfNull(string value, string trueResult)
		{
			return "IFNULL({0}, {1})".FormatStr(value, trueResult);
		}

		public override string Take(string query, string count)
		{
			return "SELECT {1} LIMIT {0}".FormatStr(count, query);
		}

		public override string FormatDateTime(DateTime dateTime)
		{
			return string.Format("\'{0:yyyy-MM-dd}\'", dateTime);
		}

		public override string UpdateWithAlias(string tableName, string alias, string expression)
		{
			return string.Format("UPDATE {1} SET {2} FROM {0} AS {1}", tableName, alias, expression);
		}
	}
}
