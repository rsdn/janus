using System;
using System.ComponentModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Аргументы собития CheckAttribute.
	/// </summary>
	public class CheckAttributeEventArgs : EventArgs
	{
		private readonly Attribute _attribute;
		private readonly PropertyDescriptor _propertyDecriptor;
		private bool _checked = true;

		/// <summary>
		/// Инициализирует экземпляр атрибутом.
		/// </summary>
		public CheckAttributeEventArgs(PropertyDescriptor propertyDescriptor,
			Attribute attribute)
		{
			_propertyDecriptor = propertyDescriptor;
			_attribute = attribute;
		}

		/// <summary>
		/// Описатель свойства, для которого выполняется проверка атрибутов.
		/// </summary>
		public PropertyDescriptor PropertyDescriptor
		{
			get { return _propertyDecriptor; }
		}

		/// <summary>
		/// Проверяемый атрибут.
		/// </summary>
		public Attribute Attribute
		{
			get { return _attribute; }
		}

		/// <summary>
		/// Признак успешности проверки.
		/// </summary>
		public bool Checked
		{
			get { return _checked; }
			set { _checked = value; }
		}
	}

	/// <summary>
	/// Обработчик события CheckAttributeEvent.
	/// </summary>
	public delegate void CheckAttributeEventHandler(object sender, CheckAttributeEventArgs e);
}