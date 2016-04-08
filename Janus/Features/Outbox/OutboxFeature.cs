using System;
using System.Windows.Forms;

using CodeJam.Services;

using Rsdn.Janus.ObjectModel;
using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for Outbox.
	/// </summary>
	public class OutboxFeature : Feature, IGetData
	{
		private static OutboxFeature _instance;

		public static OutboxFeature Instance => _instance;

		private readonly IServiceProvider _provider;

		public OutboxFeature()
		{
			_instance = this;
			_description = SR.Forum.Outcoming.DisplayName;
			_provider = ApplicationManager.Instance.ServiceProvider;
		}

		public override string Key => "Outbox";

		public override int ImageIndex => Features.Instance.OutboxImageIndex;

		#region IFeatureGui

		protected override Control CreateGuiControl()
		{
			return
				(Control)_provider
					.GetRequiredService<IOutboxManager>()
					.OutboxForm;
		}

		#endregion IFeatureGui

		#region IGetData
		void IGetData.GetData(NodeInfo nodeInfo, CellInfo[] cellData)
		{
			var om = _provider.GetRequiredService<IOutboxManager>();
			cellData[0].Text = Description;
			cellData[0].ImageIndex = ImageIndex;
			cellData[1].Text = $"{(om.NewMessages.Count == 0 ? string.Empty : om.NewMessages.Count.ToString())}/{(om.RateMarks.Count == 0 ? string.Empty : om.RateMarks.Count.ToString())}/{(om.DownloadTopics.Count == 0 ? string.Empty : om.DownloadTopics.Count.ToString())}";

			nodeInfo.Highlight = (om.NewMessages.Count > 0)
				|| (om.RateMarks.Count > 0) || (om.DownloadTopics.Count > 0);
		}
		#endregion IGetData
	}
}