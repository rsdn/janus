using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;

namespace Rsdn.Shortcuts
{
	[Serializable]
	[Editor(typeof (NodesUITypeEditor), typeof (UITypeEditor))]
	public class ShortcutCollection : CollectionBase,
		IEnumerable<CustomShortcut>,
		ICloneable
	{
		private readonly CustomShortcut _owner;

		public ShortcutCollection(CustomShortcut owner)
		{
			_owner = owner;
		}

		public CustomShortcut this[int index]
		{
			get { return ((CustomShortcut)List[index]); }
		}

		#region ICloneable Members
		public object Clone()
		{
			var scl = new ShortcutCollection(_owner);

			foreach (CustomShortcut cs in List)
				scl.Add((CustomShortcut)cs.Clone());

			return scl;
		}
		#endregion

		#region IEnumerable<CustomShortcut> Members
		IEnumerator<CustomShortcut> IEnumerable<CustomShortcut>.GetEnumerator()
		{
			foreach (CustomShortcut shortcut in InnerList)
				yield return shortcut;
		}
		#endregion

		public CustomShortcut Add(CustomShortcut value)
		{
			List.Add(value);
			return value;
		}

		public void AddRange(IEnumerable<CustomShortcut> values)
		{
			foreach (var sc in values)
				Add(sc);
		}

		public void Remove(CustomShortcut value)
		{
			value.Parent = null;
			List.Remove(value);
		}

		public void Insert(int index, CustomShortcut value)
		{
			value.Parent = _owner;
			List.Insert(index, value);
		}

		public bool Contains(CustomShortcut value)
		{
			return List.Contains(value);
		}

		public bool Contains(ShortcutCollection values)
		{
			foreach (CustomShortcut sc in values)
				if (Contains(sc))
					return true;

			return false;
		}

		public int IndexOf(CustomShortcut value)
		{
			return List.IndexOf(value);
		}

		public void CopyTo(Array dest, int index)
		{
			for (var i = 0; i < Count; i++)
				dest.SetValue(List[i], i);
		}

		protected override void OnValidate(object value)
		{
			base.OnValidate(value);

			if (!(value is CustomShortcut))
				throw new ArgumentException(string.Format(
					"The value '{0}' is not of type '{1}' and cannot be used "
						+ "in this generic collection.",
					value.GetType(), typeof (CustomShortcut)
					), "value");
		}

		protected override void OnInsert(int index, object value)
		{
			base.OnInsert(index, value);

			((CustomShortcut)value).Parent = _owner;
		}
	}
}