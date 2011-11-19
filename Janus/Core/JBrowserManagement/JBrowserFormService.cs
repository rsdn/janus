using System;

namespace Rsdn.Janus
{
	internal class JBrowserFormService : IBrowserFormService
	{
		private readonly WebBrowserForm _form;

		public JBrowserFormService(WebBrowserForm form)
		{
			_form = form;
		}

		#region IJBrowserService Members

		public string Url
		{
			get { return _form.Url; }
		}

		public void Close()
		{
			_form.Close();
		}

		public bool CanNavigateBackward
		{
			get { return _form.CanNavigateBackward; }
		}

		public bool CanNavigateForward
		{
			get { return _form.CanNavigateForward; }
		}

		public bool CanStop
		{
			get { return _form.CanStop; }
		}

		public event EventHandler Navigated
		{
			add { _form.Navigated += value; }
			remove { _form.Navigated -= value; }
		}

		public event EventHandler CanNavigateBackwardChanged
		{
			add { _form.CanNavigateBackwardChanged += value; }
			remove { _form.CanNavigateBackwardChanged -= value; }
		}

		public event EventHandler CanNavigateForwardChanged
		{
			add { _form.CanNavigateForwardChanged += value; }
			remove { _form.CanNavigateForwardChanged -= value; }
		}

		public event EventHandler DocumentCompleted
		{
			add { _form.DocumentCompleted += value; }
			remove { _form.DocumentCompleted -= value; }
		}

		public void NavigateBackward()
		{
			_form.NavigateBackward();
		}

		public void NavigateForward()
		{
			_form.NavigateForward();
		}

		public void NavigateTo(string url)
		{
			_form.NavigateTo(url);
		}

		public void Refresh()
		{
			_form.RefreshPage();
		}

		public void Stop()
		{
			_form.Stop();
		}

		#endregion
	}
}
