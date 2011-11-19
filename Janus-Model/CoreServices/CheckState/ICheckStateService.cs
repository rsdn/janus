using System;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	/// <summary>
	/// Сервис для получения состояния галочек CheckBox'ов. 
	/// </summary>
	public interface ICheckStateService
	{
		/// <summary>
		/// Получить состояние галочки.
		/// </summary>
		/// <param name="provider">Поставщик сервисов.</param>
		/// <param name="name">Имя галочки.</param>
		/// <returns>Состояние галочки.</returns>
		CheckState GetCheckState(IServiceProvider provider, string name);

		/// <summary>
		/// Подписаться на оповещения о смене состояния галочек.
		/// </summary>
		/// <param name="serviceProvider">Поставщик сервисов.</param>
		/// <param name="handler">Обработчик, вызываемый при смене состояния галочек.</param>
		IDisposable SubscribeCheckStateChanged(
			IServiceProvider serviceProvider, CheckStateChangedEventHandler handler);
	}

	public delegate void CheckStateChangedEventHandler(object sender, string[] names);
}