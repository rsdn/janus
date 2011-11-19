using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Resources;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	/// <summary>
	/// Вспомогательные функции для работы с ресурсами.
	/// </summary>
	public static class ResourceHelper
	{
		/// <summary>
		/// Получить ресурс как изображение.
		/// </summary>
		public static Image LoadImage(
			[NotNull] this Assembly assembly, [NotNull] string pathToResource)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");
			if (pathToResource == null)
				throw new ArgumentNullException("pathToResource");

			return Image.FromStream(assembly.GetRequiredResourceStream(pathToResource));
		}

		/// <summary>
		/// Получить ресурс как Icon.
		/// </summary>
		public static Icon LoadIcon(
			[NotNull] this Assembly assembly, [NotNull] string pathToResource)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");
			if (pathToResource == null)
				throw new ArgumentNullException("pathToResource");

			return new Icon(assembly.GetRequiredResourceStream(pathToResource));
		}

		/// <summary>
		/// Получить ресурс как string.
		/// </summary>
		public static string LoadText(
			[NotNull] this Assembly assembly, [NotNull] string pathToResource)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");
			if (pathToResource == null)
				throw new ArgumentNullException("pathToResource");

			using (var sr = new StreamReader(assembly.GetRequiredResourceStream(pathToResource)))
				return sr.ReadToEnd();
		}

		public static string GetDisplayString([NotNull] this ResourceManager resourceManager, string name)
		{
			if (resourceManager == null)
				throw new ArgumentNullException("resourceManager");

			if (name == null)
				return null;

			return resourceManager.GetString(name) ?? "<no resource \"{0}\">".FormatStr(name);
		}

		public static string GetDisplayString([NotNull] this ResourceSet resourceSet, string name)
		{
			if (resourceSet == null)
				throw new ArgumentNullException("resourceSet");

			if (name == null)
				return null;

			return resourceSet.GetString(name) ?? "<no resource \"{0}\">".FormatStr(name);
		}

		public static Stream GetRequiredResourceStream(
			[NotNull] this Assembly assembly,
			[NotNull] string resourceName)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");
			if (resourceName == null)
				throw new ArgumentNullException("resourceName");

			var stream = assembly.GetManifestResourceStream(resourceName);
			if (stream == null)
				throw new ApplicationException(
					"Resource '{0}' not found in assembly '{1}'.".FormatStr(resourceName, assembly));
			return stream;
		}

		public static Func<string, string> CreateResourceStringGetter(
			[NotNull] Assembly assembly, string baseName)
		{
			return CreateResourceStringGetter(assembly, baseName, null);
		}

		public static Func<string, string> CreateResourceStringGetter(
			[NotNull] Assembly assembly,
			string baseName,
			CultureInfo culture)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			if (baseName != null)
			{
				var rm = new ResourceManager(baseName, assembly);
				var resSets = new List<ResourceSet>();
				var curCulture = culture ?? CultureInfo.CurrentUICulture;
				do
				{
					resSets.Add(rm.GetResourceSet(curCulture, true, true));
					if (curCulture.Parent == null || curCulture == curCulture.Parent)
						break;
					curCulture = curCulture.Parent;
				} while (true);

				return
					name =>
						resSets
							.Select(rs => rs.GetString(name))
							.FirstOrDefault(s => s != null);
			}
			return
				name =>
				{
					throw new ApplicationException(
						"Запрошена ресурсная строка '{0}', но не указан ресурсный файл.".FormatStr(name));
				};
		}
	}
}