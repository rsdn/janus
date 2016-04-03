using System.Diagnostics;
using System.Windows.Forms;

using CodeJam;

namespace Rsdn.Janus
{
	internal partial class SP1RequiredForm : JanusBaseForm
	{
		private const string _sp1DownloadUrl =
			"http://www.microsoft.com/downloads/details.aspx?FamilyID=79BC3B77-E02C-4AD3-AACF-A7633F706BA5&displaylang=en";

		public SP1RequiredForm()
		{
			InitializeComponent();
			_messageLabel.Text = _messageLabel.Text.FormatWith(ApplicationInfo.ApplicationName);
		}

		private void LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			Process.Start(_sp1DownloadUrl);
		}
	}
}