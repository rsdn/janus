using System;
using System.Runtime.InteropServices;

using Microsoft.Win32;

//using Microsoft.InternetExplorer.Urlmon.Interop;

namespace Mihailik.InternetExplorer
{
	public static class PluggableProtocolRegistrationServices
	{
        public static void RegisterPermanentProtocolHandler<T>(string protocol)
        {
            CheckHandlerType(typeof(T));

            try
            {
                AdminRegisterPermanentProtocolHandler<T>(protocol);
            }
            catch (UnauthorizedAccessException)
            {
                NonAdminRegisterPermanentProtocolHandler<T>(protocol);
            }
        }

        static void AdminRegisterPermanentProtocolHandler<T>(string protocol)
        {
            RegistryKey handlerKey = null;
            try
            {
                string keyPath = "PROTOCOLS\\Handler\\" + protocol;
                try { handlerKey = Registry.ClassesRoot.OpenSubKey(keyPath, true); }
                catch { }
                if (handlerKey == null)
                    handlerKey = Registry.ClassesRoot.CreateSubKey(keyPath);

                using (handlerKey)
                {
                    handlerKey.SetValue(
                        "CLSID",
                        typeof(T).GUID.ToString("B"));
                }
            }
            finally
            {
                if (handlerKey != null)
                    handlerKey.Close();
            }
        }

        static void NonAdminRegisterPermanentProtocolHandler<T>(string protocol)
        {
            RegistryKey handlerKey = null;
            try
            {
                string keyPath = "Software\\Classes\\PROTOCOLS\\Handler\\" + protocol;
                try { handlerKey = Registry.CurrentUser.OpenSubKey(keyPath, true); }
                catch { }
                if (handlerKey == null)
                    handlerKey = Registry.CurrentUser.CreateSubKey(keyPath);

                using (handlerKey)
                {
                    handlerKey.SetValue(
                        "CLSID",
                        typeof(T).GUID.ToString("B"));
                }
            }
            finally
            {
                if (handlerKey != null)
                    handlerKey.Close();
            }
        }

        public static void UnregisterPermanentProtocolHandler<T>(string protocol)
        {
            CheckHandlerType(typeof(T));

            RegistryKey handlerKey=null;
            try
            {
                string keyPath="PROTOCOLS\\Handler\\"+protocol;
                Registry.ClassesRoot.DeleteSubKey(keyPath,true);
            }
            finally
            {
                if( handlerKey!=null )
                    handlerKey.Close();
            }        
        }

        sealed class ClassFactory<T> : NativeMethods.IClassFactory
        {
            public static ClassFactory<T> Instance = new ClassFactory<T>();

            private ClassFactory()
            {
            }

            IntPtr NativeMethods.IClassFactory.CreateInstance(object pOuterUnk, ref Guid iid)
            {
                if( pOuterUnk!=null )
                {
                    const int CLASS_E_NOAGGREGATION  = unchecked((int)0x80040110);
                    throw new COMException("The pUnkOuter parameter was non-NULL and the object does not support aggregation.", CLASS_E_NOAGGREGATION);
                }

                if( iid==NativeMethods.IID_IUnknown
                    || iid==typeof(NativeMethods.IInternetProtocol).GUID
                    || iid==typeof(NativeMethods.IInternetProtocolRoot).GUID )
                {
                    object obj = Activator.CreateInstance<T>();
                    IntPtr objPtr = Marshal.GetIUnknownForObject(obj);
                    IntPtr resultPtr;
                    Guid refIid = iid;
                    Marshal.QueryInterface(objPtr, ref refIid, out resultPtr);
                    return resultPtr;
                }

                const int E_NOINTERFACE = unchecked((int)0x80004002L);
                throw new COMException("The object that ppvObject points to does not support the interface identified by riid.", E_NOINTERFACE);
            }

            void NativeMethods.IClassFactory.LockServer(bool Lock)
            {
            }
        }

        public static void RegisterTemporaryProtocolHandler<T>(string protocol)
        {
            string emptyStr=null;

            NativeMethods.IInternetSession session=GetSession();
            try
            {
                Guid handlerGuid = typeof(T).GUID;
                session.RegisterNameSpace(
                    ClassFactory<T>.Instance,
                    ref handlerGuid,
                    protocol,
                    0,
                    ref emptyStr,
                    0 );
            }
            finally
            {
                Marshal.ReleaseComObject(session);
                session=null;
            }
        }

        public static void UnregisterTemporaryProtocolHandler<T>(string protocol)
        {
            NativeMethods.IInternetSession session = GetSession();
            try
            {
                session.UnregisterNameSpace(
                    ClassFactory<T>.Instance,
                    protocol);
            }
            finally
            {
                Marshal.ReleaseComObject(session);
                session = null;
            }
        }

        static NativeMethods.IInternetSession GetSession()
        {
            NativeMethods.IInternetSession session;
            NativeMethods.CoInternetGetSession(
                IntPtr.Zero,
                out session,
                IntPtr.Zero );

            if( session==null )
                throw new InvalidOperationException("CoInternetGetSession failed with null result.");

            return session;
        }

        static void CheckHandlerType( Type handlerType )
        {
            //if( !typeof(IInternetProtocolRoot).IsAssignableFrom(handlerType) )
            //    throw new ArgumentException("HandlerType must implement IInternetProtocolRoot interface.");
            if( !typeof(NativeMethods.IInternetProtocol).IsAssignableFrom(handlerType) )
                throw new ArgumentException("HandlerType must implement IInternetProtocol interface.");
        }


    }
}
