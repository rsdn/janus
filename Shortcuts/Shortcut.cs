using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace Rsdn.Shortcuts
{
	using ShortcutToStringMap   = Dictionary<Shortcut, string>;
	using ShortcutToStringEntry = KeyValuePair<Shortcut, string>;

	[Serializable]
	[TypeConverter(typeof (ShortcutConverter))]
	public class CustomShortcut : ICloneable
	{
		#region Private members
		private const BindingFlags _bindingFlags =
			BindingFlags.DeclaredOnly
			| BindingFlags.Instance
			| BindingFlags.Static
			| BindingFlags.Public
			| BindingFlags.NonPublic;

		private readonly ShortcutToStringMap _shortcuts =
			new ShortcutToStringMap();

		private ShortcutCollection _collection;
		private string _name;
		private Type _owner;
		private CustomShortcut _parent;
		#endregion

		#region Constructors
		public CustomShortcut(Type owner, string name)
		{
			_owner = owner;
			_name = name;

			Initialize(owner, name);
		}

		public CustomShortcut(Type owner, string name,
			IEnumerable<CustomShortcut> children) : this(owner, name)
		{
			Nodes.AddRange(children);
		}
		#endregion

		#region Public Properties
		[DesignerSerializationVisibility(
			DesignerSerializationVisibility.Content)]
		public ShortcutCollection Nodes
		{
			get
			{
				if (_collection == null)
					_collection = new ShortcutCollection(this);

				return _collection;
			}
		}

		public ShortcutToStringMap HashTable
		{
			get { return _shortcuts; }
		}

		public string Name
		{
			get { return _name; }
			set { _name = value; }
		}

		public CustomShortcut Parent
		{
			get { return _parent; }
			set { _parent = value; }
		}

		public Type OwnerType
		{
			get { return _owner; }
			set { _owner = value; }
		}
		#endregion

		#region ICloneable Members
		public object Clone()
		{
			var cs = new CustomShortcut(_owner, _name);

			foreach (var de in HashTable)
				cs.HashTable.Add(de.Key, de.Value);

			cs.Nodes.AddRange(Nodes);

			return cs;
		}
		#endregion

		private void Initialize(Type owner, string name)
		{
			_parent = null;
			_owner = owner;
			_name = name;

			foreach (var mi in _owner.GetMethods(_bindingFlags))
			{
				var attr = (MethodShortcutAttribute)Attribute
					.GetCustomAttribute(mi, typeof (MethodShortcutAttribute));

				if (attr != null && attr.Shortcut != Shortcut.None)
					_shortcuts.Add(attr.Shortcut, mi.Name);
			}
		}

		private string FindShortcutMethod(Keys key)
		{
			string methodName;
			_shortcuts.TryGetValue((Shortcut)key, out methodName);

			return methodName;
		}

		public bool ProcessMessageKey(Control control,
			Keys keys, ShortcutManager manager)
		{
			var methodName = FindShortcutMethod(keys);

			if (methodName == null)
				return Parent != null && Parent.ProcessMessageKey(control, keys, manager);

			var mi = _owner.GetMethod(methodName, _bindingFlags);
			var ht = new Dictionary<Type, Control> {{OwnerType, control}};

			var target = FindControlHelper.FindControl(control, ht) ?? manager.MainForm;

			if (mi != null)
				try
				{
					mi.Invoke(target, null);
				}
				catch (Exception e)
				{
					throw new ApplicationException(
						"Произошла непредвиденная ошибка при обработке клавиатурного сообщения."
							+ Environment.NewLine
								+ "Возможно у вас повреждена БД или имеется ошибка в RSDN@Home",
						e);
					//MessageBox.Show(Form.ActiveForm,
					//    "Произошла непредвиденная ошибка при обработке клавиатурного сообщения."
					//    + Environment.NewLine
					//    + "Возможно у вас повреждена БД или имеется ошибка в Янусе.",
					//    "RSDN@Home");

					//return false;
				}

			return true;
		}
	}

	internal class FindControlHelper
	{
		/// <summary>
		/// Поиск контрола.
		/// </summary>
		/// <param name="control">Контрол нижнего уровня.</param>
		/// <param name="ht">Хеш-таблица, у которой в качестве 
		/// ключей используются допустимые типы контролов.</param>
		/// <returns>Искомый контрол.</returns>
		public static Control FindControl(Control control, IDictionary ht)
		{
			while (control != null)
			{
				if (ht.Contains(control.GetType()))
					return control;

				if (control.GetType() == typeof (Panel) && control.Tag != null)
					if (ht.Contains(control.Tag.GetType()))
						return (Control)control.Tag;

				control = control.Parent;
			}

			return null;
		}
	}
}