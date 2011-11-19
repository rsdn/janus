using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Позволяет управлять сортировкой свойств.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	[Serializable]
	public class SortIndexAttribute : Attribute
	{
		private readonly int _sortIndex;

		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public SortIndexAttribute(int index)
		{
			_sortIndex = index;
		}

		/// <summary>
		/// Приоритет сортировки.
		/// </summary>
		public int SortIndex
		{
			get { return _sortIndex; }
		}
	}
}