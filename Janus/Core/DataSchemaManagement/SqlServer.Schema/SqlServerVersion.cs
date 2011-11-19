using System;
using System.Globalization;

namespace Rsdn.Janus
{
	internal sealed class SqlServerVersion
	{
		public int ver1;
		public int ver2;
		public int ver3;

		public static bool Parse(string str, out SqlServerVersion version)
		{
			version = new SqlServerVersion();

			if (String.IsNullOrEmpty(str))
				return false;

			string[] sversion = str.Split('.');

			if (sversion.Length != 3)
				return false;

			version.ver1 = Convert.ToInt32(sversion[0], CultureInfo.InvariantCulture);
			version.ver2 = Convert.ToInt32(sversion[1], CultureInfo.InvariantCulture);
			version.ver3 = Convert.ToInt32(sversion[2], CultureInfo.InvariantCulture);

			return true;
		}
	}

}
