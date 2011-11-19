using System;
using System.Collections.Generic;
using System.Text;

namespace Peak.TreeGrid
{
	public interface IObjectInfo<T>
	{
		T Object { get; }

		CellInfo GetCellInfo(string DataPropertyName, VisualInfo VisualInfo);

		StyleInfo GetStyleInfo(string DataPropertyName, VisualInfo VisualInfo);

		IDualIndexable<string, VisualInfo, CellInfo> Cells { get; }

		IDualIndexable<string, VisualInfo, StyleInfo> Styles { get; }
	}
}
