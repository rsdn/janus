using System.Windows;

namespace Janus.Wpf.Controls.TemplateSelectors {
	class ImageKeySelector : BaseDataTemplateSelector {
		public override DataTemplate SelectTemplate(object item, DependencyObject container) {
			var res = FindResource(container, item);
			return res as DataTemplate;
		}
	}
}
