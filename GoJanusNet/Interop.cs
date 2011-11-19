using System.Runtime.InteropServices;

namespace Rsdn.Janus.GoJanusNet
{
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	[Guid("61D7E14A-F91B-4DB4-A97F-340218484E85")]
	[ComVisible(true)]
	public interface IGoUrl
	{
		[DispId(1)]
		void SendURLToJanus([In] int messageID, [In] [MarshalAs(UnmanagedType.BStr)] string linkText);

		[DispId(2)]
		void ShowMessageInJanus([In] int messageID);
	}
}