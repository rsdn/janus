using System;
using System.Windows.Forms;

using CodeJam.Services;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for RateForm.
	/// </summary>
	public partial class RateForm : Form
	{
		private readonly IServiceProvider _provider;

		public RateForm(IServiceProvider provider, MessageRates rate)
		{
			_provider = provider;
			InitializeComponent();
			CustomInitializeComponents(rate);
		}

		private void CustomInitializeComponents(MessageRates rate)
		{
			_lblInfo.Text = SR.Forum.RateForm.AddRate;

			var styler = _provider.GetRequiredService<IStyleImageManager>();
			switch (rate)
			{
				case MessageRates.Plus1:
					_pbxRateImage.Image = styler.GetImage(
						"MessageViewer\\RatePlus1", StyleImageType.Small);
					break;
				case MessageRates.Agree:
					_pbxRateImage.Image = styler.GetImage(
						"MessageViewer\\RateAgree", StyleImageType.Small);
					break;
				case MessageRates.DisAgree:
					_pbxRateImage.Image = styler.GetImage(
						"MessageViewer\\RateDisagree", StyleImageType.Small);
					break;
				case MessageRates.Rate1:
					_pbxRateImage.Image = styler.GetImage(
						"MessageViewer\\Rate1", StyleImageType.Small);
					break;
				case MessageRates.Rate2:
					_pbxRateImage.Image = styler.GetImage(
						"MessageViewer\\Rate2", StyleImageType.Small);
					break;
				case MessageRates.Rate3:
					_pbxRateImage.Image = styler.GetImage(
						"MessageViewer\\Rate3", StyleImageType.Small);
					break;
				case MessageRates.Smile:
					_pbxRateImage.Image = styler.GetImage(
						"MessageViewer\\RateSmile", StyleImageType.Small);
					break;
				case MessageRates.DeleteRate:
					_pbxRateImage.Image = styler.GetImage(
						"MessageViewer\\RateX", StyleImageType.Small);
					_lblInfo.Text = SR.Forum.RateForm.DeleteRate;
					break;
				case MessageRates.DeleteLocally:
					_lblInfo.Text = SR.Forum.RateForm.DeleteRateLocally;
					break;
			}
		}
	}
}