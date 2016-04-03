using System;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;

using CodeJam;

namespace Rsdn.Janus
{
	/// <summary>
	/// Вспомогательные методы для шифрования/дешифрования строк
	/// </summary>
	public static class EncryptionHelper
	{
		private static readonly byte[] _aditionalEntropy = { 12, 45, 200, 166, 5 };

		#region Public Methods
		public static string EncryptPassword(this string pwd)
		{
			var bytes =
				pwd == null
					? new byte[0]
					: Encoding.Unicode.GetBytes(pwd);
			var prot = 
				ProtectedData.Protect(bytes, _aditionalEntropy, DataProtectionScope.CurrentUser);
			return Convert.ToBase64String(prot);
		}

		public static string DecryptPassword(this string encpsw)
		{
			try
			{
				return
					encpsw.IsNullOrEmpty()
						? ""
						: Encoding.Unicode.GetString(
							ProtectedData.Unprotect(
								Convert.FromBase64String(encpsw),
								_aditionalEntropy,
								DataProtectionScope.CurrentUser));
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex);
				return "";
			}
		}
		#endregion
	}
}