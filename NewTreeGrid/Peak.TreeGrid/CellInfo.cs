using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Xml.Serialization;

namespace Peak.TreeGrid
{
	[Serializable]
	public class CellInfo
	{
		[XmlAttribute("Text")]
		public string Text { get; internal set; }

		public Image Image { get; internal set; }

		public CellInfo(string Text, Image Image)
		{
			this.Text = Text;
			this.Image = Image;
		}
	}
}
