using System;
using System.ComponentModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Форматирует SQL в соответствии с конкретикой драйвера.
	/// </summary>
	[Localizable(false)]
	public interface ISqlFormatter
	{
		string TrueConstant { get; }
		string FalseConstant { get; }

		/// <summary>
		/// ограничение на кол-во элементов в предложении IN (...)
		/// в некоторых случаях длинное предложение mid in (...)
		/// приводит к переполнению стека парсера MsSql сервера для сложных запросов (больше 8000 элементов в in).
		/// Firebird не любит больше 1500 элементов в in.
		/// </summary>
		int MaxInClauseElements { get; }

		/// <summary>
		/// Функция длины строки.
		/// </summary>
		string StrLen(string value);

		string IfEquals(string value1, string value2, string trueResult, string falseResult);
		string IfNull(string value, string trueResult);
		string Take(string query, string count);
		string FormatDateTime(DateTime dateTime);

		/// <summary>
		/// UPDATE tableName alias SET expression
		/// </summary>
		string UpdateWithAlias(string tableName, string alias, string expression);

		/// <summary>
		/// Удаляет все записи из таблицы
		/// <param name="tableName">Имя таблицы</param>
		/// <returns>Сконструированный запрос</returns>
		/// </summary>
		string TruncateTable([Localizable(false)]string tableName);
	}
}
