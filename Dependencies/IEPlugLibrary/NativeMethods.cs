using System;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;

namespace Mihailik.InternetExplorer
{
	public sealed class NativeMethods
    {
        private NativeMethods() { throw new NotImplementedException(); }

        internal static readonly Guid IID_IUnknown=new Guid("00020400-0000-0000-C000-000000000046");
        internal static readonly Guid IID_IClassFactory=new Guid("00000001-0000-0000-C000-000000000046");

        [ComImport]
        [Guid("00000001-0000-0000-C000-000000000046")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        public interface IClassFactory
        {
            IntPtr CreateInstance(
                [MarshalAs(UnmanagedType.IUnknown)]object pOuterUnk,
                ref Guid iid);

            void LockServer(bool Lock);
        }


        [DllImport("urlmon.dll")] 
        internal static extern void CoInternetGetSession(
            IntPtr dwSessionMode,
            out IInternetSession ppIInternetSession,
            IntPtr dwReserved );

        [DllImport("mscoree.dll")]
        internal static extern int DllGetClassObject(
            ref Guid rclsid,
            ref Guid riid,
            out IntPtr ppv );

        [DllImport("ole32.dll")]
        public static extern int CreateStreamOnHGlobal(
            IntPtr hGlobal,
            int fDeleteOnRelease,
            out IStream ppstm );

        [DllImport("wininet.dll")]
        public static extern bool InternetGetCookie(
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpszUrl,
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpszCookieName,
            [In]IntPtr lpszCookieData,
            ref int lpdwSize );

        [DllImport("wininet.dll")]
        public static extern bool InternetSetCookie(
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpszUrl,
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpszCookieName,
            [MarshalAs(UnmanagedType.LPWStr)]
            string lpszCookieData );





        // structs

        /// <summary>Contains state information about the protocol that is transparent to the transaction handler. </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct PROTOCOLDATA
        {
            /// <summary>Unsigned long integer value that contains the flags.</summary>
            public int      grfFlags;
            /// <summary>Unsigned long integer value that contains the state of the protocol handler.</summary>
            public int      dwState;
            /// <summary>Address of the data buffer.</summary>
            public IntPtr   pData;
            /// <summary>Unsigned long integer value that contains the size of the data buffer.</summary>
            public int      cbData;
        }

        /// <summary>
        /// The STGMEDIUM structure is a generalized global memory handle used for data transfer operations by the IAdviseSink, IDataObject, and IOleCache interfaces.
        /// </summary>
        [StructLayout(LayoutKind.Explicit, Size=12)]
        public struct STGMEDIUM
        {
            [FieldOffset(0)]
            public TYMED tymed;

            // union
            //[FieldOffset(4)]    public IntPtr hMetaFilePict;
            //[FieldOffset(4)]    public IntPtr hHEnhMetaFile;
            //[FieldOffset(4)]    public IntPtr hGdiHandle;
            [FieldOffset(4)]    public IntPtr hGlobal;
            //[FieldOffset(4)] [MarshalAs(UnmanagedType.LPWStr)] public string lpszFileName;
            //[FieldOffset(4)]    public IntPtr pstm;
            //[FieldOffset(4)]    public IntPtr pstg;
        }

        /// <summary>
        /// Contains additional information on the requested binding operation. The meaning of this structure is specific to the type of asynchronous moniker.
        /// </summary>
        [StructLayout(LayoutKind.Sequential)]
        public struct BINDINFO
        {
            /// <summary> Size of the structure, in bytes. </summary>
            public int cbSize;                      // cbSize: ULONG;

            /// <summary> Behavior of this field is moniker-specific. For URL monikers, this string is appended to the URL when the bind operation is started. Like other OLE strings, this value is a Unicode string that the client should allocate using CoTaskMemAlloc. The URL moniker frees the memory later. </summary>
            //[MarshalAs(UnmanagedType.LPWStr)]
            public IntPtr szExtraInfo;              // szExtraInfo: LPWSTR;

            /// <summary> Data to be used in a PUT or POST operation specified by the dwBindVerb member. </summary>
            public NativeMethods.STGMEDIUM stgmedData; // stgmedData: TStgMedium;

            /// <summary> Flag from the BINDINFOF enumeration that determines the use of URL encoding during the binding operation. This member is specific to URL monikers. </summary>
            public NativeMethods.BINDINFOF grfBindInfoF; // 

            /// <summary> Value from the BINDVERB enumeration specifying an action to be performed during the bind operation. </summary>
            public NativeMethods.BINDVERB dwBindVerb;

            /// <summary> BSTR specifying a protocol-specific custom action to be performed during the bind operation (only if dwBindVerb is set to BINDVERB_CUSTOM). </summary>
            //[MarshalAs(UnmanagedType.LPWStr)]
            public IntPtr szCustomVerb;

            /// <summary> Size of the data provided in the stgmedData member. </summary>
            public int cbstgmedData;

            /// <summary> Reserved. Must be set to 0. </summary>
            public int dwOptions;

            /// <summary> Reserved. Must be set to 0. </summary>
            public int dwOptionsFlags;

            /// <summary> Unsigned long integer value that contains the code page used to perform the conversion. </summary>
            public int dwCodePage;

            /// <summary> SECURITY_ATTRIBUTES structure that contains the descriptor for the object being bound to and indicates whether the handle retrieved by specifying this structure is inheritable. </summary>
            public NativeMethods.SECURITY_ATTRIBUTES securityAttributes;

            /// <summary> Interface identifier of the IUnknown interface referred to by pUnk. </summary>
            public Guid iid;

            /// <summary> Pointer to the IUnknown interface. </summary>
            public IntPtr punk;

            /// <summary> Reserved. Must be set to 0. </summary>
            public int dwReserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct SECURITY_ATTRIBUTES
        {
            public int nLength;
            public IntPtr lpSecurityDescriptor;
            public int bInheritHandle;
        }







        // enums

        /// <summary>
        /// The TYMED enumeration values indicate the type of storage medium being used in a data transfer. They are used in the STGMEDIUM or FORMATETC structures.
        /// </summary>
        [Flags]
        public enum TYMED : int
        { 
            /// <summary> No data is being passed. </summary>
            TYMED_NULL                      = 0,
            /// <summary> The storage medium is a global memory handle (HGLOBAL). Allocate the global handle with the GMEM_SHARE flag. If the STGMEDIUM punkForRelease member is NULL, the destination process should use GlobalFree to release the memory. </summary>
            TYMED_HGLOBAL                   = 1,
            /// <summary> The storage medium is a disk file identified by a path. If the STGMEDIUM punkForRelease member is NULL, the destination process should use OpenFile to delete the file. </summary>
            TYMED_FILE                      = 2,
            /// <summary> The storage medium is a stream object identified by an IStream pointer. Use ISequentialStream::Read to read the data. If the STGMEDIUM punkForRelease member is not NULL, the destination process should use IStream::Release to release the stream component. </summary>
            TYMED_ISTREAM                   = 4,
            /// <summary> The storage medium is a storage component identified by an IStorage pointer. The data is in the streams and storages contained by this IStorage instance. If the STGMEDIUM punkForRelease member is not NULL, the destination process should use IStorage::Release to release the storage component. </summary>
            TYMED_ISTORAGE                  = 8,
            /// <summary> The storage medium is a GDI component (HBITMAP). If the STGMEDIUM punkForRelease member is NULL, the destination process should use DeleteObject to delete the bitmap. </summary>
            TYMED_GDI                       = 16,
            /// <summary> The storage medium is a metafile (HMETAFILE). Use the Windows or WIN32 functions to access the metafile's data. If the STGMEDIUM punkForRelease member is NULL, the destination process should use DeleteMetaFile to delete the bitmap. </summary>
            TYMED_MFPICT                    = 32,
            /// <summary> The storage medium is an enhanced metafile. If the STGMEDIUM punkForRelease member is NULL, the destination process should use DeleteEnhMetaFile to delete the bitmap. </summary>
            TYMED_ENHMF                     = 64
        }  


        /// <summary>Contains the flags that control the asynchronous pluggable protocol handler.</summary>
        [Flags]
        public enum PI_FLAGS : int
        {
            /// <summary>Asynchronous pluggable protocol should parse the URL and return S_OK if the URL is syntactically correct; otherwise S_FALSE.</summary>
            PI_PARSE_URL                    = 0x1,
            /// <summary>Asynchronous pluggable protocol handler should be running in filter mode and data will come in through the IInternetProtocolSink interface it exposes. The QueryInterface method will be called on the protocol handler for its IInternetProtocolSink interface.</summary>
            PI_FILTER_MODE                  = 0x2,
            /// <summary>Asynchronous pluggable protocol handler should do as little work as possible on the apartment (or user interface) thread and continue on a worker thread as soon as possible.</summary>
            PI_FORCE_ASYNC                  = 0x4,
            /// <summary>Asynchronous pluggable protocol handler should use worker threads and not use the apartment thread.</summary>
            PI_USE_WORKERTHREAD             = 0x8,
            /// <summary>Asynchronous pluggable protocol handler should verify and report the MIME type.</summary>
            PI_MIMEVERIFICATION             = 0x10,
            /// <summary>Asynchronous pluggable protocol handler should find the CLSID associated with the MIME type.</summary>
            PI_CLSIDLOOKUP                  = 0x20,
            /// <summary>Asynchronous pluggable protocol handler should report its progress.</summary>
            PI_DATAPROGRESS                 = 0x40,
            /// <summary>Asynchronous pluggable protocol handler should work synchronously.</summary>
            PI_SYNCHRONOUS                  = 0x80,
            /// <summary>Asynchronous pluggable protocol handler should use the apartment (or user interface) thread only.</summary>
            PI_APARTMENTTHREADED            = 0x100,
            /// <summary>Asynchronous pluggable protocol handler should install the class if the class is not found.</summary>
            PI_CLASSINSTALL                 = 0x200,
            /// <summary>Asynchronous pluggable protocol handler should pass address of the IBindCtx interface to the pUnk member of the PROTOCOLFILTERDATA structure.</summary>
            PI_PASSONBINDCTX                = 0x2000,
            /// <summary>Asynchronous pluggable protocol handler should disable MIME filters.</summary>
            PI_NOMIMEHANDLER                = 0x8000,
            /// <summary>Asynchronous pluggable protocol handler should load the application directly.</summary>
            PI_LOADAPPDIRECT                = 0x4000,
            /// <summary>Asynchronous pluggable protocol handler should switch to the apartment thread, even if it does not need to.</summary>
            PD_FORCE_SWITCH                 = 0x10000,
            /// <summary>Not currently supported. This type is obsolete. Do not use.</summary>
            PI_PREFERDEFAULTHANDLER         = 0x20000
        }

        /// <summary>Values from the BSCF enumeration are passed to the client in IBindStatusCallback::OnDataAvailable to indicate the type of data that is available.</summary>
        [Flags]
        public enum BSCF : int
        {
            /// <summary>Identify the first call to IBindStatusCallback::OnDataAvailable for a given bind operation.</summary>
            BSCF_FIRSTDATANOTIFICATION      = 0x1,
            /// <summary>Identify an intermediate call to IBindStatusCallback::OnDataAvailable for a bind operation.</summary>
            BSCF_INTERMEDIATEDATANOTIFICATION= 0x2,
            /// <summary>Identify the last call to IBindStatusCallback::OnDataAvailable for a bind operation.</summary>
            BSCF_LASTDATANOTIFICATION       = 0x4,
            /// <summary>All of the requested data is available.</summary>
            BSCF_DATAFULLYAVAILABLE         = 0x8,
            /// <summary>Size of the data available is unknown.</summary>
            BSCF_AVAILABLEDATASIZEUNKNOWN   = 0x10
        }

        /// <summary>Contains values that specify an action, such as an HTTP request, to be performed during the binding operation.</summary>
        /// <remarks>Values from the BINDVERB enumeration are passed to the moniker as part of the BINDINFO structure. The moniker calls the IBindStatusCallback::OnProgress method on the client to obtain additional information about the bind operation in the BINDINFO structure.</remarks>
        public enum BINDVERB : int
        {
            /// <summary>Perform an HTTP GET operation, the default operation. The stgmedData member of the BINDINFO structure should be set to TYMED_NULL for the GET operation.</summary>
            BINDVERB_GET                    = 0,
            /// <summary>Perform an HTTP POST operation. The data to be posted should be specified in the stgmedData member of the BINDINFO structure.</summary>
            BINDVERB_POST                   = 0x1,
            /// <summary>Perform an HTTP PUT operation. The data to put should be specified in the stgmedData member of the BINDINFO structure.</summary>
            BINDVERB_PUT                    = 0x2,
            /// <summary>Perform a custom operation that is protocol-specific. See the szCustomVerb member of the BINDINFO structure. The data to be used in the custom operation should be specified in the stgmedData structure.</summary>
            BINDVERB_CUSTOM                 = 0x3
        }

        /// <summary>Contains values that determine the use of URL encoding during the binding operation.</summary>
        /// <remarks>Values from the BINDINFOF enumeration are passed to the moniker as part of the BINDINFO structure. The moniker calls the IBindStatusCallback::GetBindInfo method on the client to obtain additional information about the bind operation in the BINDINFO structure.</remarks>
        public enum BINDINFOF : int
        {
            /// <summary>Use URL encoding to pass in the data provided in the stgmedData member of the BINDINFO structure for PUT and POST operations.</summary>
            BINDINFOF_URLENCODESTGMEDDATA   = 0x1,
            /// <summary>Use URL encoding to pass in the data provided in the szExtraInfo member of the BINDINFO structure.</summary>
            BINDINFOF_URLENCODEDEXTRAINFO   = 0x2
        }

        /// <summary>Contains the values that determine how a resource should be bound to a moniker.</summary>
        /// <remarks>These values are passed to the Urlmon.dll from the client application's implementation of the IBindStatusCallback::GetBindInfo method.</remarks>
        [Flags]
        public enum BINDF : int
        {
            /// <summary>Value that indicates that the moniker should return immediately from IMoniker::BindToStorage or IMoniker::BindToObject. The actual result of the bind to an object or the bind to storage arrives asynchronously. The client is notified through calls to its IBindStatusCallback::OnDataAvailable or IBindStatusCallback::OnObjectAvailable method. If the client does not specify this flag, the bind operation will be synchronous, and the client will not receive any data from the bind operation until the IMoniker::BindToStorage or IMoniker::BindToObject call returns.</summary>
            BINDF_ASYNCHRONOUS              = 0x1,
            /// <summary>Value that indicates the client application calling the IMoniker::BindToStorage method prefers that the storage and stream objects returned in IBindStatusCallback::OnDataAvailable return E_PENDING when they reference data not yet available through their read methods, rather than blocking until the data becomes available. This flag applies only to BINDF_ASYNCHRONOUS operations. Note that asynchronous stream objects return E_PENDING while data is still downloading and return S_FALSE for the end of the file.</summary>
            BINDF_ASYNCSTORAGE              = 0x2,
            /// <summary>Value that indicates that progressive rendering should not be allowed.</summary>
            BINDF_NOPROGRESSIVERENDERING    = 0x4,
            /// <summary>Value that indicates that the moniker should be bound to the cached version of the resource.</summary>
            BINDF_OFFLINEOPERATION          = 0x8,
            /// <summary>Value that indicates the bind operation should retrieve the newest version of the data/object possible. For URL monikers, this flag maps to the WinInet API flag, INTERNET_FLAG_RELOAD, which forces a download of the requested resource.</summary>
            BINDF_GETNEWESTVERSION          = 0x10,
            /// <summary>Value that indicates the bind operation should not store retrieved data in the disk cache. BINDF_PULLDATA must also be specified to turn off the cache file generation when using the IMoniker::BindToStorage method.</summary>
            BINDF_NOWRITECACHE              = 0x20,
            /// <summary>Value that indicates the downloaded resource must be saved in the cache or a local file.</summary>
            BINDF_NEEDFILE                  = 0x40,
            /// <summary>Value that indicates the asynchronous moniker allows the client of IMoniker::BindToStorage to drive the bind operation by pulling the data, rather than having the moniker drive the operation by pushing the data to the client. When this flag is specified, new data is only read/downloaded after the client finishes downloading all data that is currently available. This means data is only downloaded for the client after the client does an IStream::Read operation that blocks or returns E_PENDING. When the client specifies this flag, it must be sure to read all the data it can, even data that is not necessarily available yet. When this flag is not specified, the moniker continues downloading data and calls the client with IBindStatusCallback::OnDataAvailable whenever new data is available. This flag applies only to BINDF_ASYNCHRONOUS bind operations.</summary>
            BINDF_PULLDATA                  = 0x80,
            /// <summary>Value that indicates that security problems related to bad certificates and redirects between HTTP and Secure Hypertext Transfer Protocol (HTTPS) servers should be ignored. For URL monikers, this flag corresponds to the WinInet API flags API Flags, API Flags, API Flags, and API Flags.</summary>
            BINDF_IGNORESECURITYPROBLEM     = 0x100,
            /// <summary>Value that indicates the resource should be resynchronized. For URL monikers, this flag maps to the WinInet API flag, INTERNET_FLAG_RESYNCHRONIZE, which reloads an HTTP resource if the resource has been modified since the last time it was downloaded. All File Transfer Protocol (FTP) and Gopher resources are reloaded.</summary>
            BINDF_RESYNCHRONIZE             = 0x200,
            /// <summary>Value that indicates hyperlinks are allowed.</summary>
            BINDF_HYPERLINK                 = 0x400,
            /// <summary>Value that indicates that the bind operation should not display any user interfaces.</summary>
            BINDF_NO_UI                     = 0x800,
            /// <summary>Value that indicates the bind operation should be completed silently. No user interface or user notification should occur.</summary>
            BINDF_SILENTOPERATION           = 0x1000,
            /// <summary>Value that indicates that the resource should not be stored in the Internet cache.</summary>
            BINDF_PRAGMA_NO_CACHE           = 0x2000,
            /// <summary>Value that indicates that the class object should be retrieved. Normally the class instance is retrieved.</summary>
            BINDF_GETCLASSOBJECT            = 0x4000,
            /// <summary>Reserved.</summary>
            BINDF_RESERVED_1                = 0x8000,
            /// <summary>Reserved.</summary>
            BINDF_FREE_THREADED             = 0x10000,
            /// <summary>Value that indicates that the client application does not need to know the exact size of the data available, so the information is read directly from the source.</summary>
            BINDF_DIRECT_READ               = 0x20000,
            /// <summary>Value that indicates that this transaction should be handled as a forms submittal.</summary>
            BINDF_FORMS_SUBMIT              = 0x40000,
            /// <summary>Value that indicates the resource should be retrieved from the cache if the attempt to download the resource from the network fails.</summary>
            BINDF_GETFROMCACHE_IF_NET_FAIL  = 0x80000,
            /// <summary>Value that indicates the binding is from a URL moniker. This value was added for Microsoft┬о Internet Explorer 5.</summary>
            BINDF_FROMURLMON                = 0x100000,
            /// <summary>Value that indicates that the moniker should bind to the copy of the resource that is currently in the Internet cache. If the requested item is not found in the Internet cache, the system will attempt to locate the resource on the network. This value maps to the Microsoft Win32┬о Internet application programming interface (API) flag, INTERNET_FLAG_USE_CACHED_COPY.</summary>
            BINDF_FWD_BACK                  = 0x200000,
            /// <summary>Urlmon.dll searches for temporary or permanent namespace handlers before it uses the default registered handler for particular protocols. This value changes this behavior by allowing the moniker client to specify that Urlmon.dll should look for and use the default system protocol first.</summary>
            BINDF_PREFERDEFAULTHANDLER      = 0x400000,
            /// <summary>Contains the flags that control the encoding of URLs.</summary>
            BINDF_ENFORCERESTRICTED         = 0x800000
        }

        /// <summary>Contains the flags that control the encoding of URLs.</summary>
        public enum URL_ENCODING : int
        {
            /// <summary>Disables URL encoding.</summary>
            URL_ENCODING_NONE               = 0x00000000,
            /// <summary>Enables UTF8 encoding.</summary>
            URL_ENCODING_ENABLE_UTF8        = 0x10000000,
            /// <summary>Disables UTF8 encoding.</summary>
            URL_ENCODING_DISABLE_UTF8       = 0x20000000
        }


        /// <summary>Contains values that are passed to the client application's implementation of the IBindStatusCallback::OnProgress method to indicate the progress of the bind operation.</summary>
        public enum BINDSTATUS : int
        {
            /// <summary>Notifies the client application that the bind operation is finding the resource that holds the object or storage being bound to. The szStatusText parameter to the IBindStatusCallback::OnProgress method provides the display name of the resource being searched for (for example, "www.microsoft.com").</summary>
            BINDSTATUS_FINDINGRESOURCE          = 1,
            /// <summary>Notifies the client application that the bind operation is connecting to the resource that holds the object or storage being bound to. The szStatusText parameter to the IBindStatusCallback::OnProgress method provides the display name of the resource being connected to (for example, an IP address).</summary>
            BINDSTATUS_CONNECTING               = BINDSTATUS_FINDINGRESOURCE + 1,
            /// <summary>Notifies the client application that the bind operation has been redirected to a different data location. The szStatusText parameter to the IBindStatusCallback::OnProgress method provides the display name of the new data location.</summary>
            BINDSTATUS_REDIRECTING              = BINDSTATUS_CONNECTING + 1,
            /// <summary>Notifies the client application that the bind operation has begun receiving the object or storage being bound to. The szStatusText parameter to the IBindStatusCallback::OnProgress method provides the display name of the data location.</summary>
            BINDSTATUS_BEGINDOWNLOADDATA        = BINDSTATUS_REDIRECTING + 1,
            /// <summary>Notifies the client application that the bind operation continues to receive the object or storage being bound to. The szStatusText parameter to the IBindStatusCallback::OnProgress method provides the display name of the data location.</summary>
            BINDSTATUS_DOWNLOADINGDATA          = BINDSTATUS_BEGINDOWNLOADDATA + 1,
            /// <summary>Notifies the client application that the bind operation has finished receiving the object or storage being bound to. The szStatusText parameter to the IBindStatusCallback::OnProgress method provides the display name of the data location.</summary>
            BINDSTATUS_ENDDOWNLOADDATA          = BINDSTATUS_DOWNLOADINGDATA + 1,
            /// <summary>Notifies the client application that the bind operation is beginning to download the component.</summary>
            BINDSTATUS_BEGINDOWNLOADCOMPONENTS  = BINDSTATUS_ENDDOWNLOADDATA + 1,
            /// <summary>Notifies the client application that the bind operation is installing the component.</summary>
            BINDSTATUS_INSTALLINGCOMPONENTS     = BINDSTATUS_BEGINDOWNLOADCOMPONENTS + 1,
            /// <summary>Notifies the client application that the bind operation has finished downloading the component.</summary>
            BINDSTATUS_ENDDOWNLOADCOMPONENTS    = BINDSTATUS_INSTALLINGCOMPONENTS + 1,
            /// <summary>Notifies the client application that the bind operation is retrieving the requested object or storage from a cached copy. The szStatusText parameter to the IBindStatusCallback::OnProgress method is NULL.</summary>
            BINDSTATUS_USINGCACHEDCOPY          = BINDSTATUS_ENDDOWNLOADCOMPONENTS + 1,
            /// <summary>Notifies the client application that the bind operation is requesting the object or storage being bound to. The szStatusText parameter to the IBindStatusCallback::OnProgress method provides the display name of the object (for example, a file name).</summary>
            BINDSTATUS_SENDINGREQUEST           = BINDSTATUS_USINGCACHEDCOPY + 1,
            /// <summary>Notifies the client application that the CLSID of the resource is available.</summary>
            BINDSTATUS_CLASSIDAVAILABLE         = BINDSTATUS_SENDINGREQUEST + 1,
            /// <summary>Notifies the client application that the MIME type of the resource is available.</summary>
            BINDSTATUS_MIMETYPEAVAILABLE        = BINDSTATUS_CLASSIDAVAILABLE + 1,
            /// <summary>Notifies the client application that the temporary or cache file name of the resource is available. The temporary file name might be returned if BINDF_NOWRITECACHE is called. The temporary file will be deleted once the storage is released.</summary>
            BINDSTATUS_CACHEFILENAMEAVAILABLE   = BINDSTATUS_MIMETYPEAVAILABLE + 1,
            /// <summary>Notifies the client application that a synchronous operation has started.</summary>
            BINDSTATUS_BEGINSYNCOPERATION       = BINDSTATUS_CACHEFILENAMEAVAILABLE + 1,
            /// <summary>Notifies the client application that the synchronous operation has completed.</summary>
            BINDSTATUS_ENDSYNCOPERATION         = BINDSTATUS_BEGINSYNCOPERATION + 1,
            /// <summary>Notifies the client application that the file upload has started.</summary>
            BINDSTATUS_BEGINUPLOADDATA          = BINDSTATUS_ENDSYNCOPERATION + 1,
            /// <summary>Notifies the client application that the file upload is in progress.</summary>
            BINDSTATUS_UPLOADINGDATA            = BINDSTATUS_BEGINUPLOADDATA + 1,
            /// <summary> Notifies the client application that the file upload has completed. </summary>
            BINDSTATUS_ENDUPLOADDATA            = BINDSTATUS_UPLOADINGDATA + 1,
            /// <summary>Notifies the client application that the CLSID of the protocol handler being used is available.</summary>
            BINDSTATUS_PROTOCOLCLASSID          = BINDSTATUS_ENDUPLOADDATA + 1,
            /// <summary>Notifies the client application that the Urlmon.dll is encoding data.</summary>
            BINDSTATUS_ENCODING                 = BINDSTATUS_PROTOCOLCLASSID + 1,
            /// <summary> Notifies the client application that the verified MIME type is available. </summary>
            BINDSTATUS_VERIFIEDMIMETYPEAVAILABLE= BINDSTATUS_ENCODING + 1,
            /// <summary>Notifies the client application that the class install location is available.</summary>
            BINDSTATUS_CLASSINSTALLLOCATION     = BINDSTATUS_VERIFIEDMIMETYPEAVAILABLE + 1,
            /// <summary>Notifies the client application that the bind operation is decoding data.</summary>
            BINDSTATUS_DECODING                 = BINDSTATUS_CLASSINSTALLLOCATION + 1,
            /// <summary>Notifies the client application that a pluggable MIME handler is being loaded. This value was added for Microsoft┬о Internet Explorer 5.</summary>
            BINDSTATUS_LOADINGMIMEHANDLER       = BINDSTATUS_DECODING + 1,
            /// <summary>Notifies the client application that this resource contained a Content-Disposition header that indicates that this resource is an attachment. The content of this resource should not be automatically displayed. Client applications should request permission from the user. This value was added for Internet Explorer 5.</summary>
            BINDSTATUS_CONTENTDISPOSITIONATTACH = BINDSTATUS_LOADINGMIMEHANDLER + 1,
            /// <summary>Notifies the client application of the new MIME type of the resource. This is used by a pluggable MIME filter to report a change in the MIME type after it has processed the resource. This value was added for Internet Explorer 5.</summary>
            BINDSTATUS_FILTERREPORTMIMETYPE     = BINDSTATUS_CONTENTDISPOSITIONATTACH + 1,
            /// <summary>Notifies the Urlmon.dll that this CLSID is for the class the Urlmon.dll should return to the client on a call to IMoniker::BindToObject. This value was added for Internet Explorer 5.</summary>
            BINDSTATUS_CLSIDCANINSTANTIATE      = BINDSTATUS_FILTERREPORTMIMETYPE + 1,
            /// <summary>Reports that the IUnknown interface has been released. This value was added for Internet Explorer 5.</summary>
            BINDSTATUS_IUNKNOWNAVAILABLE        = BINDSTATUS_CLSIDCANINSTANTIATE + 1,
            /// <summary>Reports whether or not the client application is connected directly to the pluggable protocol handler. This value was added for Internet Explorer 5.</summary>
            BINDSTATUS_DIRECTBIND               = BINDSTATUS_IUNKNOWNAVAILABLE + 1,
            /// <summary>Reports the MIME type of the resource, before any code sniffing is done. This value was added for Internet Explorer 5.</summary>
            BINDSTATUS_RAWMIMETYPE              = BINDSTATUS_DIRECTBIND + 1,
            /// <summary>Reports that a proxy server has been detected. This value was added for Internet Explorer 5.</summary>
            BINDSTATUS_PROXYDETECTING           = BINDSTATUS_RAWMIMETYPE + 1,
            /// <summary>Reports the valid types of range requests for a resource. This value was added for Internet Explorer 5.</summary>
            BINDSTATUS_ACCEPTRANGES             = BINDSTATUS_PROXYDETECTING + 1,

            BINDSTATUS_COOKIE_SENT              = BINDSTATUS_ACCEPTRANGES + 1,

            BINDSTATUS_COMPACT_POLICY_RECEIVED  = BINDSTATUS_COOKIE_SENT + 1,

            BINDSTATUS_COOKIE_SUPPRESSED        = BINDSTATUS_COMPACT_POLICY_RECEIVED + 1,

            BINDSTATUS_COOKIE_STATE_UNKNOWN     = BINDSTATUS_COOKIE_SUPPRESSED + 1,

            BINDSTATUS_COOKIE_STATE_ACCEPT      = BINDSTATUS_COOKIE_STATE_UNKNOWN + 1,

            BINDSTATUS_COOKIE_STATE_REJECT      = BINDSTATUS_COOKIE_STATE_ACCEPT + 1,

            BINDSTATUS_COOKIE_STATE_PROMPT      = BINDSTATUS_COOKIE_STATE_REJECT + 1,

            BINDSTATUS_COOKIE_STATE_LEASH       = BINDSTATUS_COOKIE_STATE_PROMPT + 1,

            BINDSTATUS_COOKIE_STATE_DOWNGRADE   = BINDSTATUS_COOKIE_STATE_LEASH + 1,

            BINDSTATUS_POLICY_HREF              = BINDSTATUS_COOKIE_STATE_DOWNGRADE + 1,

            BINDSTATUS_P3P_HEADER               = BINDSTATUS_POLICY_HREF + 1,

            BINDSTATUS_SESSION_COOKIE_RECEIVED  = BINDSTATUS_P3P_HEADER + 1,

            BINDSTATUS_PERSISTENT_COOKIE_RECEIVED= BINDSTATUS_SESSION_COOKIE_RECEIVED + 1,

            BINDSTATUS_SESSION_COOKIES_ALLOWED  = BINDSTATUS_PERSISTENT_COOKIE_RECEIVED + 1
        }

        /// <summary>Contains the different options for URL parsing operations. </summary>
        public enum PARSEACTION : int
        {
            /// <summary>Canonicalize the URL.</summary>
            PARSE_CANONICALIZE          =  1,
            /// <summary>Retrieve the user-friendly name for the URL.</summary>
            PARSE_FRIENDLY              =  2,
            /// <summary>Retrieve the URL that should be used by the security manager to make security decisions. The returned URL should either return just the namespace of the protocol or map the protocol to a known protocol (such as HTTP).</summary>
            PARSE_SECURITY_URL          =  3,
            /// <summary>Return the URL of the root document for this site.</summary>
            PARSE_ROOTDOCUMENT          =  4,
            /// <summary>Remove the anchor part of the URL.</summary>
            PARSE_DOCUMENT              =  5,
            /// <summary>Remove everything from the URL before the anchor (#).</summary>
            PARSE_ANCHOR                =  6,
            /// <summary>Encode the URL.</summary>
            PARSE_ENCODE                =  7,
            /// <summary>Decode the URL.</summary>
            PARSE_DECODE                =  8,
            /// <summary>Get the path from the URL, if available.</summary>
            PARSE_PATH_FROM_URL         =  9,
            /// <summary>Create a URL from the given path.</summary>
            PARSE_URL_FROM_PATH         = 10,
            /// <summary>Return the MIME type of this URL.</summary>
            PARSE_MIME                  = 11,
            /// <summary>Return the server name.</summary>
            PARSE_SERVER                = 12,
            /// <summary>Retrieve the schema for this URL.</summary>
            PARSE_SCHEMA                = 13,
            /// <summary>Retrieve the site associated with this URL.</summary>
            PARSE_SITE                  = 14,
            /// <summary>Retrieve the domain associated with this URL.</summary>
            PARSE_DOMAIN                = 15,
            /// <summary>Retrieve the location associated with this URL.</summary>
            PARSE_LOCATION              = 16,
            /// <summary>Retrieve the security form of the URL. The returned URL should return a base URL that contains no user name, password, directory path, resource, or any other extra information.</summary>
            PARSE_SECURITY_DOMAIN       = 17,
            /// <summary>Convert unsafe characters to escape sequences.</summary>
            PARSE_ESCAPE                = 18,
            /// <summary>Convert escape sequences to the characters they represent.</summary>
            PARSE_UNESCAPE              = 19
        }

        /// <summary>Contains the available query options.</summary>
        public enum QUERYOPTION : int
        {
            /// <summary>Request the expiration date in a SYSTEMTIME format.</summary>
            QUERY_EXPIRATION_DATE       =  1,
            /// <summary>Request the last changed date in a SYSTEMTIME format.</summary>
            QUERY_TIME_OF_LAST_CHANGE   =  2,
            /// <summary>Request the content encoding schema.</summary>
            QUERY_CONTENT_ENCODING      =  3,
            /// <summary>Request the content type header.</summary>
            QUERY_CONTENT_TYPE          =  4,
            /// <summary>Request a refresh.</summary>
            QUERY_REFRESH               =  5,
            /// <summary>Combine the page URL with the nearest base URL if TRUE.</summary>
            QUERY_RECOMBINE             =  6,
            /// <summary>Check if the protocol can navigate.</summary>
            QUERY_CAN_NAVIGATE          =  7,
            /// <summary>Check if the URL needs to access the network.</summary>
            QUERY_USES_NETWORK          =  8,
            /// <summary>Check if the resource is cached locally.</summary>
            QUERY_IS_CACHED             =  9,
            /// <summary>Check if this resource is installed locally on a CD-ROM.</summary>
            QUERY_IS_INSTALLEDENTRY     = 10,
            /// <summary>Check if this resource is stored in the cache or if it is on a mapped drive (in a cache container).</summary>
            QUERY_IS_CACHED_OR_MAPPED   = 11,
            /// <summary>Check if the specified protocol uses the Internet cache.</summary>
            QUERY_USES_CACHE            = 12,
            /// <summary>Check if the protocol is encrypted.</summary>
            QUERY_IS_SECURE             = 13,
            /// <summary>Check if the protocol only serves trusted content.</summary>
            QUERY_IS_SAFE               = 14
        }

        public enum BINDSTRING 
        {
            /// <summary>Not currently supported.</summary>
            BINDSTRING_HEADERS = 1,
            /// <summary>Retrieve the accepted MIME types.</summary>
            BINDSTRING_ACCEPT_MIMES,
            /// <summary>Not currently supported.</summary>
            BINDSTRING_EXTRA_URL,
            /// <summary>Not currently supported.</summary>
            BINDSTRING_LANGUAGE,
            /// <summary>Not currently supported.</summary>
            BINDSTRING_USERNAME,
            /// <summary>Not currently supported.</summary>
            BINDSTRING_PASSWORD,
            /// <summary>Not currently supported.</summary>
            BINDSTRING_UA_PIXELS,
            /// <summary>Not currently supported.</summary>
            BINDSTRING_UA_COLOR,
            /// <summary>Not currently supported.</summary>
            BINDSTRING_OS,
            /// <summary>Retrieve the user agent string used.</summary>
            BINDSTRING_USERAGENT,
            /// <summary>Not currently supported.</summary>
            BINDSTRING_ACCEPT_ENCODINGS,
            /// <summary>Retrieve the posted cookie.</summary>
            BINDSTRING_POST_COOKIE,
            /// <summary>Retrieve the MIME type of the posted data.</summary>
            BINDSTRING_POST_DATA_MIME,
            /// <summary>Retrieve the URL.</summary>
            BINDSTRING_URL,
            /// <summary>Retrieve the CLSID of the resource. This value was added for Microsoft® Internet Explorer 5.</summary>
            BINDSTRING_IID,
            /// <summary>Retrieve a string that indicates if the protocol handler is binding to an object. This value was added for Internet Explorer 5.</summary>
            BINDSTRING_FLAG_BIND_TO_OBJECT,
            /// <summary>Retrieve the address of the IBindCtx interface. This value was added for Internet Explorer 5.</summary>
            BINDSTRING_PTR_BIND_CONTEXT
        }








        // interfaces

        /// <summary>This interface provides an implementation of the IUnknown interface, which allows client programs to determine if asynchronous pluggable protocols are supported. No additional methods are supported by this interface.</summary>
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("79eac9e0-baf9-11ce-8c82-00aa004ba90b")]
        public interface IInternet
        {
        }

        /// <summary>This interface is used to control the operation of an asynchronous pluggable protocol handler. </summary>
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("79eac9e3-baf9-11ce-8c82-00aa004ba90b")]
        public interface IInternetProtocolRoot
        {
            /// <summary>Starts the operation. </summary>
            /// <remarks>URL Moniker Error Codes can be returned only by a pluggable namespace handler or MIME filter. Only a single, permanently registered asynchronous pluggable protocol handler can be assigned to a particular scheme (such as FTP), so there are no other handlers to default to.</remarks>
            /// <param name="szUrl">
            /// Address of a string value that contains the URL. For a pluggable MIME filter, this parameter contains the MIME type.</param>
            /// <param name="pOIProtSink">
            /// Address of the protocol sink provided by the client.</param>
            /// <param name="pOIBindInfo">
            /// Address of the IInternetBindInfo interface from which the protocol gets download-specific information.</param>
            /// <param name="grfPI">
            /// Unsigned long integer value that contains the flags that determine if the method only parses or if it parses and downloads the URL. This can be one of the PI_FLAGS values.</param>
            /// <param name="dwReserved">
            /// For pluggable MIME filters, contains the address of a PROTOCOLFILTERDATA structure. Otherwise, it is reserved and must be set to NULL.</param>
            void Start(
                [MarshalAs(UnmanagedType.LPWStr)]
                [In] string szUrl,
                [MarshalAs(UnmanagedType.Interface)]
                [In] IInternetProtocolSink pOIProtSink,
                [MarshalAs(UnmanagedType.Interface)]
                [In] IInternetBindInfo pOIBindInfo,
                [In] NativeMethods.PI_FLAGS grfPI,
                [In] int dwReserved );

