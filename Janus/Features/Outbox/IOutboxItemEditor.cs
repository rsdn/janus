namespace Rsdn.Janus
{
	/// <summary>
	/// Интерфейс редактора элемента исходящих.
	/// </summary>
	public interface IOutboxItemEditor
	{
		bool AllowEdit(object item);
		
		bool AllowDelete(object item);

		void Edit(object item);

		void Delete(object item);
	}
}