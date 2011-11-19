using System;
using System.ComponentModel;

namespace Rsdn.Janus
{
	/// <summary>
	/// Базовый класс для вложенных конфигов.
	/// </summary>
	[TypeConverter(typeof (ExpandableObjectConverter))]
	public class SubConfigBase
	{
		/// <summary>
		/// Преобразовать в строку.
		/// </summary>
		public override string ToString()
		{
			return String.Empty;
		}
	}
}