            /// <summary>Allows the pluggable protocol handler to continue processing data on the apartment thread. </summary>
            /// <remarks>This method is called in response to a call to the IInternetProtocolSink::Switch method. </remarks>
            /// <param name="pProtocolData">
            /// Address of the PROTOCOLDATA structure data passed to IInternetProtocolSink::Switch.</param>
            void Continue(
                [In] ref PROTOCOLDATA pProtocolData );

            /// <summary>Cancels an operation that is in progress. </summary>
            /// <param name="hrReason">
            /// HRESULT value that contains the reason for canceling the operation. This is the HRESULT that is reported by the pluggable protocol if it successfully canceled the binding. The pluggable protocol passes this HRESULT to urlmon.dll using the IInternetProtocolSink::ReportResult method. Urlmon.dll then passes this HRESULT to the host using IBindStatusCallback::OnStopBinding.</param>
            /// <param name="dwOptions">
            /// Reserved. Must be set to 0.</param>
            void Abort(
                [In] int hrReason,
                [In] int dwOptions );

            /// <summary>Releases the resources used by the pluggable protocol handler. </summary>
            /// <remarks>Note to implementers
            /// Urlmon.dll will not call this method until your asynchronous pluggable protocol handler calls the Urlmon.dll IInternetProtocolSink::ReportResult method. When your IInternetProtocolRoot::Terminate method is called, your asynchronous pluggable protocol handler should free all resources it has allocated.
            /// Note to callers
            /// This method should be called after receiving a call to your IInternetProtocolSink::ReportResult method and after the protocol handler's IInternetProtocol::LockRequest method has been called. </remarks>
            /// <param name="dwOptions">
            /// Reserved. Must be set to 0.</param>
            void Terminate(
                [In] int dwOptions );

