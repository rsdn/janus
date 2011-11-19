using System;
using System.Collections;
using System.ComponentModel;
using System.Resources;

using Rsdn.PropGridCust;

namespace Rsdn.Janus
{
	/// <summary>
	/// Класс, оборачивающий экземпляры для подмены отображаемых имен.
	/// </summary>
	public sealed class DisplayNameWrapper : ICustomTypeDescriptor
	{
		// Оборачиваемый объект.
		private readonly PropertyDescriptor _componentNameProperty;
		private readonly string _componentTypeName;
		private readonly PropertyDescriptor _propertyDescriptor;
		// Коллекция хранящая обертки над описаниями свойств.
		private readonly PropertyDescriptorCollection _propsCollection;
		private readonly IServiceProvider _provider;
		private readonly object _wrappedInstance;

		static DisplayNameWrapper()
		{
			SR.ResourceManager = new ResourceManager("Rsdn.Janus.SR",
				typeof (DisplayNameWrapper).Assembly);
		}

		/// <summary>
		/// Инициализирует экземпляр оборачиваемым объектом.
		/// </summary>
		public DisplayNameWrapper(object obj) : this(obj, null)
		{}

		/// <summary>
		/// Инициализирует экземпляр оборачиваемым объектом и поставщиком сервисов.
		/// </summary>
		private DisplayNameWrapper(object obj, IServiceProvider provider)
			: this(obj, provider, null)
		{}

		/// <summary>
		/// Инициализирует экземпляр оборачиваемым объектом, поставщиком сервисов и
		/// описателем свойства, из которого получен объект.
		/// </summary>
		private DisplayNameWrapper(object obj, IServiceProvider provider,
			PropertyDescriptor propertyDescriptor)
		{
			// Запоминам оборачиваемый объект.
			_wrappedInstance = obj;
			_provider = provider;
			_propertyDescriptor = propertyDescriptor;
			// Создаем новую (пустую) колекцию описаний свойсв 
			// в которую поместим обертки над реальными описаниями.
			_propsCollection = new PropertyDescriptorCollection(null);
			var pdc = GetProperties();
			// Перебираем описания свойств, создаем для каждого 
			// из них обертку и помещаем ее в коллекцию.

			var al = new ArrayList();
			foreach (PropertyDescriptor pd in pdc)
				al.Add(pd);

			al.Sort(0, al.Count, new PropertyGridSortComparer());

			for (var ii = 0; ii < al.Count; ii++)
				_propsCollection.Add(new PropertyDescriptorWrapper(this, (PropertyDescriptor)al[ii]));

			var objType = _wrappedInstance.GetType();
			// Ищем замену имени типа компонента
			var dna = (DisplayNameAttribute)Attribute.GetCustomAttribute(
				objType, typeof (DisplayNameAttribute));
			_componentTypeName = dna == null ? objType.Name : dna.DisplayName;

			// Ищем свойство, заменяющее имя компонента
			var cnpa =
				(ComponentNamePropertyAttribute)
					Attribute.GetCustomAttribute(objType, typeof (ComponentNamePropertyAttribute));
			if (cnpa != null)
				_componentNameProperty = pdc[cnpa.PropertyName];
		}

		/// <summary>
		/// Позволяет получить обернутый объект.
		/// </summary>
		public object WrappedInstance
		{
			get { return _wrappedInstance; }
		}

		/// <summary>
		/// Вызывается для проверки каждого атрибута каждого свойства.
		/// </summary>
		public event CheckAttributeEventHandler CheckAttribute;

		/// <summary>
		/// Вызывает событие CheckAttribute.
		/// </summary>
		internal void OnCheckAttribute(CheckAttributeEventArgs e)
		{
			if (CheckAttribute != null)
				CheckAttribute(this, e);
		}

		private PropertyDescriptorCollection GetProperties()
		{
			var attributes = new Attribute[] {BrowsableAttribute.Yes};
			var context = new DescriptorContext(_wrappedInstance, _propertyDescriptor, _provider);
			var converter = (context.PropertyDescriptor == null)
				? TypeDescriptor.GetConverter(_wrappedInstance)
				: context.PropertyDescriptor.Converter;
			if (converter != null && converter.GetPropertiesSupported(context))
				return converter.GetProperties(context, _wrappedInstance, attributes);
			return TypeDescriptor.GetProperties(_wrappedInstance, attributes);
		}

		#region ICustomTypeDescriptor implementation
		/////////////////////////////////////////////////////////
		/// ICustomTypeDescriptor
		///
		AttributeCollection ICustomTypeDescriptor.GetAttributes()
		{
			return new AttributeCollection(null);
		}

		string ICustomTypeDescriptor.GetClassName()
		{
			return _componentTypeName;
		}

		string ICustomTypeDescriptor.GetComponentName()
		{
			return _componentNameProperty == null
				? null
				: (string)_componentNameProperty.GetValue(_wrappedInstance);
		}

		TypeConverter ICustomTypeDescriptor.GetConverter()
		{
			return null;
		}

		EventDescriptor ICustomTypeDescriptor.GetDefaultEvent()
		{
			return null;
		}


		PropertyDescriptor ICustomTypeDescriptor.GetDefaultProperty()
		{
			return null;
		}

		object ICustomTypeDescriptor.GetEditor(Type editorBaseType)
		{
			return null;
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents()
		{
			return new EventDescriptorCollection(null);
		}

		EventDescriptorCollection ICustomTypeDescriptor.GetEvents(Attribute[] attributes)
		{
			return new EventDescriptorCollection(null);
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties()
		{
			return _propsCollection;
		}

		PropertyDescriptorCollection ICustomTypeDescriptor.GetProperties(Attribute[] attributes)
		{
			return _propsCollection;
		}

		object ICustomTypeDescriptor.GetPropertyOwner(PropertyDescriptor pd)
		{
			return this;
		}
		#endregion
	}
}