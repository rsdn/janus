using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace Rsdn.Janus
{
	/// <summary>
	/// обертка редактора.
	/// </summary>
	internal class EditorWrapper : UITypeEditor
	{
		private readonly UITypeEditor _parentEditor;

		public EditorWrapper(UITypeEditor parentEditor)
		{
			_parentEditor = parentEditor;
		}

		public override object EditValue(ITypeDescriptorContext context,
			IServiceProvider provider, object value)
		{
			if (context != null)
				return _parentEditor.EditValue(CreateContext(context), provider, value);

			return base.EditValue(context, provider, value);
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			if (context != null)
				return _parentEditor.GetEditStyle(CreateContext(context));

			return base.GetEditStyle(context);
		}

		public override bool GetPaintValueSupported(ITypeDescriptorContext context)
		{
			return _parentEditor.GetPaintValueSupported(CreateContext(context));
		}

		public override void PaintValue(PaintValueEventArgs e)
		{
			_parentEditor.PaintValue(new PaintValueEventArgs(
				CreateContext(e.Context), e.Value, e.Graphics, e.Bounds));
		}

		private static ITypeDescriptorContext CreateContext(ITypeDescriptorContext parentContext)
		{
			var wrapper = parentContext.Instance as DisplayNameWrapper;
			return wrapper != null ? new DescriptorContext(
				wrapper.WrappedInstance,
				parentContext.PropertyDescriptor, parentContext) : parentContext;
		}
	}
}