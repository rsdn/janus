using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;

namespace Rsdn.Janus
{
	internal class PropertyDescriptorWrapper : PropertyDescriptor
	{
		private readonly PropertyDescriptor _propDesc;
		private readonly DisplayNameWrapper _wrapper;

		public PropertyDescriptorWrapper(PropertyDescriptor propDesc) : base(propDesc)
		{
			_propDesc = propDesc;
		}

		public PropertyDescriptorWrapper(DisplayNameWrapper wrapper,
			PropertyDescriptor propDesc) : this(propDesc)
		{
			_wrapper = wrapper;
		}

		public override string Category
		{
			get { return _propDesc.Category; }
		}

		// Это свойство возвращает название свойства 
		// отображаемое в propertyGrid
		public override string DisplayName
		{
			get
			{
				// Пытаемся получить отрибут DisplayNameAttribute.
				// В случае неудачи будет возвращен null.
				var mna = _propDesc.Attributes[typeof (DisplayNameAttribute)] as DisplayNameAttribute;
				if (mna != null)
					// Если имеется атрибут DisplayNameAttribute
					// возвращаем текст помещенный в него.
					return mna.DisplayName;
				// Если атрибут DisplayNameAttribute не задан,
				// возвращаем оригинальное имя свойства.
				return _propDesc.Name;
			}
		}

		public override Type ComponentType
		{
			get { return _propDesc.ComponentType; //typeof(ResHolder);
			}
		}

		public override bool IsReadOnly
		{
			get { return _propDesc.IsReadOnly; }
		}

		public override Type PropertyType
		{
			get { return _propDesc.PropertyType; }
		}

		public override TypeConverter Converter
		{
			get
			{
				TypeConverter conv;
				if (typeof (IList).IsAssignableFrom(_propDesc.PropertyType))
					conv = new CollectionTypeConverter();
				else if (_propDesc.PropertyType.IsEnum)
					conv = new EnumTypeConverter(_propDesc.PropertyType);
				else conv = _propDesc.PropertyType == typeof (bool) ? new BooleanTypeConverter() : base.Converter;
				return new ConverterWrapper(conv);
			}
		}

		/// <summary>
		/// Коллекция атрибутов.
		/// </summary>
		public override AttributeCollection Attributes
		{
			get
			{
				var attrs = base.Attributes;
				if (_wrapper == null)
					return attrs;
				var al = new ArrayList(attrs.Count);
				foreach (Attribute attr in attrs)
				{
					var e = new CheckAttributeEventArgs(this, attr);
					_wrapper.OnCheckAttribute(e);
					if (e.Checked)
						al.Add(attr);
				}
				return new AttributeCollection((Attribute[])al.ToArray(typeof (Attribute)));
			}
		}

		public override bool CanResetValue(object component)
		{
			if (component is DisplayNameWrapper)
				return _propDesc.CanResetValue(((DisplayNameWrapper)component).WrappedInstance);
			return _propDesc.CanResetValue(component);
		}

		public override object GetValue(object component)
		{
			if (component is DisplayNameWrapper)
				return _propDesc.GetValue(((DisplayNameWrapper)component).WrappedInstance);
			return _propDesc.GetValue(component);
		}

		public override void ResetValue(object component)
		{
			if (component is DisplayNameWrapper)
				_propDesc.ResetValue(((DisplayNameWrapper)component).WrappedInstance);
			else
				_propDesc.ResetValue(component);
		}

		public override void SetValue(object component, object value)
		{
			if (component is DisplayNameWrapper)
				_propDesc.SetValue(((DisplayNameWrapper)component).WrappedInstance, value);
			else
				_propDesc.SetValue(component, value);
		}

		public override bool ShouldSerializeValue(object component)
		{
			if (component is DisplayNameWrapper)
				return _propDesc.ShouldSerializeValue(((DisplayNameWrapper)component).WrappedInstance);
			return _propDesc.ShouldSerializeValue(component);
		}

		public override object GetEditor(Type editorBaseType)
		{
			if (editorBaseType == typeof (UITypeEditor))
				if (typeof (IList).IsAssignableFrom(_propDesc.PropertyType) &&
					(_propDesc.PropertyType != typeof (string[])))
					return new ComponentCollectionEditor();
				else
				{
					var editor = (UITypeEditor)_propDesc.GetEditor(editorBaseType);
					if (editor == null)
						return null;
					return new EditorWrapper(editor);
				}
			return _propDesc.GetEditor(editorBaseType);
		}
	}
}