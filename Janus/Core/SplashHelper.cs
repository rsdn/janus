using System;
using System.Threading;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	public static class SplashHelper
	{
		private static readonly object _showLock = new object();
		private static bool _visible;
		private static MethodInvoker _closer;
		private static SplashForm _form;

		public static IBootTimeInformer Show(IServiceProvider provider)
		{
			if (!_visible)
				lock (_showLock)
					if (!_visible)
					{
						var readyEvt = new ManualResetEvent(false);
						var splashUIThread = new Thread(
							() =>
							{
								_form = new SplashForm(provider);
								var asyncOp = AsyncHelper.CreateOperation();
								_closer = () => asyncOp.Post(_form.Close);
								readyEvt.Set();
								Application.Run(_form);
							})
							{
								IsBackground = true,
								CurrentCulture = Thread.CurrentThread.CurrentCulture,
								CurrentUICulture = Thread.CurrentThread.CurrentUICulture
							};
						splashUIThread.SetApartmentState(ApartmentState.STA);
						splashUIThread.Start();
						readyEvt.WaitOne(); // Wait for form creation
						_visible = true;
					}
			return _form;
		}

		public static void Hide()
		{
			if (_visible)
				lock (_showLock)
					if (_visible)
					{
						_visible = false;
						_closer();
						_form = null;
					}
		}

		public static IProgressService GetProgressService()
		{
			return _form;
		}
	}
}
