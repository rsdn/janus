using Janus.Model.TreeView;
using System.Windows;

namespace Janus.Wpf.Controls {
	/// <summary>
	/// Interaction logic for UserControl1.xaml
	/// </summary>
	public partial class ForumTree {
		public static readonly DependencyProperty SelectedItemProperty =
			DependencyProperty.Register("MyProperty", typeof(ITreeNode), typeof(ForumTree),
				new FrameworkPropertyMetadata(null,
					flags: FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
					propertyChangedCallback: SelectedItem_Changed));


		private bool _IsSelfChanging;

		public ForumTree() {
			InitializeComponent();
		}

		public ITreeNode SelectedItem {
			get { return (ITreeNode)GetValue(SelectedItemProperty); }
			set { SetValue(SelectedItemProperty, value); }
		}

		private static void SelectedItem_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e) {
			(d as ForumTree).DoSelectedItemChanged(e);

		}

		private void DoSelectedItemChanged(DependencyPropertyChangedEventArgs e) {
			if (_IsSelfChanging) {
				return;
			}
			if (SelectedItem != null) {
				SelectedItem.IsSelected = true;
			}
			else if (realTreeControl.SelectedItem is ITreeNode selNode) {
				selNode.IsSelected = false;
			}
		}

		private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e) {
			if (_IsSelfChanging) {
				return;
			}
			_IsSelfChanging = true;
			try {
				SetCurrentValue(SelectedItemProperty, e.NewValue);
			}
			finally {
				_IsSelfChanging = false;
			}
		}
	}
}
