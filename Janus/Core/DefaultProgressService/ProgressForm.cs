using System;
using System.ComponentModel;
using System.Windows.Forms;

using Rsdn.SmartApp;

namespace Rsdn.Janus.Framework
{
	internal partial class ProgressForm : Form
	{
		private readonly IServiceProvider _serviceProvider;
		private readonly AsyncOperation _asyncOp;

		public ProgressForm(IServiceProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");

			_serviceProvider = provider;

			_asyncOp = AsyncHelper.CreateOperation();

			InitializeComponent();
		}

		private void _flowLayoutPanel_ControlRemoved(object sender, ControlEventArgs e)
		{
			if (_flowLayoutPanel.Controls.Count == 0)
				Hide();
		}

		public IProgressVisualizer CreateVisualizer(bool allowCancel)
		{
			DefaultProgressVisualizer visualizer = null;

			_asyncOp.Send(
				() =>
				{
					visualizer = new DefaultProgressVisualizer(allowCancel);
					_flowLayoutPanel.Controls.Add(visualizer);
				});

			_asyncOp.Post(
				() =>
				{
					if (!Visible)
						ShowDialog(
							_serviceProvider
								.GetRequiredService<IUIShell>()
								.GetMainWindowParent());
				});

			return visualizer;
		}
	}
}