            /// <summary>Not currently implemented.</summary>
            void Suspend();

            /// <summary>Not currently implemented. </summary>
            void Resume();
        }

        /// <summary>This is the main interface exposed by an asynchronous pluggable protocol. This interface and the IInternetProtocolSink interface communicate with each other very closely during download operations. </summary>
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("79eac9e4-baf9-11ce-8c82-00aa004ba90b")]
        public interface IInternetProtocol
        {
            /// <summary>Starts the operation. </summary>
            /// <remarks>URL Moniker Error Codes can be returned only by a pluggable namespace handler or MIME filter. Only a single, permanently registered asynchronous pluggable protocol handler can be assigned to a particular scheme (such as FTP), so there are no other handlers to default to.</remarks>
            /// <param name="szUrl">
            /// Address of a string value that contains the URL. For a pluggable MIME filter, this parameter contains the MIME type.</param>
            /// <param name="pOIProtSink">
            /// Address of the protocol sink provided by the client.</param>
            /// <param name="pOIBindInfo">
            /// Address of the IInternetBindInfo interface from which the protocol gets download-specific information.</param>
            /// <param name="grfPI">
            /// Unsigned long integer value that contains the flags that determine if the method only parses or if it parses and downloads the URL. This can be one of the PI_FLAGS values.</param>
            /// <param name="dwReserved">
            /// For pluggable MIME filters, contains the address of a PROTOCOLFILTERDATA structure. Otherwise, it is reserved and must be set to NULL.</param>
            void Start(
                [MarshalAs(UnmanagedType.LPWStr)]
                [In] string szUrl,
                [MarshalAs(UnmanagedType.Interface)]
                [In] IInternetProtocolSink pOIProtSink,
                [MarshalAs(UnmanagedType.Interface)]
                [In] IInternetBindInfo pOIBindInfo,
                [In] NativeMethods.PI_FLAGS grfPI,
                [In] int dwReserved);

