using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Summary description for ChangePropertyAttribute.
	/// </summary>
	[Flags]
	public enum ChangeActionType
	{
		NoAction = 0,
		Refresh = 1,
		Restart = 2
	}
}