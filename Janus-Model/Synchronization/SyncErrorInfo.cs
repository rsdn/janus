using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Информация об ошибке синхронизации.
	/// </summary>
	public class SyncErrorInfo : IEquatable<SyncErrorInfo>
	{
		private readonly string _taskName;
		private readonly string _text;
		private readonly SyncErrorType _type;

		public SyncErrorInfo(SyncErrorType type, string taskName, string text)
		{
			if (taskName == null)
				throw new ArgumentNullException("taskName");

			_type = type;
			_taskName = taskName;
			_text = text;
		}

		public SyncErrorType Type
		{
			get { return _type; }
		}

		public string TaskName
		{
			get { return _taskName; }
		}

		public string Text
		{
			get { return _text; }
		}

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
			if (!Equals(_taskName, syncErrorInfo._taskName))
				return false;
			return Equals(_text, syncErrorInfo._text) && Equals(_type, syncErrorInfo._type);
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj) || Equals(obj as SyncErrorInfo);
		}

		public override int GetHashCode()
		{
			var result = _taskName.GetHashCode();
			result = 29 * result + (_text != null ? _text.GetHashCode() : 0);
			result = 29 * result + _type.GetHashCode();
			return result;
		}
	}
}