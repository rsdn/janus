using System.Data;

namespace Rsdn.Janus
{
	public interface IDBSchemaDriver
	{
		/// <summary>
		/// Создать БД.
		/// </summary>
		/// <param name="constr">строка подключения</param>
		void CreateDatabase(string constr);

		/// <summary>
		/// Создать схему метаданных из исходной базы
		/// </summary>
		/// <param name="connStr"></param>
		DBSchema LoadExistingSchema(string connStr);

		/// <summary>
		/// Сравнение схемы с эталонной и выдача во внутреннюю таблицу DDL комманд
		/// </summary>
		void CompareDbsc(DBSchema dbsc, string targetConnStr);

		/// <summary>
		/// Привести исходную базу к той с которой сравнивали.
		/// </summary>
		/// <param name="connStr"></param>
		void Prepare(string connStr);

		/// <summary>
		/// Запись сгенерированых DDL комманд в файл
		/// </summary>
		/// <param name="path">путь к файлу для записи команд</param>
		void PrepareToSqlFile(string path);

		IDbConnection CreateConnection(string connStr);
		string MakeSelect(TableSchema table, bool orderedByPK);
		string MakeInsert(TableSchema table);
		IDbDataParameter ConvertToDbParameter(TableColumnSchema column, IDbDataParameter parameter);
		void BeginTableLoad(IDbConnection connection, TableSchema table);
		void EndTableLoad(IDbConnection connection, TableSchema table);
	}
}