using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static System.Math;
namespace Janus.Wpf.Controls.DialogBoxes {
	/// <summary>
	/// Interaction logic for ConfirmationDialog.xaml
	/// </summary>
	public partial class ConfirmationDialog : UserControl {
		private bool _Moving = false;
		private bool _Downed = false;
		private Point _DownPos;
		private Thickness _StartMargin;

		public ConfirmationDialog() {
			InitializeComponent();
		}

		private void Caption_MouseDown(object sender, MouseButtonEventArgs e) {
			if (e.ChangedButton == MouseButton.Left) {
				_DownPos = e.GetPosition(this);
				_StartMargin = Margin;
				_Downed = true;
				_Moving = false;
			}
		}

		private void Caption_MouseUp(object sender, MouseButtonEventArgs e) {
			if (e.ChangedButton == MouseButton.Left) {
				_Downed = false;
				_Moving = false;
			}
		}

		private void Caption__MouseMove(object sender, MouseEventArgs e) {
			if (_Downed) {
				var currPos = e.GetPosition(this);
				var move = currPos - _DownPos;
				if (!_Moving) {
					_Moving = Abs(move.X) >= 5 || Abs(move.Y) >= 5;
				}
				if (_Moving) {
					Margin = new Thickness(_StartMargin.Left + move.X, _StartMargin.Top + move.Y,
						_StartMargin.Right + move.X, _StartMargin.Bottom + move.Y);
				}
			}
		}
	}
}
