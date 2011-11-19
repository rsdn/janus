using System;
using JetBrains.Annotations;

namespace Rsdn.Janus
{
	internal class MainWindowService : IMainWindowService
	{
		private readonly Func<MainForm> _mainFormGetter;
		private MainForm _form;

		public MainWindowService([NotNull] Func<MainForm> mainFormGetter)
		{
			if (mainFormGetter == null)
				throw new ArgumentNullException("mainFormGetter");

			_mainFormGetter = mainFormGetter;
		}

		public void EnsureVisible()
		{
			GetMainForm().EnsureVisible();
		}

		public void UpdateText()
		{
			GetMainForm().Text = MainForm.GetCaption();
		}

		public void Refresh()
		{
			GetMainForm().Refresh();
		}

		private MainForm GetMainForm()
		{
			return _form ?? (_form = _mainFormGetter());
		}
	}
}