            /// <summary>Allows the pluggable protocol handler to continue processing data on the apartment thread. </summary>
            /// <remarks>This method is called in response to a call to the IInternetProtocolSink::Switch method. </remarks>
            /// <param name="pProtocolData">
            /// Address of the PROTOCOLDATA structure data passed to IInternetProtocolSink::Switch.</param>
            void Continue(
                [In] ref NativeMethods.PROTOCOLDATA pProtocolData );

            /// <summary>Cancels an operation that is in progress. </summary>
            /// <param name="hrReason">
            /// HRESULT value that contains the reason for canceling the operation. This is the HRESULT that is reported by the pluggable protocol if it successfully canceled the binding. The pluggable protocol passes this HRESULT to urlmon.dll using the IInternetProtocolSink::ReportResult method. Urlmon.dll then passes this HRESULT to the host using IBindStatusCallback::OnStopBinding.</param>
            /// <param name="dwOptions">
            /// Reserved. Must be set to 0.</param>
            void Abort(
                [In] int hrReason,
                [In] int dwOptions );

            /// <summary>Releases the resources used by the pluggable protocol handler. </summary>
            /// <remarks>Note to implementers
            /// Urlmon.dll will not call this method until your asynchronous pluggable protocol handler calls the Urlmon.dll IInternetProtocolSink::ReportResult method. When your IInternetProtocolRoot::Terminate method is called, your asynchronous pluggable protocol handler should free all resources it has allocated.
            /// Note to callers
            /// This method should be called after receiving a call to your IInternetProtocolSink::ReportResult method and after the protocol handler's IInternetProtocol::LockRequest method has been called. </remarks>
            /// <param name="dwOptions">
            /// Reserved. Must be set to 0.</param>
            void Terminate(
                [In] int dwOptions );

