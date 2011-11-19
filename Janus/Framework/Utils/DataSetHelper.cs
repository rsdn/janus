using System.Data;

namespace Rsdn.Janus
{
	public static class DataSetHelper
	{
		public static bool ContainsData(this DataSet dataSet)
		{
			foreach (DataTable table in dataSet.Tables)
				if (table.Rows.Count > 0)
					return true;
			return false;
		}
	}
}