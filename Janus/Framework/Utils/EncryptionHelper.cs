using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace Rsdn.Janus.Framework
{
	/// <summary>
	/// Вспомогательные методы для шифрования/дешифрования строк
	/// </summary>
	public static class EncryptionHelper
	{
		#region Declarations & Private Methods

		//Ключ шифрования
		private const int maxPswLength = 24;
		private const string TD_KEY = "2B47282B154F421AC217546237E262A1DCD6340053BF2107";
		private const string TD_IV = "EDCDA4CAF59493D1";

		private static byte[] GetKey()
		{
			byte[] key = new byte[TD_KEY.Length/2];
			for (int i = 0; i < TD_KEY.Length; i += 2)
			{
				string s = TD_KEY.Substring(i, 2);
				key[i/2] = Byte.Parse(s, NumberStyles.HexNumber);
			}
			byte xorbyte = key[0];
			for (int i = 1; i < key.Length; i++)
			{
				key[i] = (byte) (key[i] ^ xorbyte);
			}
			return key;
		}

		private static byte[] GetIV()
		{
			byte[] iv = new byte[TD_IV.Length/2];
			for (int i = 0; i < TD_IV.Length; i += 2)
			{
				string s = TD_IV.Substring(i, 2);
				iv[i/2] = Byte.Parse(s, NumberStyles.HexNumber);
			}
			return iv;
		}

		#endregion

		#region Public Methods

		public static string EncryptPassword(string pwd)
		{
			if (string.IsNullOrEmpty(pwd))
				return string.Empty;

			byte[] bs = Encoding.GetEncoding(1251).GetBytes(pwd.PadRight(maxPswLength, ' '));
			TripleDESCryptoServiceProvider tdc = new TripleDESCryptoServiceProvider();
			ICryptoTransform ct = tdc.CreateEncryptor(GetKey(), GetIV());
			byte[] encpsw = ct.TransformFinalBlock(bs, 0, bs.Length);
			StringBuilder res = new StringBuilder(maxPswLength);
			for (int i = 0; i < encpsw.Length; i++)
			{
				res.Append(String.Format("{0:X2}", encpsw[i]));
			}
			return res.ToString();
		}

		public static string DecryptPassword(string encpsw)
		{
			if (string.IsNullOrEmpty(encpsw))
				return "";
			byte[] ep = new byte[encpsw.Length/2];
			for (int i = 0; i < encpsw.Length; i += 2)
			{
				string s = encpsw.Substring(i, 2);
				ep[i/2] = Byte.Parse(s, NumberStyles.HexNumber);
			}
			TripleDESCryptoServiceProvider tdc = new TripleDESCryptoServiceProvider();
			ICryptoTransform ct = tdc.CreateDecryptor(GetKey(), GetIV());
			byte[] decpsw = ct.TransformFinalBlock(ep, 0, ep.Length);
			return Encoding.GetEncoding(1251).GetString(decpsw).Trim();
		}

		#endregion
	}
}