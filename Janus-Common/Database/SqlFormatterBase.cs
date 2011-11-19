using System;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	public abstract class SqlFormatterBase : ISqlFormatter
	{
		#region ISqlFormatter Members
		public virtual string TrueConstant
		{
			get { return "1"; }
		}

		public virtual string FalseConstant
		{
			get { return "0"; }
		}

		/// <summary>
		/// ограничение на кол-во элементов в предложении IN (...)
		/// в некоторых случаях длинное предложение mid in (...)
		/// приводит к переполнению стека парсера MsSql сервера для сложных запросов (больше 8000 элементов в in).
		/// Firebird не любит больше 1500 элементов в in.
		/// </summary>
		public abstract int MaxInClauseElements { get; }

		/// <summary>
		/// Функция длины строки.
		/// </summary>
		public virtual string StrLen(string value)
		{
			return "Len({0})".FormatStr(value);
		}

		public virtual string IfEquals(string value1, string value2, string trueResult, string falseResult)
		{
			return
				"(CASE {0} WHEN {1} THEN {2} ELSE {3} END)"
				.FormatStr(value1, value2, trueResult, falseResult);
		}

		public virtual string IfNull(string value, string trueResult)
		{
			return "COALESCE({0}, {1})".FormatStr(value, trueResult);
		}

		public abstract string Take(string query, string count);

		public virtual string FormatDateTime(DateTime dateTime)
		{
			return "\'{0:MM-dd-yyyy}\'".FormatStr(dateTime);
		}

		/// <summary>
		/// UPDATE tableName alias SET expression
		/// </summary>
		public virtual string UpdateWithAlias(string tableName, string alias, string expression)
		{
			return string.Format("UPDATE {0} {1} SET {2} ", tableName, alias, expression);
		}

		/// <summary>
		/// Удаляет все записи из таблицы
		/// <param name="tableName">Имя таблицы</param>
		/// <returns>Сконструированный запрос</returns>
		/// </summary>
		public virtual string TruncateTable(string tableName)
		{
			return string.Format("DELETE FROM {0}", tableName);
		}
		#endregion
	}
}
