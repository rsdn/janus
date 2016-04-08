using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Информация об ошибке синхронизации.
	/// </summary>
	public class SyncErrorInfo : IEquatable<SyncErrorInfo>
	{
		public SyncErrorInfo(SyncErrorType type, string taskName, string text)
		{
			if (taskName == null)
				throw new ArgumentNullException(nameof(taskName));

			Type = type;
			TaskName = taskName;
			Text = text;
		}

		public SyncErrorType Type { get; }

		public string TaskName { get; }

		public string Text { get; }

		public static bool operator !=(SyncErrorInfo syncErrorInfo1, SyncErrorInfo syncErrorInfo2)
		{
			return !Equals(syncErrorInfo1, syncErrorInfo2);
		}

		public static bool operator ==(SyncErrorInfo syncErrorInfo1, SyncErrorInfo syncErrorInfo2)
		{
			return Equals(syncErrorInfo1, syncErrorInfo2);
		}

		public bool Equals(SyncErrorInfo syncErrorInfo)
		{
			if (syncErrorInfo == null)
				return false;
			if (!Equals(TaskName, syncErrorInfo.TaskName))
				return false;
			return Equals(Text, syncErrorInfo.Text) && Equals(Type, syncErrorInfo.Type);
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj) || Equals(obj as SyncErrorInfo);
		}

		public override int GetHashCode()
		{
			var result = TaskName.GetHashCode();
			result = 29 * result + (Text?.GetHashCode() ?? 0);
			result = 29 * result + Type.GetHashCode();
			return result;
		}
	}
}