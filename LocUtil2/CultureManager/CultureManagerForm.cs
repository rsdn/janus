using System.Windows.Forms;
using System.Globalization;
using System.Collections.Generic;
using System;
using System.Drawing;

namespace Rsdn.LocUtil
{
	internal partial class CultureManagerForm : Form
	{
		private readonly NotifiedList<CultureInfo> _cultures;

		public CultureManagerForm(CultureInfo[] cultures)
		{
			InitializeComponent();

			_cultures = new NotifiedList<CultureInfo>(cultures);
			_cultures.SizeChanged += _cultures_SizeChanged;
			SetListSize();
		}

		public CultureInfo[] Cultures
		{
			get { return _cultures.ToArray(); }
		}

		private void _cultures_SizeChanged(object sender, EventArgs e)
		{
			SetListSize();
			_cultures.Sort(
				delegate(CultureInfo x, CultureInfo y)
				{
					return x.Name.CompareTo(y.Name);
				});
		}

		private void SetListSize()
		{
			_culturesList.VirtualListSize = _cultures.Count;
			SetDelButtonState();
		}

		private void _culturesList_RetrieveVirtualItem(object sender, RetrieveVirtualItemEventArgs e)
		{
			CultureInfo ci = _cultures[e.ItemIndex];
			string[] items;
			Color c;
			if (ci == CultureInfo.InvariantCulture)
			{
				items = new string[] { "(default)", "Default Culture" };
				c = Color.Gray;
			}
			else
			{
				items = new string[] { ci.Name, ci.DisplayName };
				c = Color.Black;
			}
			e.Item = new ListViewItem(items,
				ci.IsNeutralCulture || ci == CultureInfo.InvariantCulture ? 0 : 1);
			e.Item.ForeColor = c;
		}

		private void _culturesList_SelectedIndexChanged(object sender, EventArgs e)
		{
			SetDelButtonState();
		}

		private void SetDelButtonState()
		{
			bool state = false;
			if (_culturesList.SelectedIndices.Count > 0)
			{
				state = true;
				foreach (int idx in _culturesList.SelectedIndices)
					if (_cultures[idx] == CultureInfo.InvariantCulture)
					{
						state = false;
						break;
					}
			}
			_removeButton.Enabled = state;
		}

		private void _addButton_Click(object sender, EventArgs e)
		{
			using (NewCultureForm ncf = new NewCultureForm(_cultures))
				if (ncf.ShowDialog(this) == DialogResult.OK)
				{
					_cultures.Add(ncf.SelectedCulture);
				}
		}

		private void _removeButton_Click(object sender, EventArgs e)
		{
			int[] sis = new int[_culturesList.SelectedIndices.Count];
			_culturesList.SelectedIndices.CopyTo(sis, 0);
			Array.Sort(sis);
			foreach (int i in sis)
			{
				_cultures.RemoveAt(i);
				for (int j = i; j < sis.Length; j++)
					sis[j]--;
			}
		}

		private void _culturesList_VirtualItemsSelectionRangeChanged(object sender, ListViewVirtualItemsSelectionRangeChangedEventArgs e)
		{
			SetDelButtonState();
		}
	}
}