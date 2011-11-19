using System;
using System.Windows.Forms;

namespace Rsdn.Shortcuts
{
	/// <summary>
	/// Summary description for IDialogContainer.
	/// </summary>
	public interface IDialogContainer : IDisposable
	{
		Control GetDialog();
		void AcceptChanges();
		void RejectChanges();
	}
}