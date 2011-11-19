using System.ComponentModel;
using Rsdn.LocUtil.Model.Design;

namespace Rsdn.LocUtil.Model
{
	/// <summary>
	/// Ресурс.
	/// </summary>
	[TypeConverter(typeof (ResourceItemConverter))]
	public class ResourceItem : ItemBase
	{
		private readonly ResourceValueCollection _valueCollection =
			new ResourceValueCollection();

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public ResourceItem(string name, Category parent)
			: base(name, parent)
		{
		}

		/// <summary>
		/// Коллекция значений.
		/// </summary>
		[Browsable(false)]
		public ResourceValueCollection ValueCollection
		{
			get { return _valueCollection; }
		}

		/// <summary>
		/// Удалить элемент.
		/// </summary>
		public void Delete()
		{
			Parent.DeleteItem(this);
		}
	}
}
