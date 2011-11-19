using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing.Design;

namespace Rsdn.Janus
{
	/// <summary>
	/// Редактор коллекций компонент, поддерживающий атрибут DisplayName
	/// </summary>
	internal class ComponentCollectionEditor : UITypeEditor
	{
		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider,
			object value)
		{
			var frm = new ComponentCollectionEditorForm((IList)value);
			frm.ShowDialog();
			return frm.Collection;
		}

		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}