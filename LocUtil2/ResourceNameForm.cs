using System.Windows.Forms;
using Microsoft.CSharp;

namespace Rsdn.LocUtil
{
	/// <summary>
	/// Форма ввода имени ресурса.
	/// </summary>
	internal partial class ResourceNameForm : Form
	{
		private const char _namespaceSeparator = '.';

		internal ResourceNameForm()
		{
			// Required for Windows Form Designer support
			InitializeComponent();
		}

		/// <summary>
		/// Введенное имя.
		/// </summary>
		public string ResourceName
		{
			get { return _nameBox.Text; }
			set
			{
				_nameBox.Text = value;
				_nameBox.Select(value.Length, 1);
			}
		}

		private void _nameBox_TextChanged(object sender, System.EventArgs e)
		{
			_okButton.Enabled = IsResourceNameValid(_nameBox.Text);
		}

		private bool IsResourceNameValid(string name)
		{
			if (string.IsNullOrEmpty(name))
				return false;
			if (name[name.Length - 1] == _namespaceSeparator)
				return false;
			string[] parts = name.Split(_namespaceSeparator);
			if (parts.Length == 0)
				return false;
			var prov = new CSharpCodeProvider();
			foreach (string part in parts)
				if (!prov.IsValidIdentifier(part))
					return false;
			return true;
		}
	}
}
