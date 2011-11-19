using System.Collections;
using System.Globalization;

namespace Rsdn.LocUtil.Model
{
	/// <summary>
	/// Коллекция значений ресурсов.
	/// </summary>
	public class ResourceValueCollection : DictionaryBase
	{
		/// <summary>
		/// Доступ к элементу коллекции.
		/// </summary>
		public string this[CultureInfo culture]
		{
			get
			{
				return (string)Dictionary[culture];
			}
			set
			{
				Dictionary[culture] = value;
			}
		}

		/// <summary>
		/// Добавить значение.
		/// </summary>
		public void Add(CultureInfo culture, string value)
		{
			Dictionary.Add(culture, value);
		}

		/// <summary>
		/// Получить содержащиеся культуры.
		/// </summary>
		public CultureInfo[] GetCultures()
		{
			CultureInfo[] res = new CultureInfo[Count];
			Dictionary.Keys.CopyTo(res, 0);
			return res;
		}
	}
}
