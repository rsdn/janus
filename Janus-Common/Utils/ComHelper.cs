using System;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;

using JetBrains.Annotations;

using Microsoft.Win32;

namespace Rsdn.Janus
{
	public static class ComHelper
	{
		public static bool IsTypeRegisteredInCom([NotNull] Type type)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));

			var strClsId = "{" + Marshal.GenerateGuidForType(type).ToString().ToUpper(CultureInfo.InvariantCulture) + "}";

			using (var rootKey = Registry.ClassesRoot.OpenSubKey(@"CLSID", writable: false))
			{
				if (rootKey == null)
					return false;

				using (var clsIdKey = rootKey.OpenSubKey(strClsId, writable: false))
				{
					if (clsIdKey == null)
						return false;

					using (var inProcServerKey = clsIdKey.OpenSubKey(@"InprocServer32", writable: false))
					{
						if (inProcServerKey == null)
							return false;

						var assembly = type.Assembly;

						if (!IsComRegisteredInNode(type, assembly, inProcServerKey))
							return false;

						var versionStr = assembly.GetName().Version.ToString(4);
						using (var versionSubKey = inProcServerKey.OpenSubKey(versionStr, writable: false))
						{
							if (versionSubKey == null)
								return false;

							return IsComRegisteredInNode(type, assembly, versionSubKey);
						}
					}
				}
			}
		}

		private static bool IsComRegisteredInNode(
			[NotNull] Type type,
			[NotNull] Assembly assembly,
			[NotNull] RegistryKey inProcServerKey)
		{
			var comparer = StringComparer.Ordinal;

			var regFullName = inProcServerKey.GetValue(@"Class") as string;
			if (comparer.Compare(regFullName, type.FullName) != 0)
				return false;

			var regAsmName = inProcServerKey.GetValue(@"Assembly") as string;
			if (comparer.Compare(regAsmName, assembly.FullName) != 0)
				return false;

			var regRuntimeVersion = inProcServerKey.GetValue(@"RuntimeVersion") as string;
			if (comparer.Compare(regRuntimeVersion, assembly.ImageRuntimeVersion) != 0)
				return false;

			var regCodeBase = inProcServerKey.GetValue(@"CodeBase") as string;
			if (comparer.Compare(regCodeBase, assembly.CodeBase) != 0)
				return false;

			return true;
		}

		public static void RegisterAssembly([NotNull] Type type)
		{
			if (type == null)
				throw new ArgumentNullException(nameof(type));

			var rs = new RegistrationServices();
			rs.RegisterAssembly(type.Assembly, AssemblyRegistrationFlags.SetCodeBase);
		}
	}
}