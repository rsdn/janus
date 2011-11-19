using System;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Rsdn.Shortcuts
{
	/// <summary>
	/// Summary description for NodesUITypeEditor.
	/// </summary>
	internal class NodesUITypeEditor : UITypeEditor
	{
		public override object EditValue(
			ITypeDescriptorContext context,
			IServiceProvider provider,
			object value)
		{
			var service = (IWindowsFormsEditorService)provider
				.GetService(typeof (IWindowsFormsEditorService));

			if (service != null)
			{
				var host = (IDesignerHost)context
					.GetService(typeof (IDesignerHost));

				var trans = host
					.CreateTransaction("NodesCollectionEditor");

				var dialog = new EditorNodes();
				var collection = (ShortcutCollection)value;

				if (collection.Count == 0)
				{
					var root = new CustomShortcut(typeof (Type), "Base");
					collection.Add(root);
				}

				dialog._collection = collection;

				if (service.ShowDialog(dialog) == DialogResult.OK)
				{
					context.OnComponentChanged();
					context.OnComponentChanging();
					trans.Commit();
					return dialog._collection;
				}
				trans.Cancel();
				return value;
			}

			return value;
		}

		public override UITypeEditorEditStyle GetEditStyle(
			ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}
	}
}