using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Janus.Wpf.Controls.TemplateSelectors {
	public abstract class BaseDataTemplateSelector : DataTemplateSelector {
		protected static object FindResource(DependencyObject container, object templateKey) {
			if (container is FrameworkElement fElem && templateKey != null) {
				var retVal = fElem.Resources[templateKey];
				while (retVal == null && container != null) {
					container = VisualTreeHelper.GetParent(container);
					fElem = container as FrameworkElement;
					if (fElem != null) {
						retVal = fElem.Resources[templateKey];
					}
				}
				return retVal;
			}
			else {
				return null;
			}
		}
	}
}