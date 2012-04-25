namespace Rsdn.Janus
{
	public interface IMenuItemVisitor<in TContext>
	{
		void Visit(IMenu menu, TContext context);
		void Visit(IMenuCommand menuCommand, TContext context);
		void Visit(IMenuSplitButton menuSplitButton, TContext context);
		void Visit(IMenuCheckCommand menuCheckCommand, TContext context);
	}
}