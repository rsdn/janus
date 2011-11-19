using System;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	/// <summary>
	/// Редактор <see cref="TagLineInfos"/>.
	/// </summary>
	internal class TagLineInfoEditor : UITypeEditor
	{
		public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context)
		{
			return UITypeEditorEditStyle.Modal;
		}

		public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value)
		{
			using (var tllf = new TagLineListForm(
					ApplicationManager.Instance.ServiceProvider, ((TagLineInfos)value).Infos))
			{
				if (tllf.ShowDialog(ApplicationManager.Instance.MainForm) == DialogResult.OK)
				{
					var tgis = new TagLineInfos { Infos = tllf.TagLines.ToArray() };
					return tgis;
				}
				return value;
			}
		}
	}
}
