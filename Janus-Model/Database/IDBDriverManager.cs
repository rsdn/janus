using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Сервис, обеспечивающий доступ к информации о драйверах.
	/// </summary>
	public interface IDBDriverManager
	{
		/// <summary>
		/// Возвращает экземпляр драйвера по его имени.
		/// </summary>
		[NotNull]
		IDBDriver GetDriver([NotNull] string driverName);

		/// <summary>
		/// Возвращает список зарегистрированных драйверов.
		/// </summary>
		[NotNull]
		JanusDBDriverInfo[] GetRegisteredDriverInfos();

		[NotNull]
		JanusDBDriverInfo GetDriverInfo([NotNull] string driverName);
	}
}