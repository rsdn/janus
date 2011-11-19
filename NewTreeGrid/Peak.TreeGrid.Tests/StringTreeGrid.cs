using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Peak.TreeGrid.Tests
{
	[ToolboxItem(true)]
	public partial class StringTreeGrid : TreeGrid1<string>
	{
		public StringTreeGrid()
		{
			InitializeComponent();
		}
	}
}
