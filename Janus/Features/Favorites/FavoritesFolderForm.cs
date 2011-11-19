using System.ComponentModel;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	/// <summary>
	/// UI управление папкой избранного
	/// </summary>
	public partial class FavoritesFolderForm : Form
	{
		#region Constructor(s
		public FavoritesFolderForm(string name, string comment)
			: this(name, comment, false)
		{}

		public FavoritesFolderForm(string name, string comment, bool showAsRootButton)
		{
			InitializeComponent();

			FolderName = name;
			FolderComment = comment;
			_isRootCheckBox.Visible = showAsRootButton;
		}
		#endregion

		#region Properties
		public string FolderName
		{
			get { return _nameBox.Text; }
			set { _nameBox.Text = value; }
		}

		public string FolderComment
		{
			get { return _commentBox.Text; }
			set { _commentBox.Text = value; }
		}

		[Browsable(false)]
		public bool CreateAsRoot
		{
			get { return _isRootCheckBox.Checked; }
		}
		#endregion
	}
}