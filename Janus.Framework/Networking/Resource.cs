using System.IO;
using System.Text;

namespace Rsdn.Janus.Framework.Networking
{
	public class Resource
	{
		private readonly byte[] _bytes;
		private readonly int _mimeEnd;
		private readonly string _mimeType;

		public Resource(string mimeType, byte[] bytes)
		{
			_mimeType = mimeType;
			_bytes = Pack(mimeType, bytes, ref _mimeEnd);
		}

		public Resource(string mimeType, string text, Encoding encoding)
			: this(mimeType, EncodeText(text, encoding))
		{}

		public Resource(string mimeType, string text)
			: this(mimeType, EncodeText(text, Encoding.UTF8))
		{}

		private Resource(string mimeType, byte[] bytes, int mimeEnd)
		{
			_mimeType = mimeType;
			_bytes = bytes;
			_mimeEnd = mimeEnd;
		}

		public string MimeType
		{
			get { return _mimeType; }
		}

		public int MimeEnd
		{
			get { return _mimeEnd; }
		}

		public static byte[] EncodeText(string text, Encoding encoding)
		{
			using (var ms = new MemoryStream())
			{
				using (var sw = new StreamWriter(ms, encoding))
					sw.Write(text);
				return ms.ToArray();
			}
		}

		private static byte[] Pack(string mimeType, byte[] bytes, ref int mimeEnd)
		{
			using (var ms = new MemoryStream())
			{
				using (var sw = new BinaryWriter(ms))
				{
					sw.Write(mimeType);
					sw.Flush();
					mimeEnd = (int)ms.Position;
					sw.Write(bytes);
				}
				return ms.ToArray();
			}
		}

		public static Resource Unpack(byte[] bytes)
		{
			using (var ms = new MemoryStream(bytes, false))
			using (var sr = new BinaryReader(ms))
			{
				var mimeType = sr.ReadString();
				var mimeEnd = (int)ms.Position;
				return new Resource(mimeType, bytes, mimeEnd);
			}
		}


		public ResourceReader GetReader()
		{
			return new ResourceReader(_bytes, _mimeEnd);
		}

		public byte[] GetPackedBytes()
		{
			return _bytes;
		}
	}
}