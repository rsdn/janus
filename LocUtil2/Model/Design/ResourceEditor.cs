using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Rsdn.LocUtil.Model.Design
{
	/// <summary>
	/// Редактор ресурса.
	/// </summary>
	public class ResourceEditor : UITypeEditor
	{
		/// <summary>
		/// Смотри <see cref="UITypeEditor.GetEditStyle(ITypeDescriptorContext)"/>
		/// </summary>
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		/// <summary>
		/// Смотри <see cref="UITypeEditor.EditValue(ITypeDescriptorContext, IServiceProvider, object)"/>
		/// </summary>
		public override object EditValue(ITypeDescriptorContext context,
			IServiceProvider provider, object value)
		{
			IEditContext ec = (IEditContext)provider.GetService(typeof (IEditContext));
			using (ResourceEditorForm refr = new ResourceEditorForm(context.PropertyDescriptor.DisplayName))
			{
				refr.ResourceText = (string)value;
				if (refr.ShowDialog(ec.DialogsOwner) == DialogResult.OK)
					return refr.ResourceText;
			}
			return base.EditValue (context, provider, value);
		}
	}
}
