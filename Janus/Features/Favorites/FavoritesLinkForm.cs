using System;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for FavoritesItemForm.
	/// </summary>
	public partial class FavoritesLinkForm : Form
	{
		#region Constructor(s) & Dispose

		public FavoritesLinkForm()
		{
			InitializeComponent();
		}

		public FavoritesLinkForm(string url, string comment)
			: this()
		{
			Url = url;
			Comment = comment;
		}
		#endregion

		#region Properties

		/// <summary>
		/// Ссылка
		/// </summary>
		public string Url
		{
			get { return _urlTextBox.Text.Trim(); }
			set
			{
				//TODO оптимизировать
				if(value.StartsWith("janus://message/"))
					_urlTextBox.ReadOnly = true;

				_urlTextBox.Text = value;
			}
		}

		/// <summary>
		/// Комментарий
		/// </summary>
		public string Comment
		{
			get { return _commentTextBox.Text.Trim(); }
			set { _commentTextBox.Text = value; }
		}

		#endregion

		#region Control events

		private void _okButton_Click(object sender, EventArgs e)
		{
			if (_urlTextBox.Text.Trim().Length == 0)
			{
				MessageBox.Show(this, SR.Favorites.EmptyLink,
					ApplicationInfo.ApplicationName,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}
			//TODO оптимизировать проверку
			if (!(_urlTextBox.Text.Trim().StartsWith("janus://") 
				|| _urlTextBox.Text.Trim().StartsWith("http://")))
			{
				MessageBox.Show(this, SR.Favorites.BadProtocol,
					ApplicationInfo.ApplicationName,
					MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			DialogResult = DialogResult.OK;
		}

		#endregion
	}
}