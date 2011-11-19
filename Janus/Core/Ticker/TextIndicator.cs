using System.Drawing;

namespace Rsdn.Janus
{
	/// <summary>
	/// Текстовый индикатор
	/// </summary>
	public class TextIndicator : TickerIndicatorBase
	{
		public TextIndicator()
		{
			ForeColor = Color.Lime;
		}

		private bool _on;
		public bool On
		{
			get {return _on;}
			set 
			{
				_on = value;
				Owner.RedrawIndicator(this);
			}
		}

		public override void Draw(Graphics g)
		{
			Color c;
			Image img;
			if(On)
			{
				c = ForeColor;
				img = OnImage;
			}
			else
			{
				c = Color.FromArgb(ForeColor.R / 3, ForeColor.G / 3, ForeColor.B / 3);
				img = OffImage;
			}
			using(var txtbr = new SolidBrush(c))
			using(var bkbr = new SolidBrush(BackColor))
			{
				g.FillRectangle(bkbr, 0, 0, Size.Width, Size.Height);
				g.DrawString(Text, Font, txtbr, 0, 0);
				var ts = g.MeasureString(Text, Font);
				if (img != null)
					g.DrawImage(img, (int) ts.Width + 2, 0, img.Width, img.Height);
			}
		}

		private string _text = "";
		public string Text 
		{
			get {return _text;}
			set 
			{
				_text = value;
				/*using(Graphics g = Owner.CreateGraphics())
				{
					SizeF ts = g.MeasureString(Text,Font);
					Size = new Size((int)ts.Width, (int)ts.Height); 
				}*/
				Owner.RedrawIndicator(this);
			}
		}

		private Font _font = new Font("Times New Roman", 6);
		public Font Font
		{
			get {return _font;}
			set 
			{
				_font = value;
				Owner.RedrawIndicator(this);
			}
		}

		public Image OffImage { get; set; }

		public Image OnImage { get; set; }
	}
}
