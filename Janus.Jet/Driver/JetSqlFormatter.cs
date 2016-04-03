using System;

namespace Rsdn.Janus.Jet
{
	internal class JetSqlFormatter : SqlFormatterBase
	{
		/// <summary>
		/// ограничение на кол-во элементов в предложении IN (...)
		/// в некоторых случаях длинное предложение mid in (...)
		/// приводит к переполнению стека парсера MsSql сервера для сложных запросов (больше 8000 элементов в in).
		/// Firebird не любит больше 1500 элементов в in.
		/// </summary>
		public override int MaxInClauseElements => 2000;

		public override string IfEquals(string value1, string value2, string trueResult,
			string falseResult)
		{
			return $"IIF({value1} = {value2}, {trueResult}, {falseResult})";
		}

		public override string FalseConstant => "False";

		public override string TrueConstant => "True";

		public override string IfNull(string value, string trueResult) => $"IIF({value} IS NULL, {trueResult}, {value})";

		public override string Take(string query, string count)
		{
			return $"SELECT TOP {count} {query}";
		}

		public override string FormatDateTime(DateTime dateTime) => $"#{dateTime:MM-dd-yyyy}#";
	}
}
