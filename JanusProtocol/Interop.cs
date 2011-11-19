using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Rsdn.Janus.Protocol
{
	[StructLayout(LayoutKind.Explicit, Size = 4, Pack = 4)]
	public struct __MIDL_IAdviseSink_0002
	{}

	[StructLayout(LayoutKind.Explicit, Size = 4, Pack = 4)]
	public struct __MIDL_IAdviseSink_0003
	{}

	[StructLayout(LayoutKind.Explicit, Size = 8, Pack = 8)]
	public struct __MIDL_IWinTypes_0003
	{}

	[StructLayout(LayoutKind.Explicit, Size = 8, Pack = 8)]
	public struct __MIDL_IWinTypes_0004
	{}

	[StructLayout(LayoutKind.Explicit, Size = 8, Pack = 8)]
	public struct __MIDL_IWinTypes_0005
	{}

	[StructLayout(LayoutKind.Explicit, Size = 8, Pack = 8)]
	public struct __MIDL_IWinTypes_0006
	{}

	[StructLayout(LayoutKind.Explicit, Size = 8, Pack = 8)]
	public struct __MIDL_IWinTypes_0007
	{}

	[StructLayout(LayoutKind.Explicit, Size = 8, Pack = 8)]
	public struct __MIDL_IWinTypes_0008
	{}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	[ComConversionLoss]
	public struct _BYTE_BLOB
	{
		public uint clSize;

		[ComConversionLoss]
		public IntPtr abData;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	[ComConversionLoss]
	public struct _FLAGGED_BYTE_BLOB
	{
		public uint fFlags;
		public uint clSize;

		[ComConversionLoss]
		public IntPtr abData;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct _GDI_OBJECT
	{
		public uint ObjectType;
		public __MIDL_IAdviseSink_0002 u;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct _LARGE_INTEGER
	{
		public long QuadPart;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	[ComConversionLoss]
	public struct _remoteMETAFILEPICT
	{
		public int mm;
		public int xExt;
		public int yExt;

		[ComConversionLoss]
		public IntPtr hMF;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	[ComConversionLoss]
	public struct _SECURITY_ATTRIBUTES
	{
		public uint nLength;

		[ComConversionLoss]
		public IntPtr lpSecurityDescriptor;

		public int bInheritHandle;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct _STGMEDIUM_UNION
	{
		public uint tymed;
		public __MIDL_IAdviseSink_0003 u;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	[ComConversionLoss]
	public struct _tagBINDINFO
	{
		public uint cbSize;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string szExtraInfo;

		[ComConversionLoss]
		[ComAliasName("UrlMon.Interop.wireSTGMEDIUM")]
		public IntPtr stgmedData;

		public uint grfBindInfoF;
		public uint dwBindVerb;

		[MarshalAs(UnmanagedType.LPWStr)]
		public string szCustomVerb;

		public uint cbstgmedData;
		public uint dwOptions;
		public uint dwOptionsFlags;
		public uint dwCodePage;
		public _SECURITY_ATTRIBUTES securityAttributes;
		public Guid iid;

		[MarshalAs(UnmanagedType.IUnknown)]
		public object pUnk;

		public uint dwReserved;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	[ComConversionLoss]
	public struct _tagPROTOCOLDATA
	{
		public uint grfFlags;
		public uint dwState;

		[ComConversionLoss]
		public IntPtr pData;

		public uint cbData;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct _ULARGE_INTEGER
	{
		public ulong QuadPart;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	[ComConversionLoss]
	public struct _userBITMAP
	{
		public int bmType;
		public int bmWidth;
		public int bmHeight;
		public int bmWidthBytes;
		public ushort bmPlanes;
		public ushort bmBitsPixel;
		public uint cbSize;

		[ComConversionLoss]
		public IntPtr pBuffer;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct _userHBITMAP
	{
		public int fContext;
		public __MIDL_IWinTypes_0007 u;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct _userHENHMETAFILE
	{
		public int fContext;
		public __MIDL_IWinTypes_0006 u;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct _userHGLOBAL
	{
		public int fContext;
		public __MIDL_IWinTypes_0003 u;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct _userHMETAFILE
	{
		public int fContext;
		public __MIDL_IWinTypes_0004 u;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct _userHMETAFILEPICT
	{
		public int fContext;
		public __MIDL_IWinTypes_0005 u;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 8)]
	public struct _userHPALETTE
	{
		public int fContext;
		public __MIDL_IWinTypes_0008 u;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct _userSTGMEDIUM
	{
		public _STGMEDIUM_UNION __MIDL_0003;

		[MarshalAs(UnmanagedType.IUnknown)]
		public object pUnkForRelease;
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("79EAC9E1-BAF9-11CE-8C82-00AA004BA90B")]
	public interface IInternetBindInfo
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetBindInfo(out uint grfBINDF, [In] [Out] ref _tagBINDINFO pbindinfo);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void GetBindString(
			[In] uint ulStringType,
			[In] [Out] [MarshalAs(UnmanagedType.LPWStr)] ref string ppwzStr,
			[In] uint cEl,
			[In] [Out] ref uint pcElFetched);
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComConversionLoss]
	[Guid("79EAC9E4-BAF9-11CE-8C82-00AA004BA90B")]
	public interface IInternetProtocol : IInternetProtocolRoot
	{
		new void Start(
			[In] [MarshalAs(UnmanagedType.LPWStr)] string szUrl,
			[In] [MarshalAs(UnmanagedType.Interface)] IInternetProtocolSink pOIProtSink,
			[In] [MarshalAs(UnmanagedType.Interface)] IInternetBindInfo pOIBindInfo,
			[In] uint grfPI,
			[In] uint dwReserved);

		new void Continue([In] ref _tagPROTOCOLDATA pProtocolData);

		new void Abort([In] [MarshalAs(UnmanagedType.Error)] int hrReason, [In] uint dwOptions);

		new void Terminate([In] uint dwOptions);

		new void Suspend();

		new void Resume();

		[PreserveSig]
		int Read([In] [Out] IntPtr pv, [In] uint cb, out uint pcbRead);

		void Seek([In] _LARGE_INTEGER dlibMove, [In] uint dwOrigin, out _ULARGE_INTEGER plibNewPosition);

		void LockRequest([In] uint dwOptions);

		void UnlockRequest();
	}

	[ComImport]
	[Guid("79EAC9E3-BAF9-11CE-8C82-00AA004BA90B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IInternetProtocolRoot
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Start(
			[In] [MarshalAs(UnmanagedType.LPWStr)] string szUrl,
			[In] [MarshalAs(UnmanagedType.Interface)] IInternetProtocolSink pOIProtSink,
			[In] [MarshalAs(UnmanagedType.Interface)] IInternetBindInfo pOIBindInfo,
			[In] uint grfPI,
			[In] uint dwReserved);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Continue([In] ref _tagPROTOCOLDATA pProtocolData);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Abort([In] [MarshalAs(UnmanagedType.Error)] int hrReason, [In] uint dwOptions);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Terminate([In] uint dwOptions);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Suspend();

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Resume();
	}

	[ComImport]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("79EAC9E5-BAF9-11CE-8C82-00AA004BA90B")]
	public interface IInternetProtocolSink
	{
		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void Switch([In] ref _tagPROTOCOLDATA pProtocolData);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ReportProgress(
			[In] BINDSTATUS ulStatusCode,
			[In] [MarshalAs(UnmanagedType.LPWStr)] string szStatusText);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ReportData([In] BSCF bscf, [In] uint ulProgress, [In] uint ulProgressMax);

		[MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime)]
		void ReportResult(
			[In] [MarshalAs(UnmanagedType.Error)] int hrResult,
			[In] uint dwError,
			[In] [MarshalAs(UnmanagedType.LPWStr)] string szResult);
	}

	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("79eac9ec-baf9-11ce-8c82-00aa004ba90b")]
	public interface IInternetProtocolInfo
	{
		[PreserveSig]
		UInt32 ParseUrl(
			[MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
			/* [in] */ PARSEACTION ParseAction,
			UInt32 dwParseFlags,
			IntPtr pwzResult,
			UInt32 cchResult,
			out UInt32 pcchResult,
			UInt32 dwReserved);

		[PreserveSig]
		UInt32 CombineUrl(
			[MarshalAs(UnmanagedType.LPWStr)] string pwzBaseUrl,
			[MarshalAs(UnmanagedType.LPWStr)] string pwzRelativeUrl,
			UInt32 dwCombineFlags,
			IntPtr pwzResult,
			UInt32 cchResult,
			out UInt32 pcchResult,
			UInt32 dwReserved);

		[PreserveSig]
		UInt32 CompareUrl(
			[MarshalAs(UnmanagedType.LPWStr)] string pwzUrl1,
			[MarshalAs(UnmanagedType.LPWStr)] string pwzUrl2,
			UInt32 dwCompareFlags);

		[PreserveSig]
		UInt32 QueryInfo(
			[MarshalAs(UnmanagedType.LPWStr)] string pwzUrl,
			QUERYOPTION OueryOption,
			UInt32 dwQueryFlags,
			IntPtr pBuffer,
			UInt32 cbBuffer,
			ref UInt32 pcbBuf,
			UInt32 dwReserved);
	}

	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct tagLOGPALETTE
	{
		public ushort palVersion;
		public ushort palNumEntries;
		public IntPtr palPalEntry;
	}

	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct tagPALETTEENTRY
	{
		public byte peRed;
		public byte peGreen;
		public byte peBlue;
		public byte peFlags;
	}

	public enum PARSEACTION
	{
		PARSE_CANONICALIZE = 1,
		PARSE_FRIENDLY = PARSE_CANONICALIZE + 1,
		PARSE_SECURITY_URL = PARSE_FRIENDLY + 1,
		PARSE_ROOTDOCUMENT = PARSE_SECURITY_URL + 1,
		PARSE_DOCUMENT = PARSE_ROOTDOCUMENT + 1,
		PARSE_ANCHOR = PARSE_DOCUMENT + 1,
		PARSE_ENCODE = PARSE_ANCHOR + 1,
		PARSE_DECODE = PARSE_ENCODE + 1,
		PARSE_PATH_FROM_URL = PARSE_DECODE + 1,
		PARSE_URL_FROM_PATH = PARSE_PATH_FROM_URL + 1,
		PARSE_MIME = PARSE_URL_FROM_PATH + 1,
		PARSE_SERVER = PARSE_MIME + 1,
		PARSE_SCHEMA = PARSE_SERVER + 1,
		PARSE_SITE = PARSE_SCHEMA + 1,
		PARSE_DOMAIN = PARSE_SITE + 1,
		PARSE_LOCATION = PARSE_DOMAIN + 1,
		PARSE_SECURITY_DOMAIN = PARSE_LOCATION + 1,
		PARSE_ESCAPE = PARSE_SECURITY_DOMAIN + 1,
		PARSE_UNESCAPE = PARSE_ESCAPE + 1,
	}

	public enum QUERYOPTION
	{
		QUERY_EXPIRATION_DATE = 1,
		QUERY_TIME_OF_LAST_CHANGE = QUERY_EXPIRATION_DATE + 1,
		QUERY_CONTENT_ENCODING = QUERY_TIME_OF_LAST_CHANGE + 1,
		QUERY_CONTENT_TYPE = QUERY_CONTENT_ENCODING + 1,
		QUERY_REFRESH = QUERY_CONTENT_TYPE + 1,
		QUERY_RECOMBINE = QUERY_REFRESH + 1,
		QUERY_CAN_NAVIGATE = QUERY_RECOMBINE + 1,
		QUERY_USES_NETWORK = QUERY_CAN_NAVIGATE + 1,
		QUERY_IS_CACHED = QUERY_USES_NETWORK + 1,
		QUERY_IS_INSTALLEDENTRY = QUERY_IS_CACHED + 1,
		QUERY_IS_CACHED_OR_MAPPED = QUERY_IS_INSTALLEDENTRY + 1,
		QUERY_USES_CACHE = QUERY_IS_CACHED_OR_MAPPED + 1,
		QUERY_IS_SECURE = QUERY_USES_CACHE + 1,
		QUERY_IS_SAFE = QUERY_IS_SECURE + 1,
	}

	public enum BINDSTATUS
	{
		BINDSTATUS_FINDINGRESOURCE = 1,
		BINDSTATUS_CONNECTING = BINDSTATUS_FINDINGRESOURCE + 1,
		BINDSTATUS_REDIRECTING = BINDSTATUS_CONNECTING + 1,
		BINDSTATUS_BEGINDOWNLOADDATA = BINDSTATUS_REDIRECTING + 1,
		BINDSTATUS_DOWNLOADINGDATA = BINDSTATUS_BEGINDOWNLOADDATA + 1,
		BINDSTATUS_ENDDOWNLOADDATA = BINDSTATUS_DOWNLOADINGDATA + 1,
		BINDSTATUS_BEGINDOWNLOADCOMPONENTS = BINDSTATUS_ENDDOWNLOADDATA + 1,
		BINDSTATUS_INSTALLINGCOMPONENTS = BINDSTATUS_BEGINDOWNLOADCOMPONENTS + 1,
		BINDSTATUS_ENDDOWNLOADCOMPONENTS = BINDSTATUS_INSTALLINGCOMPONENTS + 1,
		BINDSTATUS_USINGCACHEDCOPY = BINDSTATUS_ENDDOWNLOADCOMPONENTS + 1,
		BINDSTATUS_SENDINGREQUEST = BINDSTATUS_USINGCACHEDCOPY + 1,
		BINDSTATUS_CLASSIDAVAILABLE = BINDSTATUS_SENDINGREQUEST + 1,
		BINDSTATUS_MIMETYPEAVAILABLE = BINDSTATUS_CLASSIDAVAILABLE + 1,
		BINDSTATUS_CACHEFILENAMEAVAILABLE = BINDSTATUS_MIMETYPEAVAILABLE + 1,
		BINDSTATUS_BEGINSYNCOPERATION = BINDSTATUS_CACHEFILENAMEAVAILABLE + 1,
		BINDSTATUS_ENDSYNCOPERATION = BINDSTATUS_BEGINSYNCOPERATION + 1,
		BINDSTATUS_BEGINUPLOADDATA = BINDSTATUS_ENDSYNCOPERATION + 1,
		BINDSTATUS_UPLOADINGDATA = BINDSTATUS_BEGINUPLOADDATA + 1,
		BINDSTATUS_ENDUPLOADDATA = BINDSTATUS_UPLOADINGDATA + 1,
		BINDSTATUS_PROTOCOLCLASSID = BINDSTATUS_ENDUPLOADDATA + 1,
		BINDSTATUS_ENCODING = BINDSTATUS_PROTOCOLCLASSID + 1,
		BINDSTATUS_VERIFIEDMIMETYPEAVAILABLE = BINDSTATUS_ENCODING + 1,
		BINDSTATUS_CLASSINSTALLLOCATION = BINDSTATUS_VERIFIEDMIMETYPEAVAILABLE + 1,
		BINDSTATUS_DECODING = BINDSTATUS_CLASSINSTALLLOCATION + 1,
		BINDSTATUS_LOADINGMIMEHANDLER = BINDSTATUS_DECODING + 1,
		BINDSTATUS_CONTENTDISPOSITIONATTACH = BINDSTATUS_LOADINGMIMEHANDLER + 1,
		BINDSTATUS_FILTERREPORTMIMETYPE = BINDSTATUS_CONTENTDISPOSITIONATTACH + 1,
		BINDSTATUS_CLSIDCANINSTANTIATE = BINDSTATUS_FILTERREPORTMIMETYPE + 1,
		BINDSTATUS_IUNKNOWNAVAILABLE = BINDSTATUS_CLSIDCANINSTANTIATE + 1,
		BINDSTATUS_DIRECTBIND = BINDSTATUS_IUNKNOWNAVAILABLE + 1,
		BINDSTATUS_RAWMIMETYPE = BINDSTATUS_DIRECTBIND + 1,
		BINDSTATUS_PROXYDETECTING = BINDSTATUS_RAWMIMETYPE + 1,
		BINDSTATUS_ACCEPTRANGES = BINDSTATUS_PROXYDETECTING + 1,
		BINDSTATUS_COOKIE_SENT = BINDSTATUS_ACCEPTRANGES + 1,
		BINDSTATUS_COMPACT_POLICY_RECEIVED = BINDSTATUS_COOKIE_SENT + 1,
		BINDSTATUS_COOKIE_SUPPRESSED = BINDSTATUS_COMPACT_POLICY_RECEIVED + 1,
		BINDSTATUS_COOKIE_STATE_UNKNOWN = BINDSTATUS_COOKIE_SUPPRESSED + 1,
		BINDSTATUS_COOKIE_STATE_ACCEPT = BINDSTATUS_COOKIE_STATE_UNKNOWN + 1,
		BINDSTATUS_COOKIE_STATE_REJECT = BINDSTATUS_COOKIE_STATE_ACCEPT + 1,
		BINDSTATUS_COOKIE_STATE_PROMPT = BINDSTATUS_COOKIE_STATE_REJECT + 1,
		BINDSTATUS_COOKIE_STATE_LEASH = BINDSTATUS_COOKIE_STATE_PROMPT + 1,
		BINDSTATUS_COOKIE_STATE_DOWNGRADE = BINDSTATUS_COOKIE_STATE_LEASH + 1,
		BINDSTATUS_POLICY_HREF = BINDSTATUS_COOKIE_STATE_DOWNGRADE + 1,
		BINDSTATUS_P3P_HEADER = BINDSTATUS_POLICY_HREF + 1,
		BINDSTATUS_SESSION_COOKIE_RECEIVED = BINDSTATUS_P3P_HEADER + 1,
		BINDSTATUS_PERSISTENT_COOKIE_RECEIVED = BINDSTATUS_SESSION_COOKIE_RECEIVED + 1,
		BINDSTATUS_SESSION_COOKIES_ALLOWED = BINDSTATUS_PERSISTENT_COOKIE_RECEIVED + 1,
		BINDSTATUS_CACHECONTROL = BINDSTATUS_SESSION_COOKIES_ALLOWED + 1,
		BINDSTATUS_CONTENTDISPOSITIONFILENAME = BINDSTATUS_CACHECONTROL + 1,
		BINDSTATUS_MIMETEXTPLAINMISMATCH = BINDSTATUS_CONTENTDISPOSITIONFILENAME + 1,
		BINDSTATUS_PUBLISHERAVAILABLE = BINDSTATUS_MIMETEXTPLAINMISMATCH + 1,
		BINDSTATUS_DISPLAYNAMEAVAILABLE = BINDSTATUS_PUBLISHERAVAILABLE + 1,
	}

	public enum BSCF
	{
		BSCF_FIRSTDATANOTIFICATION = 0x00000001,
		BSCF_INTERMEDIATEDATANOTIFICATION = 0x00000002,
		BSCF_LASTDATANOTIFICATION = 0x00000004,
		BSCF_DATAFULLYAVAILABLE = 0x00000008,
		BSCF_AVAILABLEDATASIZEUNKNOWN = 0x00000010
	}

	public sealed class InteropConstants
	{
		/// <summary>HRESULT for FALSE (not an error).</summary>
		public const int S_FALSE = 0x00000001;

		/// <summary>HRESULT for generic success.</summary>
		public const int S_OK = 0x00000000;

		/// <summary>HRESULT for default action </summary>
		public static UInt32 INET_E_DEFAULT_ACTION = 0x800C0011;
	}
}