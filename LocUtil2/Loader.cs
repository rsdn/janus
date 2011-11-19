using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Resources;

using Rsdn.LocUtil.Model;
using System.Collections.Generic;

namespace Rsdn.LocUtil
{
	/// <summary>
	/// Загрузчик дерева.
	/// </summary>
	public static class Loader
	{
		private const string _resxFileExtension = "*.resx";

		private static readonly Dictionary<string, CultureInfo> _cultures = new Dictionary<string, CultureInfo>();

		/// <summary>
		/// Загрузить ресурсы.
		/// </summary>
		public static RootCategory Load(string file, out CultureInfo[] loadedCultures,
			out DateTime lastModifiedTime)
		{
			// Fill cultures hash
			foreach (CultureInfo ci in CultureInfo.GetCultures(CultureTypes.AllCultures))
				_cultures[ci.Name] = ci;

			string fnwe = Path.GetFileNameWithoutExtension(file);
			string locExt = Path.GetExtension(fnwe);
			string sn = !string.IsNullOrEmpty(locExt) && _cultures.ContainsKey(locExt.Substring(1))
				? fnwe.Substring(0, fnwe.Length - locExt.Length)
				: fnwe;
			RootCategory root = new RootCategory(sn);
			List<CultureInfo> loaded = new List<CultureInfo>();
			lastModifiedTime = File.GetLastWriteTime(file);
			foreach (string fn in Directory.GetFiles(Path.GetDirectoryName(file),
				_resxFileExtension))
				if (Path.GetFileName(fn).IndexOf(sn) == 0)
				{
					loaded.Add(AppendResX(root, fn));
					DateTime lmt = File.GetLastWriteTime(fn);
					if (lmt > lastModifiedTime)
						lastModifiedTime = lmt;
				}
			loadedCultures = loaded.ToArray();
			return root;
		}

		private static CultureInfo AppendResX(RootCategory root, string file)
		{
			CultureInfo locale = GetLocale(file);
			ResXResourceReader rxrr = new ResXResourceReader(file);
			foreach (DictionaryEntry de in rxrr)
			{
				ResourceItem item = root.GetItem((string)de.Key);
				item.ValueCollection[locale] = de.Value.ToString();
			}
			return locale;
		}

		private static CultureInfo GetLocale(string fileName)
		{
			fileName = Path.GetFileName(fileName);
			string[] parts = fileName.Split('.');
			if (parts.Length < 3)
				return CultureInfo.InvariantCulture;

			string cn = parts[parts.Length - 2];

			CultureInfo ci;
			if (_cultures.TryGetValue(cn, out ci))
				return ci;

			return CultureInfo.InvariantCulture;
		}
	}
}
