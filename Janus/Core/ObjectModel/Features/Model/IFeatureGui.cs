using System.Windows.Forms;

namespace Rsdn.Janus.ObjectModel
{
	/// <summary>
	/// Должен реализовываться фичами, чтобы те могли указать контрол который
	/// они желают использовать для отображения своего содержимого.
	/// </summary>
	public interface IFeatureGui
	{
		/// <summary>
		/// Возвращает контрол который будет использоваться для взаимодействия
		/// с фичей.
		/// </summary>
		Control GuiControl{ get; }

		/// <summary>
		/// !!! На кой хрен это сюда добавили, да еще не потрудились откомментировать???
		/// </summary>
		void ConfigChanged();
	}
}
