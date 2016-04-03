using System;

namespace Rsdn.Janus
{
	public abstract class SqlFormatterBase : ISqlFormatter
	{
		#region ISqlFormatter Members
		public virtual string TrueConstant => "1";

		public virtual string FalseConstant => "0";

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
			return $"Len({value})";
		}

		public virtual string IfEquals(string value1, string value2, string trueResult, string falseResult)
		{
			return $"(CASE {value1} WHEN {value2} THEN {trueResult} ELSE {falseResult} END)";
		}

		public virtual string IfNull(string value, string trueResult)
		{
			return $"COALESCE({value}, {trueResult})";
		}

		public abstract string Take(string query, string count);

		public virtual string FormatDateTime(DateTime dateTime)
		{
			return $"\'{dateTime:MM-dd-yyyy}\'";
		}

		/// <summary>
		/// UPDATE tableName alias SET expression
		/// </summary>
		public virtual string UpdateWithAlias(string tableName, string alias, string expression)
		{
			return $"UPDATE {tableName} {alias} SET {expression} ";
		}

		/// <summary>
		/// Удаляет все записи из таблицы
		/// <param name="tableName">Имя таблицы</param>
		/// <returns>Сконструированный запрос</returns>
		/// </summary>
		public virtual string TruncateTable(string tableName)
		{
			return $"DELETE FROM {tableName}";
		}
		#endregion
	}
}
