using System;

using CodeJam.Services;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public partial class LogForm : JanusBaseForm
	{
		private readonly StripMenuGenerator _stripMenuGenerator;

		public LogForm([NotNull] IServiceProvider serviceProvider)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException(nameof(serviceProvider));

			InitializeComponent();

			var styleImageManager = serviceProvider.GetService<IStyleImageManager>();
			if (styleImageManager != null)
				Icon = styleImageManager.GetImage("log", StyleImageType.Small).ToIcon();

			_stripMenuGenerator = new StripMenuGenerator(serviceProvider, _toolStrip, "Janus.Log.Toolbar");

			ApplicationManager.Instance.Logger.OnLog += _logComboBox.AddItem;

			StyleConfig.StyleChange += StyleConfig_StyleChange;
			UpdateStyle();
		}

		public void Clear()
		{
			// Глючный контрол...
			//_logComboBox.Items.Clear();
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				StyleConfig.StyleChange -= StyleConfig_StyleChange;

				_stripMenuGenerator?.Dispose();

				components?.Dispose();
			}
			base.Dispose(disposing);
		}

		private void UpdateStyle()
		{
			_logComboBox.BackColor = StyleConfig.Instance.LogBackColor;
		}

		private void StyleConfig_StyleChange(object s, StyleChangeEventArgs e)
		{
			UpdateStyle();
		}
	}
}