            /// <summary>Not currently implemented.</summary>
            void Suspend();

            /// <summary>Not currently implemented.</summary>
            void Resume();

            /// <summary>Reads data retrieved by the pluggable protocol handler. </summary>
            /// <remarks>Developers who are implementing an asynchronous pluggable protocol must be prepared to have their implementation of IInternetProtocol::Read continue to be called a few extra times after it has returned S_FALSE. </remarks>
            /// <param name="pv">
            /// Address of the buffer where the information will be stored.</param>
            /// <param name="cb">
            /// Value that indicates the size of the buffer.</param>
            /// <param name="pcbRead">
            /// Address of a value that indicates the amount of data stored in the buffer.</param>
            [PreserveSig]
            int Read(
                [In,Out] IntPtr pv,
                [In] int cb,
                [Out] out int pcbRead );

            /// <summary>Moves the current seek offset.</summary>
            /// <param name="dlibMove">
            /// Large integer value that indicates how far to move the offset.</param>
            /// <param name="dwOrigin">
            /// DWORD value that indicates where the move should begin.
            /// FILE_BEGIN : Starting point is zero or the beginning of the file. If FILE_BEGIN is specified, dlibMove is interpreted as an unsigned location for the new file pointer.
            /// FILE_CURRENT : Current value of the file pointer is the starting point.
            /// FILE_END : Current end-of-file position is the starting point. This method fails if the content length is unknown.</param>
            /// <param name="plibNewPosition">
            /// Address of an unsigned long integer value that indicates the new offset.</param>
            void Seek(
                [In] long dlibMove,
                [In] int dwOrigin,
                [Out] out long plibNewPosition );

