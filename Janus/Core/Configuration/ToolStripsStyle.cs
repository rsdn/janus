using Rsdn.Janus;

namespace Rsdn.Janus
{
public enum ToolStripsStyle
{
	/// <summary>
	/// Системный стиль.
	/// </summary>
	[JanusDisplayName(SR.Config.ToolStripsStyle.System.DisplayNameResourceName)]
	System,
	
	/// <summary>
	/// Office 2003 like стиль.
	/// </summary>
	[JanusDisplayName(SR.Config.ToolStripsStyle.Professional.DisplayNameResourceName)]
	Professional,

	/// <summary>
	/// Tan Color Table стиль.
	/// </summary>
	[JanusDisplayName(SR.Config.ToolStripsStyle.TanColorTable.DisplayNameResourceName)]
	TanColorTable
}
}