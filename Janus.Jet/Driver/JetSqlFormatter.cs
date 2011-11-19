using System;

using Rsdn.SmartApp;

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
		public override int MaxInClauseElements
		{
			get { return 2000; }
		}

		public override string IfEquals(string value1, string value2, string trueResult,
			string falseResult)
		{
			return "IIF({0} = {1}, {2}, {3})".FormatStr(value1, value2, trueResult, falseResult);
		}

		public override string FalseConstant
		{
			get { return "False"; }
		}

		public override string TrueConstant
		{
			get { return "True"; }
		}

		public override string IfNull(string value, string trueResult)
		{
			return "IIF({0} IS NULL, {1}, {0})".FormatStr(value, trueResult);
		}

		public override string Take(string query, string count)
		{
			return "SELECT TOP {0} {1}".FormatStr(count, query);
		}

		public override string FormatDateTime(DateTime dateTime)
		{
			return string.Format("#{0:MM-dd-yyyy}#", dateTime);
		}
	}
}
