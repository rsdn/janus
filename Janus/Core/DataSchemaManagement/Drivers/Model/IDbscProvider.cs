namespace Rsdn.Janus
{
	public interface IDbscProvider
	{
		/// <summary>
		/// Создать схему БД на основании строки соединения.
		/// </summary>
		/// <param name="constr">строка соединения</param>
		/// <returns>схема БД</returns>
		DbsmSchema MakeDbsc(string constr);
	}
}