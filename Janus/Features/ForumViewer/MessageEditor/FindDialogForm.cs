using System.Windows.Forms;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for FindDialogForm.
	/// </summary>
	internal partial class FindDialogForm : Form
	{
		#region Costructor(s)

		public FindDialogForm()
		{
			InitializeComponent();
		}

		#endregion

		#region Public properties

		public string TextToFind
		{
			get { return _findStrBox.Text; }
		}

		public bool MatchCase
		{
			get { return _matchCaseCheckBox.Checked; }
		}

		public bool MatchWholeWord
		{
			get { return _matchWholeWordCheckBox.Checked; }
		}

		public bool MatchWordStart
		{
			get { return _matchWordStartCheckBox.Checked; }
		}

		public bool Reverse
		{
			get { return _reverseSearchCheckBox.Checked; }
		}

		public bool UseRegex
		{
			get { return _useRegExCheckBox.Checked; }
		}

		public bool SelectionOnly
		{
			get { return _selOnlyRadioBtn.Checked; }
		}

		#endregion
	}
}