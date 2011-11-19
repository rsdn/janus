namespace Rsdn.Janus
{
	interface IDBSchemaDriver
	{
		/// <summary>
		/// Создать схему метаданных из исходной базы
		/// </summary>
		void MakeDbsc();

		/// <summary>
		/// Сравнение схемы с эталонной и выдача во внутреннюю таблицу DDL комманд
		/// </summary>
		void CompareDbsc(DbsmSchema dbsc);

		/// <summary>
		/// Привести исходную базу к той с которой сравнивали.
		/// </summary>
		void Prepare();

		/// <summary>
		/// Запись сгенерированых DDL комманд в файл
		/// </summary>
		/// <param name="path">путь к файлу для записи команд</param>
		void PrepareToSqlFile(string path);
	}
}
