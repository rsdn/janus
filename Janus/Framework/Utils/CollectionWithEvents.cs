using System;

namespace Rsdn.Janus.Framework
{

	#region Event args
	public class BeforeClearEventArgs : EventArgs
	{
		public BeforeClearEventArgs()
		{
			Cancel = false;
		}

		public bool Cancel { get; set; }
	}

	public class ValidateEventArgs<T> : EventArgs
	{
		private readonly T _item;

		public ValidateEventArgs(T item)
		{
			_item = item;
			Cancel = false;
		}

		public T Item
		{
			get { return _item; }
		}

		public bool Cancel { get; set; }
	}

	public class AfterInsertEventArgs<T> : EventArgs
	{
		private readonly int _index;

		private readonly T _item;

		public AfterInsertEventArgs(int index, T item)
		{
			_index = index;
			_item = item;
		}

		public int Index
		{
			get { return _index; }
		}

		public T Item
		{
			get { return _item; }
		}
	}

	public class BeforeInsertEventArgs<T> : AfterInsertEventArgs<T>
	{
		public BeforeInsertEventArgs(int index, T item)
			: base(index, item)
		{
			Cancel = false;
		}

		public bool Cancel { get; set; }
	}

	public class BeforeRemoveEventArgs<T> : BeforeInsertEventArgs<T>
	{
		public BeforeRemoveEventArgs(int index, T item)
			: base(index, item)
		{}
	}

	public class AfterRemoveEventArgs<T> : AfterInsertEventArgs<T>
	{
		public AfterRemoveEventArgs(int index, T item)
			: base(index, item)
		{}
	}

	public class BeforeSetEventArgs<T> : BeforeInsertEventArgs<T>
	{
		private readonly T _oldValue;

		public BeforeSetEventArgs(int index, T oldValue, T item)
			: base(index, item)
		{
			_oldValue = oldValue;
		}

		public T OldValue
		{
			get { return _oldValue; }
		}
	}

	public class AfterSetEventArgs<T> : AfterInsertEventArgs<T>
	{
		private readonly T _oldValue;

		public AfterSetEventArgs(int index, T oldValue, T item)
			: base(index, item)
		{
			_oldValue = oldValue;
		}

		public T OldValue
		{
			get { return _oldValue; }
		}
	}
	#endregion

	public class CollectionWithEvents<T> : CollectionBase<T>
	{
		public CollectionWithEvents()
		{}

		public CollectionWithEvents(int capacity)
			: base(capacity)
		{}

		public event EventHandler<BeforeClearEventArgs> BeforeClear;
		public event EventHandler AfterClear;
		public event EventHandler<ValidateEventArgs<T>> ValidateItem;
		public event EventHandler<BeforeInsertEventArgs<T>> BeforeInsert;
		public event EventHandler<AfterInsertEventArgs<T>> AfterInsert;
		public event EventHandler<BeforeRemoveEventArgs<T>> BeforeRemove;
		public event EventHandler<AfterRemoveEventArgs<T>> AfterRemove;
		public event EventHandler<BeforeSetEventArgs<T>> BeforeSet;
		public event EventHandler<AfterSetEventArgs<T>> AfterSet;

		#region Notification handlers
		protected override bool OnClear()
		{
			if (BeforeClear != null)
			{
				var ea = new BeforeClearEventArgs();
				BeforeClear(this, ea);
				if (ea.Cancel)
					return false;
			}

			return base.OnClear();
		}

		protected override void OnClearComplete()
		{
			if (AfterClear != null)
				AfterClear(this, EventArgs.Empty);

			base.OnClearComplete();
		}

		protected override bool OnValidate(T value)
		{
			if (ValidateItem != null)
			{
				var ea = new ValidateEventArgs<T>(value);
				ValidateItem(this, ea);
				if (ea.Cancel)
					return false;
			}

			return base.OnValidate(value);
		}

		protected override bool OnInsert(int index, T value)
		{
			if (BeforeInsert != null)
			{
				var ea = new BeforeInsertEventArgs<T>(index, value);
				BeforeInsert(this, ea);
				if (ea.Cancel)
					return false;
			}

			return base.OnInsert(index, value);
		}

		protected override void OnInsertComplete(int index, T value)
		{
			if (AfterInsert != null)
				AfterInsert(this, new AfterInsertEventArgs<T>(index, value));

			base.OnInsertComplete(index, value);
		}

		protected override bool OnRemove(int index, T value)
		{
			if (BeforeRemove != null)
			{
				var ea = new BeforeRemoveEventArgs<T>(index, value);
				BeforeRemove(this, ea);
				if (ea.Cancel)
					return false;
			}

			return base.OnRemove(index, value);
		}

		protected override void OnRemoveComplete(int index, T value)
		{
			if (AfterRemove != null)
				AfterRemove(this, new AfterRemoveEventArgs<T>(index, value));

			base.OnRemoveComplete(index, value);
		}

		protected override bool OnSet(int index, T oldValue, T value)
		{
			if (BeforeSet != null)
			{
				var ea = new BeforeSetEventArgs<T>(index, oldValue, value);
				BeforeSet(this, ea);
				if (ea.Cancel)
					return false;
			}

			return base.OnSet(index, oldValue, value);
		}

		protected override void OnSetComplete(int index, T oldValue, T value)
		{
			if (AfterSet != null)
				AfterSet(this, new AfterSetEventArgs<T>(index, oldValue, value));

			base.OnSetComplete(index, oldValue, value);
		}
		#endregion
	}
}