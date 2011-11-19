using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Peak.TreeGrid
{
	/// <summary>
	/// Column of TreeGrid.
	/// </summary>
	[DesignTimeVisible(false)]
	public class Column : Component
	{
		public Column()
		{
			Width = 100;
			HeadingAlignment = HorizontalAlignment.Left;
			Alignment = HorizontalAlignment.Left;
		}

		internal ColumnCollection columns;

		private string headingText = string.Empty;
		private Image image = null;
		private string dataPropertyName = string.Empty;
		private int width = 100;
		private HorizontalAlignment headingAlignment = HorizontalAlignment.Left;
		private HorizontalAlignment alignment = HorizontalAlignment.Left;
		private SortOrder sortOrder = SortOrder.None;

		[DefaultValue("")]
		[DisplayName("Heading text")]
		public string HeadingText
		{
			get { return headingText; }
			set
			{
				headingText = value;
				Changed();
			}
		}

		[DefaultValue("")]
		[DisplayName("Data property name")]
		public string DataPropertyName
		{
			get { return dataPropertyName; }
			set
			{
				dataPropertyName = value;
				Changed();
			}
		}

		[DefaultValue(null)]
		public Image Image
		{
			get { return image; }
			set
			{
				image = value;
				Changed();
			}
		}

		[DefaultValue(SortOrder.None)]
		[DisplayName("Sort order")]
		public SortOrder SortOrder
		{
			get { return sortOrder; }
			set
			{
				sortOrder = value;
				Changed();
			}
		}

		[DefaultValue(100)]
		public int Width
		{
			get { return width; }
			set
			{
				width = value;
				Changed();
			}
		}

		[DefaultValue(HorizontalAlignment.Left)]
		public HorizontalAlignment HeadingAlignment {
			get { return headingAlignment; }
			set
			{
				headingAlignment = value;
				Changed();
			}
		}

		[DefaultValue(HorizontalAlignment.Left)]
		public HorizontalAlignment Alignment
		{
			get { return alignment; }
			set
			{
				alignment = value;
				Changed();
			}
		}

		protected void Changed()
		{
			if (columns != null && columns.owner != null)
			{
				TreeGrid1.FillGridColumn(this, columns.owner.realGrid.Columns[columns.IndexOf(this)]);
			}
		}

		Dictionary<VisualInfo, StyleInfo> styles = new Dictionary<VisualInfo, StyleInfo>();

		[Browsable(false)]
		public virtual IIndexable<VisualInfo, StyleInfo> Styles
		{
			get
			{
				return new DelegateIndexable<VisualInfo, StyleInfo>(
					(VisualInfo info1) => (styles.ContainsKey(info1) ? styles[info1] : (StyleInfo)null),
					delegate(VisualInfo info2, StyleInfo vle) { styles[info2] = vle; });
			}
		}
	}
}
