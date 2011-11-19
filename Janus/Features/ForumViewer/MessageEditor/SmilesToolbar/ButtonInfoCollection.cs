using Rsdn.Janus.Framework;

namespace Rsdn.Janus
{
	public delegate void ButtonInfoHandler(
		ButtonInfoCollection sender, ButtonInfo buttonInfo);

	public delegate void ButtonInfoChangedHandler(
		ButtonInfoCollection sender,
		ButtonInfo oldButtonInfo,
		ButtonInfo newButtonInfo);

	/// <summary>
	/// Типизированная коллекция позволяющие хранить экземпляры 
	/// класса ButtonInfo.
	/// </summary>
	public class ButtonInfoCollection : CollectionBase<ButtonInfo>
	{
		protected override void OnInsertComplete(int index, ButtonInfo value)
		{
			OnButtonInfoAdded(value);
		}

		protected override void OnRemoveComplete(int index, ButtonInfo value)
		{
			OnButtonInfoRemoved(value);
		}

		protected override void OnSetComplete(int index, ButtonInfo oldValue, ButtonInfo newValue)
		{
			OnButtonInfoChanged(oldValue, newValue);
		}

		public event ButtonInfoHandler ButtonInfoAdded;

		protected virtual void OnButtonInfoAdded(ButtonInfo buttonInfo)
		{
			if (ButtonInfoAdded != null)
				ButtonInfoAdded(this, buttonInfo);
		}

		public event ButtonInfoHandler ButtonInfoRemoved;

		protected virtual void OnButtonInfoRemoved(ButtonInfo buttonInfo)
		{
			if (ButtonInfoRemoved != null)
				ButtonInfoRemoved(this, buttonInfo);
		}
		
		public event ButtonInfoChangedHandler ButtonInfoChanged;

		protected virtual void OnButtonInfoChanged(
			ButtonInfo oldButtonInfo, ButtonInfo newButtonInfo)
		{
			if (ButtonInfoChanged != null)
				ButtonInfoChanged(this, oldButtonInfo, newButtonInfo);
		}
	}
}
