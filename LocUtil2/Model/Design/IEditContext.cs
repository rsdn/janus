using System.Globalization;
using System.Windows.Forms;

namespace Rsdn.LocUtil.Model.Design
{
	/// <summary>
	/// Интерфейс контекста редактирования.
	/// </summary>
	public interface IEditContext
	{
		/// <summary>
		/// Получить список доступных культур.
		/// </summary>
		CultureInfo[] AvailableCultures
		{get; set;}

		/// <summary>
		/// Владелец диалоговых окон.
		/// </summary>
		IWin32Window DialogsOwner
		{get;}
	}
}
