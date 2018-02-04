using Janus.Model.Gui.Dialogs;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Janus.Wpf.Controls.DialogBoxes {
	public class ModalPane : ContentControl {
		private static Dictionary<Window, ModalDescriptor> _ModalDescriptors = new Dictionary<Window, ModalDescriptor>();
		private static Dictionary<DialogModelBase, ModalDescriptor> _ModalsForDialogs = new Dictionary<DialogModelBase, ModalDescriptor>();

		public static void InitializeControl(Window mainWindow, Grid placerGrid) {
			lock (_ModalDescriptors) {
				if (_ModalDescriptors.ContainsKey(mainWindow)) {
					return;
				}
				_ModalDescriptors[mainWindow] = new ModalDescriptor {
					PlacerGrid = placerGrid,
				};
			}
		}

		public static void ShowDialog(Window mainWindow, Control controlToShow) {
			if (controlToShow.DataContext is DialogModelBase dlgModel && _ModalDescriptors.TryGetValue(mainWindow, out var modalDescr)) {
				_ModalsForDialogs[dlgModel] = modalDescr;
				modalDescr.ShowModal(controlToShow);
				_ModalsForDialogs.Remove(dlgModel);
			}
		}

		public static void EndModal(DialogModelBase dialogModelBase) {
			if (_ModalsForDialogs.TryGetValue(dialogModelBase, out var modalDescr)) {
				modalDescr.EndModal(modalDescr.CurrentPane.ShowingControl);
			}
		}

		static ModalPane() {
			DefaultStyleKeyProperty.OverrideMetadata(typeof(ModalPane), new FrameworkPropertyMetadata(typeof(ModalPane)));
		}

		private class ModalDescriptor {
			public Dictionary<FrameworkElement, bool> ControlEnabled { get; } = new Dictionary<FrameworkElement, bool>();

			public Stack<PaneDescriptor> PaneDescriptors { get; } = new Stack<PaneDescriptor>();

			public Grid PlacerGrid { get; set; }

			public PaneDescriptor CurrentPane { get; set; }

			public void ShowModal(Control newModalControl) {
				var paneDescr = new PaneDescriptor { Pane = new ModalPane(), ShowingControl = newModalControl };
				if (CurrentPane != null) {
					CurrentPane.Pane.Visibility = Visibility.Hidden;
					CurrentPane.Pane.IsEnabled = false;
					PaneDescriptors.Push(CurrentPane);
				}
				else {
					foreach (var child in PlacerGrid.Children) {
						if (child is ModalPane mPane && PaneDescriptors.Any(pDescr => pDescr.Pane == mPane)) {
							continue;
						}
						if (child is FrameworkElement fElement) {
							ControlEnabled[fElement] = fElement.IsEnabled;
							fElement.IsEnabled = false;
						}
					}
				}
				CurrentPane = paneDescr;
				PlacerGrid.Children.Add(paneDescr.Pane);
				paneDescr.DispatcherFrame = new DispatcherFrame();
				Dispatcher.PushFrame(paneDescr.DispatcherFrame);
			}

			public void EndModal(Control endModalControl) {
				if (CurrentPane == null) {
					return;
				}
				if (CurrentPane.ShowingControl != endModalControl) {
					return;
				}
				var pane = CurrentPane;
				PlacerGrid.Children.Remove(CurrentPane.Pane);
				if (PaneDescriptors.Count > 0) {
					CurrentPane = PaneDescriptors.Pop();
					CurrentPane.Pane.Visibility = Visibility.Visible;
					CurrentPane.Pane.IsEnabled = true;
				}
				else {
					CurrentPane = null;
					foreach (var ctlPair in ControlEnabled) {
						ctlPair.Key.IsEnabled = ctlPair.Value;
					}
					ControlEnabled.Clear();
				}
				pane.DispatcherFrame.Continue = false;
			}

		}
		private class PaneDescriptor {
			public ModalPane Pane { get; set; }
			public Control ShowingControl {
				get { return Pane?.Content as Control; }
				set {
					if (Pane != null) {
						Pane.Content = value;
					}
				}
			}

			public DispatcherFrame DispatcherFrame { get; internal set; }
		}
	}
}
