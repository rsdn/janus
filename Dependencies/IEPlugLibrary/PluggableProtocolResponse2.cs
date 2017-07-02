using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mihailik.InternetExplorer
{
    public sealed class PluggableProtocolResponse2
    {
        readonly Stream m_OutputStream;

        internal PluggableProtocolResponse2(Stream outputStream)
        {
            this.m_OutputStream = outputStream;
        }

        public Stream OutputStream
        {
            get { return m_OutputStream; }
        }

        public event EventHandler Closed;

        public void Close()
        {
            EventHandler temp = this.Closed;
            if (temp != null)
            {
                temp(this,EventArgs.Empty);
            }
        }
    }
}
