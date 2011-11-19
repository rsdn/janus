namespace Rsdn.Janus
{
	/// <summary>
	/// Элемент меню.
	/// </summary>
	public interface IMenuItem
	{
		/// <summary>
		/// Индекс, по которому будет осуществляться сортировка.
		/// </summary>
		int OrderIndex { get; }

		void AcceptVisitor<TContext>(IMenuItemVisitor<TContext> visitor, TContext context);
	}
}