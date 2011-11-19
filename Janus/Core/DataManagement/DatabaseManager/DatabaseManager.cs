using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Класс для изоляции работы с базой данных
	/// </summary>
	internal static partial class DatabaseManager
	{
		#region Конструкторы
		static DatabaseManager()
		{
			if (Config.Instance.DbDriver.IsNullOrEmpty())
				Config.Instance.DbDriver = "Jet";
		}
		#endregion
	}
}