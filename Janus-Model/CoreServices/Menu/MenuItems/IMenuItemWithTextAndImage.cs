namespace Rsdn.Janus
{
	public interface IMenuItemWithTextAndImage : IMenuItem
	{
		string Text { get; }
		string Image { get; }
		string Description { get; }
		MenuItemDisplayStyle DisplayStyle { get; }
	}
}