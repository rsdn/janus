using System;
using System.ComponentModel;
using System.ComponentModel.Design.Serialization;
using System.Globalization;
using System.Reflection;

namespace Rsdn.Shortcuts
{
	/// <summary>
	/// Summary description for ShortcutConverter.
	/// </summary>
	internal class ShortcutConverter : TypeConverter
	{
		public override bool CanConvertTo(
			ITypeDescriptorContext context, Type destinationType)
		{
			return destinationType == typeof (InstanceDescriptor) || base.CanConvertTo(context, destinationType);
		}

		public override object ConvertTo(ITypeDescriptorContext context,
			CultureInfo culture, object value, Type destinationType)
		{
			if (destinationType == null)
				throw new ArgumentNullException("destinationType");

			if (destinationType == typeof (InstanceDescriptor)
				&& value is CustomShortcut)
			{
				MemberInfo mi;
				object[] arguments;

				var cs = (CustomShortcut)value;

				if (cs.Nodes.Count == 0)
				{
					var typeConstruct =
						new[] {typeof (Type), typeof (string)};

					mi = typeof (CustomShortcut)
						.GetConstructor(typeConstruct);

					arguments = new object[] {cs.OwnerType, cs.Name};
				}
				else
				{
					var typeConstruct = new[]
					{
						typeof (Type), typeof (string), typeof (CustomShortcut[])
					};

					mi = typeof (CustomShortcut)
						.GetConstructor(typeConstruct);

					var csRange =
						new CustomShortcut[checked((uint)cs.Nodes.Count)];

					cs.Nodes.CopyTo(csRange, 0);

					arguments = new object[] {cs.OwnerType, cs.Name, csRange};
				}

				return new InstanceDescriptor(mi, arguments);
			}

			return base.ConvertTo(context, culture, value, destinationType);
		}
	}
}