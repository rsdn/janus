using System;
using System.ComponentModel;

namespace Rsdn.Janus
{
	internal class DescriptorContext : ITypeDescriptorContext
	{
		private readonly object _instance;
		private readonly PropertyDescriptor _propertyDescriptor;
		private readonly IServiceProvider _serviceProvider;

		public DescriptorContext(object instance, PropertyDescriptor propertyDescriptor,
			IServiceProvider serviceProvider)
		{
			_instance = instance;
			_propertyDescriptor = propertyDescriptor;
			_serviceProvider = serviceProvider;
		}

		#region ITypeDescriptorContext Members
		public bool OnComponentChanging()
		{
			return false;
		}

		public void OnComponentChanged()
		{}

		public IContainer Container
		{
			get { return null; }
		}

		public object Instance
		{
			get { return _instance; }
		}

		public PropertyDescriptor PropertyDescriptor
		{
			get { return _propertyDescriptor; }
		}

		public object GetService(Type serviceType)
		{
			return _serviceProvider == null ? null : _serviceProvider.GetService(serviceType);
		}
		#endregion
	}
}