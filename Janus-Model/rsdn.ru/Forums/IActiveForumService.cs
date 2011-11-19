using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	public interface IActiveForumService
	{
		IForum ActiveForum { get; }
		event EventHandler<IActiveForumService> ActiveForumChanged;
	}
}