using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using CodeJam.Services;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for SyncForm.
	/// </summary>
	public partial class SyncForm: Form, ISyncProgressVisualizer, ITaskIndicatorProvider, ISyncErrorInformer
	{
		private const int _normalHeight = 280;
		private const int _errorHeight = 580;
		private const string _skippedTextSubst = "-//-";

		private readonly ImageList _taskStateImages = new ImageList();
		private readonly Dictionary<SyncTaskState, int> _taskStateImageIndexes;
		private readonly Image _pinOpenImage;
		private readonly Image _pinCloseImage;
		private readonly Image _warningIcon;
		private readonly Image _errorIcon;

		private readonly string _emptyCaption;
		private string _progressText;
		private readonly AsyncOperation _asyncOp;
		private readonly List<TaskIndicator> _indicators = new List<TaskIndicator>();
		private readonly BindingList<SyncErrorInfo> _errors = new BindingList<SyncErrorInfo>();
		private bool _errorMode;

		public SyncForm(IServiceProvider provider)
		{
			var styler = provider.GetRequiredService<IStyleImageManager>();
			_pinOpenImage = styler.GetImage(@"Sync\DrawingPinOpen", StyleImageType.ConstSize);
			_pinCloseImage = styler.GetImage(@"Sync\DrawingPinClosed", StyleImageType.ConstSize);
			_warningIcon = styler.GetImage(@"WarningIcon", StyleImageType.ConstSize);
			_errorIcon = styler.GetImage(@"ErrorIcon", StyleImageType.ConstSize);

			_taskStateImages.ColorDepth = ColorDepth.Depth32Bit;

			_taskStateImageIndexes = new Dictionary<SyncTaskState, int>
				{
					{
						SyncTaskState.WaitForSync,
						styler.AppendImage(@"Sync\TaskState_WaitForSync", StyleImageType.ConstSize, _taskStateImages)
					},
					{
						SyncTaskState.Sync,
						styler.AppendImage(@"Sync\TaskState_Sync", StyleImageType.ConstSize, _taskStateImages)
					},
					{
						SyncTaskState.Succeed,
						styler.AppendImage(@"Sync\TaskState_Succeed", StyleImageType.ConstSize, _taskStateImages)
					},
					{
						SyncTaskState.Failed,
						styler.AppendImage(@"Sync\TaskState_Failed", StyleImageType.ConstSize, _taskStateImages)
					},
				};

			_asyncOp = AsyncOperationManager.CreateOperation(null);

			InitializeComponent();
			CustomInitializeComponent();
			_emptyCaption = Text;
		}

		public bool IsCancelled { get; private set; }
		private bool IsPinned { get; set; }

		public string ProgressText
		{
			get { return _progressText; }
			set
			{
				_progressText = value;
				InitProgressText();
			}
		}

		public void TryClose()
		{
			if (!IsPinned && !_errorMode)
				Dispose();
			else
			{
				_refreshDurationTimer.Enabled = false;
				ChangeButtonMode();
				ProgressValue = 0;
				ProgressText = "";
				_progressPie.Visible = false;
				_progressLabel.Visible = false;
				Text = _emptyCaption;
			}
		}

		private void InitProgressText()
		{
			_progressLabel.Text = _progressText;
			Text = $"{_emptyCaption} - {_progressText}";
		}

		private void CustomInitializeComponent()
		{
			_syncTaskList.SmallImageList = _taskStateImages;
			_pinCheck.Checked = Config.Instance.SyncWindowPinned;
			_splitContainer.Panel2Collapsed = true;
			_errorsGrid.AutoGenerateColumns = false;
			_errorsGrid.DataSource = _errors;
			Height = _normalHeight;
		}

		private void _cancelButton_Click(object sender, EventArgs e)
		{
			_cancelButton.Enabled = false;
			IsCancelled = true;
		}

		private void ChangeButtonMode()
		{
			_cancelButton.Enabled = true;
			_cancelButton.Text = SR.CloseButtonText;

			_cancelButton.Click -= _cancelButton_Click;
			_cancelButton.Click += (sender, args) => Close();
		}

		public int ProgressMax
		{
			get { return _progressPie.Maximum; }
			set { _progressPie.Maximum = value; }
		}

		public int ProgressValue
		{
			get { return _progressPie.Value; }
			set
			{
				 _progressPie.Value = value;
				_progressPie.Invalidate();
			}
		}

		private void PinCheckCheckedChanged(object sender, EventArgs e)
		{
			_pinCheck.Image = _pinCheck.Checked ? _pinCloseImage : _pinOpenImage;
			IsPinned = _pinCheck.Checked;
			Config.Instance.SyncWindowPinned = IsPinned;
		}

		private void _copyButton_Click(object sender, EventArgs e)
		{
			if (_errors.Count == 0)
				throw new InvalidOperationException();
			Clipboard.SetText(_errors.GetText());
		}

		#region IProgressVisualizer Members
		public void ReportProgress(int total, int current)
		{
			_asyncOp.Post(
				state =>
					{
						ProgressMax = total;
						ProgressValue = current;
					},
				null);
		}

		public void SetProgressText(string text)
		{
			_asyncOp.Post(state => ProgressText = text, null);
		}

		public void SetCompressionSign(CompressionState state)
		{
			_asyncOp.Post(() => _compressPicture.Visible = state == CompressionState.On);
		}

		#endregion

		#region ITaskIndicatorProvider Implementation
		private void RefreshIndicators()
		{
			_asyncOp.Post(() => _syncTaskList.Invalidate());
		}

		private void RefreshDurationTimer_Tick(object sender, EventArgs e)
		{
			_syncTaskList.Invalidate();
		}

		private void SyncTaskListRetrieveVirtualItem(
			object sender,
			RetrieveVirtualItemEventArgs e)
		{
			var indicator = _indicators[e.ItemIndex];
			var duration = indicator.GetDuration();
			e.Item = new ListViewItem(
				new[]
					{
						indicator.TaskName,
						indicator.StartTime != null
							? indicator.StartTime.Value.TimeOfDay.ToSecondsString()
							: "",
						duration != null
							? duration.ToSecondsString()
							: "",
						indicator.StatusText
					}) { ImageIndex = _taskStateImageIndexes[indicator.CurrentState] };
		}

		public ITaskIndicator AppendTaskIndicator(string taskName)
		{
			var indicator = new TaskIndicator(taskName, RefreshIndicators);
			_indicators.Add(indicator);
			_asyncOp.Post(
				() =>
				{
					_syncTaskList.VirtualListSize = _indicators.Count;
					_refreshDurationTimer.Enabled = true;
				});
			RefreshIndicators();
			return indicator;
		}
		#endregion

		#region ISyncErrorInformer Members
		private void _errorsGrid_CellFormatting(
			object sender,
			DataGridViewCellFormattingEventArgs e)
		{
			if (e.ColumnIndex == 0)
				switch ((SyncErrorType)e.Value)
				{
					case SyncErrorType.Warning:
						e.Value = _warningIcon;
						break;
					case SyncErrorType.CriticalError:
						e.Value = _errorIcon;
						break;
					default:
						e.Value = null;
						break;
				}
		}

		public void AddError(SyncErrorInfo errorInfo)
		{
			_asyncOp.Post(
				() =>
				{
					// Drop text, if previous error was the same
					if (HasSameErrorText(errorInfo))
						errorInfo = new SyncErrorInfo(errorInfo.Type, errorInfo.TaskName, _skippedTextSubst);
					_errors.Add(errorInfo);
					if (!_errorMode)
					{
						_splitContainer.Panel2Collapsed = false;
						Height = _errorHeight;
						_copyButton.Visible = true;
						_errorMode = true;
					}
				});
		}

		private bool HasSameErrorText(SyncErrorInfo errorInfo)
		{
			var i = _errors.Count - 1;
			while (i >= 0)
			{
				var cur = _errors[i];
				if (cur.Text != _skippedTextSubst)
					return cur.Text == errorInfo.Text;
				i--;
			}
			return false;
		}
		#endregion
	}
}