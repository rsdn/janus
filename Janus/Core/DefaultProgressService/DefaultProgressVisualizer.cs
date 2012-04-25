using System.ComponentModel;
using System.Windows.Forms;

namespace Rsdn.Janus.Framework
{
	internal partial class DefaultProgressVisualizer : UserControl, IProgressVisualizer
	{
		private readonly AsyncOperation _asyncOp;

		public DefaultProgressVisualizer(bool allowCancel)
		{
			_asyncOp = AsyncHelper.CreateOperation();
			InitializeComponent();
			_cancelButton.Visible = allowCancel;
		}

		private void CancelButtonClick(object sender, System.EventArgs e)
		{
			CancelRequested = true;
			_cancelButton.Enabled = false;
		}

		#region IProgressVisualizer Members

		public void SetProgressText(string text)
		{
			_asyncOp.Post(() => _messageLabel.Text = text);
		}

		public void ReportProgress(int total, int current)
		{
			_asyncOp.Post(
				() =>
				{
					if (total > 0)
					{
						_progressBar.Style = ProgressBarStyle.Blocks;
						_progressBar.Minimum = 0;
						_progressBar.Maximum = total;
						_progressBar.Value = current;
					}
					else
						_progressBar.Style = ProgressBarStyle.Marquee;

				});
		}

		public void Complete()
		{
			_asyncOp.PostOperationCompleted(Dispose);
		}

		public bool CancelRequested
		{
			get;
			private set;
		}

		#endregion
	}
}
