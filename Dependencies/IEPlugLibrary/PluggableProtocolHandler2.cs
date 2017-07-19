using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace Mihailik.InternetExplorer
{
    public abstract class PluggableProtocolHandler2 :
        NativeMethods.IInternetProtocol,
        NativeMethods.IInternetProtocolRoot
    {
        enum ProtocolState
        {
            Initialization,
            Initialized,
            Starting,
            Started,
            FirstPortionProduced,
            ProducingFinished,
            Terminated,
            Aborted
        }

        PluggableProtocolRequest2 m_Request;
        ProtocolState m_State;
        bool m_IsSuspended;
        bool m_IsRequestLocked;

        NativeMethods.IInternetProtocolSink m_Sink;
        NativeMethods.IInternetBindInfo m_BindInfo;
        NativeMethods.PI_FLAGS m_Flags;

        PluggableProtocolResponse2 m_Response;
        PluggableProtocolResponseOutputStream outputStream;

        public PluggableProtocolHandler2()
        {
        }

        public PluggableProtocolRequest2 Request
        {
            get { return m_Request; }
        }

        public PluggableProtocolResponse2 Response
        {
            get { return m_Response; }
        }

        public bool IsSuspended
        {
            get { return m_IsSuspended; }
        }

        public bool IsRequestLocked
        {
            get { return m_IsRequestLocked; }
        }

        public event EventHandler Started;
        public event EventHandler IsSuspendedChanged;

        protected virtual void OnStarted(EventArgs e)
        {
            EventHandler temp = this.Started;
            if (temp != null)
            {
                temp(this,e);
            }
        }

        protected virtual void OnIsSuspendedChanged(EventArgs e)
        {
            EventHandler temp = this.IsSuspendedChanged;
            if (temp != null)
            {
                temp(this,e);
            }
        }

        ProtocolState State
        {
            get { return m_State; }
            set
            {
                Debug.Assert((int)this.State <= (int)value);
                m_State = value;
            }
        }

        NativeMethods.IInternetProtocolSink Sink
        {
            get { return m_Sink; }
        }

        NativeMethods.IInternetBindInfo BindInfo
        {
            get { return m_BindInfo; }
        }

        void ReleaseComSinks()
        {
            if (m_Sink != null)

            {
                Marshal.ReleaseComObject(m_Sink);
                m_Sink = null;
            }

            if (m_BindInfo != null)
            {
                Marshal.ReleaseComObject(m_BindInfo);
                m_BindInfo = null;
            }
        }

        void StartCore(
            string szUrl,
            NativeMethods.IInternetProtocolSink pOIProtSink,
            NativeMethods.IInternetBindInfo pOIBindInfo,
            NativeMethods.PI_FLAGS grfPI,
            int dwReserved)
        {
            bool completedSuccessfully = false;
            try
            {
                Uri url;
                string httpMethod;

                Uri.TryCreate(szUrl, UriKind.Absolute, out url);

                NativeMethods.BINDF bindf;
                NativeMethods.BINDINFO bindinfo = new NativeMethods.BINDINFO();
                bindinfo.cbSize = Marshal.SizeOf(typeof(NativeMethods.BINDINFO));



                //            System.Diagnostics.Trace.WriteLine(
                //                "useragent: "+GetBindString(bind,NativeMethods.BINDSTRING.BINDSTRING_USERAGENT)+"\r\n"+
                //                "url: "+GetBindString(bind,NativeMethods.BINDSTRING.BINDSTRING_URL)+"\r\n"+
                //                "post cookie: "+GetBindString(bind,NativeMethods.BINDSTRING.BINDSTRING_POST_COOKIE)+"\r\n"+
                //                "post MIME: "+GetBindString(bind,NativeMethods.BINDSTRING.BINDSTRING_POST_DATA_MIME) );

                pOIBindInfo.GetBindInfo(out bindf, ref bindinfo);

                switch (bindinfo.dwBindVerb)
                {
                    case NativeMethods.BINDVERB.BINDVERB_GET:
                        httpMethod = "GET";
                        break;

                    case NativeMethods.BINDVERB.BINDVERB_POST:
                        httpMethod = "POST";
                        break;

                    case NativeMethods.BINDVERB.BINDVERB_PUT:
                        httpMethod = "PUT";
                        break;

                    case NativeMethods.BINDVERB.BINDVERB_CUSTOM:
                        httpMethod = Marshal.PtrToStringUni(bindinfo.szCustomVerb);
                        break;

                    default:
                        httpMethod = null;
                        break;
                }


                m_Sink = pOIProtSink;
                m_BindInfo = pOIBindInfo;
                m_Flags = grfPI;

                m_Request = new PluggableProtocolRequest2(
                    szUrl,
                    url,
                    httpMethod,
                    new MemoryStream(new byte[] { }, false));


                outputStream = new PluggableProtocolResponseOutputStream();
                m_Response = new PluggableProtocolResponse2(outputStream);

                outputStream.Written += new EventHandler(outputStream_Written);
                m_Response.Closed += new EventHandler(Response_Closed);

                this.State = ProtocolState.Initialized;

                this.State = ProtocolState.Starting;

                OnStarted(EventArgs.Empty);

                if( this.State == ProtocolState.Starting )
                    this.State = ProtocolState.Started;

                completedSuccessfully = true;
            }
            finally
            {
                if (!completedSuccessfully)
                {
                    if (Debugger.IsAttached)
                        Debugger.Break();
                }
            }
        }



        void Response_Closed(object sender, EventArgs e)
        {
            if (this.State == ProtocolState.Starting
                || this.State == ProtocolState.Started
                || this.State == ProtocolState.FirstPortionProduced)
            {
                this.State = ProtocolState.ProducingFinished;

                this.Sink.ReportData(NativeMethods.BSCF.BSCF_DATAFULLYAVAILABLE, 100, 100);
                outputStream.SetClosed();
            }
        }

        void outputStream_Written(object sender, EventArgs e)
        {
            if (this.State == ProtocolState.Starting || this.State == ProtocolState.Started)
            {
                this.State = ProtocolState.FirstPortionProduced;
            }

            this.Sink.ReportData(
                NativeMethods.BSCF.BSCF_FIRSTDATANOTIFICATION,
                0,
                100);
        }

        void ContinueCore(ref NativeMethods.PROTOCOLDATA pProtocolData)
        {
        }

        void AbortCore(int hrReason, int dwOptions)
        {
            ReleaseComSinks();
            this.State = ProtocolState.Aborted;
        }

        void TerminateCore(int dwOptions)
        {
            ReleaseComSinks();
            this.State = ProtocolState.Terminated;
        }

        void SuspendCore()
        {
            this.m_IsSuspended = true;
        }

        void ResumeCore()
        {
            this.m_IsSuspended = false;
        }

        int ReadCore(IntPtr pv, int cb, out int pcbRead)
        {            
            try
            {
                pcbRead = this.outputStream.ReadToMemory(pv, cb);
                
                if( pcbRead == 0 && this.State == ProtocolState.ProducingFinished )
                    return 1; // S_FALSE (completed)
                else
                    return 0; // S_OK (continue)
            }
            catch (Exception error)
            {
                pcbRead = 0;
                return Marshal.GetHRForException(error);
            }
        }

        void SeekCore(long dlibMove, int dwOrigin, out long plibNewPosition)
        {
            throw new NotSupportedException();
        }

        void LockRequestCore(int dwOptions)
        {
            this.m_IsRequestLocked = true;
        }

        void UnlockRequestCore()
        {
            this.m_IsRequestLocked = false;
        }

        #region IInternetProtocol Members

        void NativeMethods.IInternetProtocol.Start(string szUrl, NativeMethods.IInternetProtocolSink pOIProtSink, NativeMethods.IInternetBindInfo pOIBindInfo, NativeMethods.PI_FLAGS grfPI, int dwReserved)
        {
            this.StartCore(szUrl, pOIProtSink, pOIBindInfo, grfPI, dwReserved);
        }

        void NativeMethods.IInternetProtocol.Continue(ref NativeMethods.PROTOCOLDATA pProtocolData)
        {
            this.ContinueCore(ref pProtocolData);
        }

        void NativeMethods.IInternetProtocol.Abort(int hrReason, int dwOptions)
        {
            this.AbortCore(hrReason, dwOptions);
        }

        void NativeMethods.IInternetProtocol.Terminate(int dwOptions)
        {
            this.TerminateCore(dwOptions);
        }

        void NativeMethods.IInternetProtocol.Suspend()
        {
            this.SuspendCore();
        }

        void NativeMethods.IInternetProtocol.Resume()
        {
            this.ResumeCore();
        }

        int NativeMethods.IInternetProtocol.Read(IntPtr pv, int cb, out int pcbRead)
        {
            return this.ReadCore(pv, cb, out pcbRead);
        }

        void NativeMethods.IInternetProtocol.Seek(long dlibMove, int dwOrigin, out long plibNewPosition)
        {
            this.SeekCore(dlibMove, dwOrigin, out plibNewPosition);
        }

        void NativeMethods.IInternetProtocol.LockRequest(int dwOptions)
        {
            this.LockRequestCore(dwOptions);
        }

        void NativeMethods.IInternetProtocol.UnlockRequest()
        {
            this.UnlockRequestCore();
        }

        #endregion

        #region IInternetProtocolRoot Members

        void NativeMethods.IInternetProtocolRoot.Start(string szUrl, NativeMethods.IInternetProtocolSink pOIProtSink, NativeMethods.IInternetBindInfo pOIBindInfo, NativeMethods.PI_FLAGS grfPI, int dwReserved)
        {
            this.StartCore(szUrl, pOIProtSink, pOIBindInfo, grfPI, dwReserved);
        }

        void NativeMethods.IInternetProtocolRoot.Continue(ref NativeMethods.PROTOCOLDATA pProtocolData)
        {
            this.ContinueCore(ref pProtocolData);
        }

        void NativeMethods.IInternetProtocolRoot.Abort(int hrReason, int dwOptions)
        {
            this.AbortCore(hrReason, dwOptions);
        }

        void NativeMethods.IInternetProtocolRoot.Terminate(int dwOptions)
        {
            this.TerminateCore(dwOptions);
        }

        void NativeMethods.IInternetProtocolRoot.Suspend()
        {
            this.SuspendCore();
        }

        void NativeMethods.IInternetProtocolRoot.Resume()
        {
            this.ResumeCore();
        }

        #endregion
    }
}
