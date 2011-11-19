using System;
using System.Collections.Generic;
using System.Text;

namespace Peak.TreeGrid
{
	public interface ITreeGrid<T>
	{
		ITreeModel<T> TreeModel { get; set; }

		ICollection<Column> Columns { get; }
	}
}
