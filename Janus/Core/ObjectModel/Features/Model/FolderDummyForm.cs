using System;
using System.Windows.Forms;

using Rsdn.Janus.ObjectModel;
using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Дефолтная реализация гуя фичи с вложенными узлами.
	/// </summary>
	public partial class FolderDummyForm : UserControl, IFeatureView
	{
		public FolderDummyForm(IServiceProvider provider, IFeature folderFeature)
		{
			if (provider == null)
				throw new ArgumentNullException("provider");
			if (folderFeature == null)
				throw new ArgumentNullException("folderFeature");

			InitializeComponent();

			_pictureBox.Image = provider
				.GetRequiredService<IStyleImageManager>()
				.GetImage("folder", StyleImageType.ConstSize);
			_label.Text = folderFeature.Description;
		}

		#region IFeatureView Members

		void IFeatureView.Activate(IFeature feature) { }

		void IFeatureView.Refresh() { }

		void IFeatureView.ConfigChanged() { }

		#endregion
	}
}
