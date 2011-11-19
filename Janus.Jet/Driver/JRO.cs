using System.Runtime.InteropServices;

using ADODB;

namespace JRO
{
	[ComImport]
	[Guid("DE88C160-FF2C-11D1-BB6F-00C04FAE22DA")]
	public class JetEngineClass
	{}

	[ComImport]
	[Guid("9F63D980-FF25-11D1-BB6F-00C04FAE22DA")]
	public interface IJetEngine
	{
		void CompactDatabase(string SourceConnection, string DestConnection);
		void RefreshCache(Connection Connection);
	}
}