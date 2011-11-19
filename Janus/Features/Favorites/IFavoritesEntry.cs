using Rsdn.TreeGrid;

namespace Rsdn.Janus
{
	/// <summary>
	/// Интерфейс элемента Избранного 
	/// </summary>
	public interface IFavoritesEntry : ITreeNode
	{
		int Id { get; set; }

		//родительский узел
		new IFavoritesEntry Parent { get; set; }

		//может ли узел содержать подузлы
		bool IsContainer { get; }

		//Комментарий
		string Comment { get; set; }

		//Вызывается для обновления данных элемента
		void Update();

		//Вызывается для удаления элемента
		void Delete();

		//Вызывается для перемещения элемента в новый раздел
		bool Move(IFavoritesEntry newParent);
	}
}