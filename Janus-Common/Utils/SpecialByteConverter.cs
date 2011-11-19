using System;
using System.Globalization;
using System.Text;

namespace Rsdn.Janus.Framework
{
	/// <summary>
	/// Convert from byte[] and to byte[].
	/// </summary>
	public static class SpecialByteConverter
	{
		public static string ToHexString(this byte[] array)
		{
			if (array == null)
				return string.Empty;

			var sb = new StringBuilder(array.Length * 2);
			foreach (byte b in array)
			{
				sb.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", b);
			}

			return sb.ToString();
		}

		public static byte[] FromHexString(this string str)
		{
			if (str.Length != 16)
				return new byte[8];

			var ba = new byte[str.Length / 2];
			for (int i = 0; i < str.Length; i += 2)
			{
				string s = str.Substring(i, 2);
				ba[i / 2] = Byte.Parse(s, NumberStyles.HexNumber);
			}

			return ba;
		}
	}
}