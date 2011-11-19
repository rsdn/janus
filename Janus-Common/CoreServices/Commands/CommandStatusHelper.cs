using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public static class CommandStatusHelper
	{
		public static CommandStatus UnavailableIfNot(
			this CommandStatus baseStatus,
			[NotNull] Func<bool> condition)
		{
			if (condition == null)
				throw new ArgumentNullException("condition");

			return 
				baseStatus == CommandStatus.Normal || baseStatus == CommandStatus.Disabled
					? (condition()
						? baseStatus
						: CommandStatus.Unavailable)
					: baseStatus;
		}

		public static CommandStatus DisabledIfNot(
			this CommandStatus baseStatus,
			[NotNull] Func<bool> condition)
		{
			if (condition == null)
				throw new ArgumentNullException("condition");

			return
				baseStatus == CommandStatus.Normal
					? (condition()
						? baseStatus
						: CommandStatus.Disabled)
					: baseStatus;
		}

		public static CommandStatus Or(this CommandStatus status1, CommandStatus status2)
		{
			if (status1 == CommandStatus.Normal || status2 == CommandStatus.Normal)
				return CommandStatus.Normal;
			if (status1 == CommandStatus.Disabled || status2 == CommandStatus.Disabled)
				return CommandStatus.Disabled;
			return CommandStatus.Unavailable;	
		}
	}
}