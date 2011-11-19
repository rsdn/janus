using System;

namespace Rsdn.Janus
{
	public abstract class MenuItem : IMenuItem, IEquatable<MenuItem>
	{
		private readonly int _orderIndex;

		protected MenuItem(int orderIndex)
		{
			_orderIndex = orderIndex;
		}

		public abstract void AcceptVisitor<TContext>(IMenuItemVisitor<TContext> visitor, TContext context);

		public int OrderIndex
		{
			get { return _orderIndex; }
		}

		public bool Equals(MenuItem obj)
		{
			if (ReferenceEquals(null, obj))
				return false;
			if (ReferenceEquals(this, obj))
				return true;
			if (obj.GetType() != GetType())
				return false;
			return obj._orderIndex == _orderIndex;
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as MenuItem);
		}

		public override int GetHashCode()
		{
			return _orderIndex;
		}
	}
}