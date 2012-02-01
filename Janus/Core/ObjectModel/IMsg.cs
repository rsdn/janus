using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	// TODO: Отрефакторить до полного уничтожения.
	/// <summary>
	/// Summary description for IMsg.
	/// </summary>
	public interface IMsg : IForumMessageInfo, ITreeNode
	{
		new IMsg Topic { get; }

		/// <summary>Дочернее сообщение или null если данная реализация
		/// не поддерживает данного свойства.</summary>
		new MsgBase Parent{ get; }
	}
}