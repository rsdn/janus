using Rsdn.SmartApp;

namespace Rsdn.Janus.Firebird
{
	internal class FbSqlFormatter: SqlFormatterBase
	{
		/// <summary>
		/// ограничение на кол-во элементов в предложении IN (...)
		/// в некоторых случаях длинное предложение mid in (...)
		/// приводит к переполнению стека парсера MsSql сервера для сложных запросов (больше 8000 элементов в in).
		/// Firebird не любит больше 1500 элементов в in.
		/// </summary>
		public override int MaxInClauseElements
		{
			get { return 1200; }
		}

		public override string StrLen(string value)
		{
			return string.Format("StrLen({0})", value);
		}

		public override string Take(string query, string count)
		{
			return "SELECT FIRST({0}) {1}".FormatStr(count, query);
		}
	}
}
