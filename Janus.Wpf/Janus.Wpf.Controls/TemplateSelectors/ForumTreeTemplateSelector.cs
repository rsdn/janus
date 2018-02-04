using Janus.Model.TreeView;
using System.Windows;

namespace Janus.Wpf.Controls.TemplateSelectors {
	class ForumTreeTemplateSelector : BaseDataTemplateSelector {
		private object _TemplateKey = typeof(ITreeNode);

		public object TemplateKey {
			get { return _TemplateKey; }
			set { _TemplateKey = value; }
		}

		public override DataTemplate SelectTemplate(object item, DependencyObject container) {
			if (item is ITreeNode) {
				return FindResource(container, item.GetType()) as DataTemplate ?? FindResource(container, _TemplateKey) as DataTemplate ?? base.SelectTemplate(item, container);
			}
			else {
				return FindResource(container, _TemplateKey) as DataTemplate ?? base.SelectTemplate(item, container);
			}
		}

	}
}