            /// <summary>Locks the requested resource so that the IInternetProtocolRoot::Terminate method can be called and the remaining data can be read. </summary>
            /// <remarks>For asynchronous pluggable protocols that do not need to lock a request, the method should return S_OK.</remarks>
            /// <param name="dwOptions">
            /// Reserved. Must be set to 0.</param>
            void LockRequest(
                [In] int dwOptions );

            /// <summary>Frees any resources associated with a lock. </summary>
            /// <remarks>This method is called only if IInternetProtocol::LockRequest was called. </remarks>
            void UnlockRequest();            
        }

        /// <summary>This interface receives the reports and binding data from the asynchronous pluggable protocol. It is a free-threaded interface and can be called from any thread. </summary>
        /// <remarks>This interface is implemented by client applications interacting directly with an asynchronous pluggable protocol and pluggable MIME filters.</remarks>
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("79eac9e5-baf9-11ce-8c82-00aa004ba90b")]
        public interface IInternetProtocolSink
        {
            /// <summary>Passes data from an asynchronous pluggable protocol's worker thread intended for the same asynchronous pluggable protocol's apartment thread.</summary>
            /// <remarks>The IInternetProtocolSink interface should pass the data from the worker thread, without modification, back to the asynchronous pluggable protocol by calling its IInternetProtocolRoot::Continue method.
            /// Asynchronous pluggable protocol handlers can operate asynchronously in a separate worker thread. This method allows the protocol handler to pass data back to the apartment (or user interface) thread it was created on.</remarks>
            /// <param name="pProtocolData">
            /// PROTOCOLDATA structure containing the data to be passed back to the apartment thread.</param>
            void Switch(
                [In] ref PROTOCOLDATA pProtocolData );

            /// <summary>Reports progress made during a state operation. </summary>
            /// <remarks>Asynchronous pluggable protocol handlers should not generate the BINDSTATUS, BINDSTATUS, and BINDSTATUS reports, since they are generated internally when the data arrives.</remarks>
            /// <param name="ulStatusCode">
            /// BINDSTATUS value that indicates the status of the state operation.</param>
            /// <param name="szStatusText">
            /// String value that describes the status of the state operation.</param>
            void ReportProgress(
                [In] BINDSTATUS ulStatusCode,
                [MarshalAs(UnmanagedType.LPWStr)]
                [In] string szStatusText );

            /// <summary>Reports the amount of data that is available on the thread. </summary>
            /// <param name="grfBSCF">
            /// BSCF value that indicates the type of data available. BSCF indicates that all available data has been reported.</param>
            /// <param name="ulProgress">
            /// Integer value that indicates the progress made so far.</param>
            /// <param name="ulProgressMax">
            /// Integer value that indicates the total amount of work to be done.</param>
            void ReportData(
                [In] BSCF grfBSCF,
                [In] int ulProgress,
                [In] int ulProgressMax );

            /// <summary>Reports the result of the operation when called on any thread. </summary>
            /// <remarks>Notes to implementers
            /// After your IInternetProtocolSink::ReportResult method is called, your application should call the protocol handler's IInternetProtocol::LockRequest method to lock the resource you are reading from the protocol handler. Then your application should call the protocol handler's IInternetProtocolRoot::Terminate method. 
            /// Notes to callers
            /// The call to IInternetProtocolSink::ReportResult is the last call that your pluggable protocol handler must make to the client application's IInternetProtocolSink interface.</remarks>
            /// <param name="hrResult">
            /// HRESULT value that indicates the result returned by the operation.</param>
            /// <param name="dwError">
            /// Integer value that is a protocol-specific code.</param>
            /// <param name="szResult">
            /// Protocol-specific result string that should be NULL if the operation succeeded.</param>
            void ReportResult(
                [In] int hrResult,
                [In] int dwError,
                [MarshalAs(UnmanagedType.LPWStr)]
                [In] string szResult );
        }

        /// <summary>This interface is implemented by the system and provides data that the protocol might need to bind successfully. </summary>
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("79eac9e1-baf9-11ce-8c82-00aa004ba90b")]
        public interface IInternetBindInfo
        {
            /// <summary>
            /// This method gets the BINDINFO structure associated with the binding operation.
            /// </summary>
            /// <param name="grfBINDF">Address of a value taken from the BINDF enumeration indicating whether the bind should proceed synchronously or asynchronously.</param>
            /// <param name="pbindinfo">Address of the BINDINFO structure, which describes how the client wants the binding to occur.</param>
            void GetBindInfo(
                [Out] out BINDF grfBINDF,
                [In,Out] ref BINDINFO pbindinfo );

            /// <summary>Retrieves the strings needed by the protocol for its operation. </summary>
            /// <remarks>This method is used if the protocol requires any additional information, such as a user name or password needed to access a URL.</remarks>
            /// <param name="ulStringType">
            /// Integer value that indicates the type of string or strings that should be returned. This can be one of the BINDSTRING values.</param>
            /// <param name="ppwzStr">
            /// Address of an array of strings.</param>
            /// <param name="cEl">
            /// Integer value that indicates the number of elements in the array provided.</param>
            /// <param name="pcElFetched">
            /// Address of an integer value that indicates the number of elements in the array that are filled.</param>
            void GetBindString(
                [In] BINDSTRING ulStringType,
                [In] IntPtr ppwzStr,
                [In] int cEl,
                [In,Out] ref int pcElFetched );
        }

        /// <summary>This interface is implemented by the client application to create temporary pluggable protocol handlers.</summary>
        [ComImport, InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        [Guid("79eac9e7-baf9-11ce-8c82-00aa004ba90b")]
        public interface IInternetSession
        {
            /// <summary>Registers a temporary pluggable namespace handler on the current process.</summary>
            /// <remarks>This method only registers a pluggable namespace handler on the current process. No other processes will be affected by this method.
            /// An application can register a pluggable namespace handler for a particular period of time when it wants to handle requests for some protocols by calling IInternetSession::RegisterNameSpace. If ppwzPatterns and cPatterns are NULL, the registered pluggable namespace handler will be called for all protocol requests. This method can be called multiple times using the same interface to register the different namespaces it wants to handle.</remarks>
            /// <param name="pCF">
            /// Pointer to an IClassFactory interface where an IInternetProtocol object can be created.</param>
            /// <param name="rclsid">
            /// Reference to the pluggable namespace handler.</param>
            /// <param name="pwzProtocol">
            /// String value that contains the protocol to be handled.</param>
            /// <param name="cPatterns">
            /// Unsigned long integer that indicates the number of elements in the ppwzPatterns parameter.</param>
            /// <param name="ppwzPatterns">
            /// Array of strings that contain the patterns the handler will be used for.</param>
            /// <param name="dwReserved">
            /// Reserved. Must be set to 0.</param>
            void RegisterNameSpace(
                [MarshalAs(UnmanagedType.Interface)]
                IClassFactory pCF,
                //IntPtr pCF,
                [In] ref Guid rclsid,
                [MarshalAs(UnmanagedType.LPWStr)] string pwzProtocol,
                int cPatterns,
                [In, MarshalAs(UnmanagedType.LPWStr)] ref string ppwzPatterns,
                int dwReserved );

            /// <summary>Unregisters a temporary pluggable namespace handler.</summary>
            /// <param name="pCF">
            /// Address of the IClassFactory interface that created the handler.</param>
            /// <param name="pszProtocol">
            /// Address of a string value that contains the protocol that was handled.</param>
            void UnregisterNameSpace(
                IClassFactory pCF,
                //IntPtr pCF,
                [MarshalAs(UnmanagedType.LPWStr)] string pszProtocol );

            /// <summary>Registers a temporary pluggable MIME filter on the current process.</summary>
            /// <remarks>This method only registers a pluggable MIME filter on the current process. No other processes will be affected by this method.
            /// An application can register a pluggable MIME filter for a particular period of time when it wants to handle requests for some MIMEs by calling IInternetSession::RegisterMimeFilter. This method can be called multiple times using the same interface to register the different MIME types it wants to handle.</remarks>
            /// <param name="pCF">
            /// Address of an IClassFactory interface where an IInternetProtocol object can be created.</param>
            /// <param name="rclsid">
            /// Reference to the pluggable MIME handler.</param>
            /// <param name="pwzType">
            /// String value that contains the MIME to register.</param>
            void RegisterMimeFilter(
                IntPtr pCF,
                [In] ref Guid rclsid,
                [MarshalAs(UnmanagedType.LPWStr)] string pwzType );

            /// <summary>Unregisters a temporary pluggable MIME filter.</summary>
            /// <param name="pCF">
            /// Address of the IClassFactory interface that created the filter.</param>
            /// <param name="pwzType">
            /// String value that indicates the MIME that the filter was handling.</param>
            void UnregisterMimeFilter(
                IntPtr pCF,
                [MarshalAs(UnmanagedType.LPWStr)] string pwzType );

            /// <summary>Not currently implemented.</summary>
            void CreateBinding(
                IntPtr pbc,
                [MarshalAs(UnmanagedType.LPWStr)] string szUrl,
                [MarshalAs(UnmanagedType.IUnknown)] object pUnkOuter,
                [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppunk,
                [Out] out IInternetProtocol ppOInetProt,
                int dwOption );

            /// <summary>Not currently implemented.</summary>
            void SetSessionOption(
                int dwOption,
                [MarshalAs(UnmanagedType.I4)] IntPtr pBuffer,
                int dwBufferLength,
                int dwReserved );

            /// <summary>Not currently implemented.</summary>
            void GetSessionOption (
                int dwOption,
                [MarshalAs(UnmanagedType.I4)] IntPtr pBuffer,
                [In, Out] ref int pdwBufferLength,
                int dwReserved );
        }
    }
}
