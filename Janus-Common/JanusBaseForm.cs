using System;
using System.Windows.Forms;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Базовая форма януса.
	/// </summary>
	public partial class JanusBaseForm : Form
	{
		public void EnsureVisible()
		{
			EnsureVisible(null);
		}

		public void EnsureVisible([CanBeNull] Func<IWin32Window> ownerGetter)
		{
			if (!Visible)
				if (ownerGetter != null)
					Show(ownerGetter());
				else
					Show();
			else
				Activate();

			if (WindowState == FormWindowState.Minimized)
				WindowState = LastNonMinimizedState;
		}

		public FormWindowState LastNonMinimizedState { get; private set; }


		protected override void OnLocationChanged(EventArgs e)
		{
			base.OnLocationChanged(e);

			if (WindowState != FormWindowState.Minimized)
				LastNonMinimizedState = WindowState;
		}
	}
}