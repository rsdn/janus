using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Peak.TreeGrid.Tests
{
	public partial class Form1 : Form
	{
		private StringTreeGrid objectTreeGrid1;

		public Form1()
		{
			InitializeComponent();
			this.objectTreeGrid1 = new StringTreeGrid();
			this.column1 = new Peak.TreeGrid.Column();
			this.column2 = new Peak.TreeGrid.Column();
			// 
			// objectTreeGrid1
			// 
			this.objectTreeGrid1.Columns.Add(this.column1);
			this.objectTreeGrid1.Columns.Add(this.column2);
			this.objectTreeGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.objectTreeGrid1.HeaderHeight = 25;
			this.objectTreeGrid1.Location = new System.Drawing.Point(0, 25);
			this.objectTreeGrid1.Name = "objectTreeGrid1";
			this.objectTreeGrid1.Size = new System.Drawing.Size(636, 402);
			this.objectTreeGrid1.TabIndex = 0;
			// 
			// column1
			// 
			this.column1.DataPropertyName = "Name";
			this.column1.HeadingText = "Заголовок";
			this.column1.Image = global::Peak.TreeGrid.Tests.Properties.Resources.arrowleft_green16;
			this.column1.Width = 300;
			// 
			// column2
			// 
			this.column2.DataPropertyName = "Attributes";
			this.column2.HeadingText = "Автор";
			this.column2.Image = global::Peak.TreeGrid.Tests.Properties.Resources.arrowright_green16;

			this.Controls.Add(this.objectTreeGrid1);

			objectTreeGrid1.BringToFront();
			objectTreeGrid1.TreeModel = new StringTreeModel();
		}
	}
}
