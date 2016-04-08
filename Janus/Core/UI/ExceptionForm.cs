using System;
using System.Windows.Forms;

using CodeJam.Services;

namespace Rsdn.Janus
{
	/// <summary>
	/// Форма с сообщением об ошибке.
	/// </summary>
	internal partial class ExceptionForm : Form
	{
		private readonly IServiceProvider _provider;
		private readonly Exception _exception;

		public ExceptionForm(
			IServiceProvider provider,
			Exception exception,
			bool allowContinue)
		{
			if (exception == null)
				throw new ArgumentNullException(nameof(exception));

			InitializeComponent();

			if (provider != null)
			{
				var styleImageSvc = provider.GetService<IStyleImageManager>();
				if (styleImageSvc != null)
					_errorPictureBox.Image = styleImageSvc.GetImage("error", StyleImageType.ConstSize);
				_bugReportButton.Enabled = true;
			}

			_provider = provider;
			_exception = exception;
			_errorMessageTextBox.Text = exception.Message;
			_additionalInfoTextBox.Text = exception.ToString();

			_continueButton.Enabled = allowContinue;
		}

		private void BugReportButtonClick(object sender, EventArgs e)
		{
			_provider
				.GetRequiredService<IOutboxManager>()
				.AddBugReport(
					_exception.Message,
					"",
					_exception.StackTrace,
					false);
			_bugReportButton.Enabled = false;
		}

		private void ExitButtonClick(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
