using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Стандартная реализация <see cref="ITaskIndicator"/>
	/// </summary>
	public class TaskIndicator : ITaskIndicator
	{
		private readonly Action _refresher;
		private SyncTaskState _currentState = SyncTaskState.WaitForSync;

		public TaskIndicator(
			string taskName,
			Action dataChangeHander)
		{
			TaskName = taskName;
			_refresher = dataChangeHander;
		}

		public SyncTaskState CurrentState
		{
			get { return _currentState; }
		}

		public string TaskName { get; private set; }
		public string StatusText { get; private set; }
		public DateTime? StartTime { get; private set; }
		public DateTime? StopTime { get; private set; }

		#region ITaskIndicator Members
		public void SetTaskState(SyncTaskState state)
		{
			if (state == SyncTaskState.Sync && _currentState != SyncTaskState.Sync)
			{
				StartTime = DateTime.Now;
				StopTime = null;
			}
			else if (_currentState == SyncTaskState.Sync && state != SyncTaskState.Sync)
				StopTime = DateTime.Now;
			_currentState = state;
			_refresher();
		}

		public void SetStatusText(string text)
		{
			StatusText = text;
			_refresher();
		}
		#endregion

		public TimeSpan? GetDuration()
		{
			if (StartTime == null)
				return null;
			return
				StopTime == null
					? DateTime.Now - StartTime
					: StopTime - StartTime;
		}
	}
}