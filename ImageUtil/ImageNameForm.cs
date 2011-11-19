using System;
using System.Windows.Forms;

namespace ImageUtil
{
	public partial class ImageNameForm : Form
	{
		public ImageNameForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Имя изображения.
		/// </summary>
		public string ImageName
		{
			get { return _nameBox.Text; }
			set
			{
				_nameBox.Text = value;
				_nameBox.Select(value.Length, 1);
			}
		}

		public event EventHandler<ImageNameChangedEventArgs> ImageNameChanged;

		private void NameBoxTextChanged(object sender, EventArgs e)
		{
			var ea = new ImageNameChangedEventArgs(ImageName);
			if (ImageNameChanged != null)
				ImageNameChanged(this, ea);
			_okButton.Enabled = !ea.Cancel;
		}
	}

	public class ImageNameChangedEventArgs: EventArgs
	{
		private readonly string _imageName;

		public ImageNameChangedEventArgs(string imageName)
		{
			_imageName = imageName;
		}

		public bool Cancel { get; set; }

		public string ImageName
		{
			get { return _imageName; }
		}
	}
}