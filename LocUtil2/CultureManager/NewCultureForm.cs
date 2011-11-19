using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Globalization;

namespace Rsdn.LocUtil
{
	internal partial class NewCultureForm : Form
	{
		public NewCultureForm(ICollection<CultureInfo> excludeList)
		{
			InitializeComponent();

			Dictionary<string, object> exclude = new Dictionary<string, object>();
			foreach (CultureInfo ci in excludeList)
				exclude[ci.Name] = ci;
			foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
				if (ci.Name.Length != 0 && !exclude.ContainsKey(ci.Name))
					_cultureBox.Items.Add(ci);
		}

		public CultureInfo SelectedCulture
		{
			get { return (CultureInfo)_cultureBox.SelectedItem; }
		}

		private void _cultureBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			_okButton.Enabled = _cultureBox.SelectedItem != null;
		}
	}
}