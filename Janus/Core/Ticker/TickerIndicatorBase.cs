using System;
using System.Drawing;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for TickerIndicatorBase.
	/// </summary>
	public abstract class TickerIndicatorBase : ITickerIndicator
	{
		protected ITicker _owner;
		public virtual ITicker Owner
		{
			get {return _owner;}
			set {_owner = value;}
		}

		protected Rectangle bounds = new Rectangle(0,0,20,10);

		public virtual Rectangle Bounds 
		{
			get {return bounds;}
			set {bounds = value;}
		}

		public virtual Point Location
		{
			get {return bounds.Location;}
			set {bounds = new Rectangle(value,bounds.Size);}
		}

		public virtual Size Size 
		{
			get {return bounds.Size;}
			set {bounds = new Rectangle(bounds.Location,value);}
		}

		protected Color foreColor = Color.White;
		public virtual Color ForeColor
		{
			get {return foreColor;}
			set {foreColor = value;}
		}

		protected Color backColor = Color.Black;
		public virtual Color BackColor
		{
			get {return backColor;}
			set {backColor = value;}
		}

		public abstract void Draw(Graphics g);

		protected TickerIndicatorBase()
		{
		}
	}
}
