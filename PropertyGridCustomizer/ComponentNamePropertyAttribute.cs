using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Указывает свойство, содержащее имя компонента.
	/// </summary>
	[AttributeUsage(AttributeTargets.Class)]
	public class ComponentNamePropertyAttribute : Attribute
	{
		private readonly string _propertyName;

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public ComponentNamePropertyAttribute(string propertyName)
		{
			_propertyName = propertyName;
		}

		/// <summary>
		/// Имя свойства.
		/// </summary>
		public string PropertyName
		{
			get { return _propertyName; }
		}
	}
}