using System.ComponentModel;

namespace Rsdn.LocUtil.Model
{
	/// <summary>
	/// Базовый класс для элементов дерева ресурсов.
	/// </summary>
	public abstract class ItemBase
	{
		private readonly Category _parent;
		private readonly string _name;
		private readonly string _shortName;

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public ItemBase(string name, Category parent)
		{
			_parent = parent;
			_name = name;
			if (_name != null)
			{
				string[] parts = _name.Split('.');
				_shortName = parts[parts.Length - 1];
			}
		}

		/// <summary>
		/// Имя.
		/// </summary>
		public string Name
		{
			get { return _name; }
		}

		/// <summary>
		/// Короткое имя.
		/// </summary>
		[Browsable(false)]
		public string ShortName
		{
			get { return _shortName; }
		}

		/// <summary>
		/// Родительская категория.
		/// </summary>
		[Browsable(false)]
		public Category Parent
		{
			get { return _parent; }
		}
	}
}
