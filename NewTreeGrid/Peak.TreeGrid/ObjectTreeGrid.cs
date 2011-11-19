using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Peak.TreeGrid
{
	[ToolboxItem(true)]
	public partial class ObjectTreeGrid : TreeGrid1<object>
	{
		public ObjectTreeGrid()
		{
			InitializeComponent();
		}
	}
}
