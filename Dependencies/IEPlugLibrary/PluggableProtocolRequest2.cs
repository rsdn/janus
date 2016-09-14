using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Mihailik.InternetExplorer
{
    public sealed class PluggableProtocolRequest2
    {
        readonly string m_RawUrl;
        readonly Uri m_Url;
        string m_HttpMethod;
        Stream m_InputStream;

        internal PluggableProtocolRequest2(
            string rawUrl,
            Uri url,
            string httpMethod,
            Stream inputStream)
        {
            this.m_RawUrl = rawUrl;
            this.m_Url = url;
            this.m_HttpMethod = httpMethod;
            this.m_InputStream = inputStream;
        }

        public string RawUrl { get { return m_RawUrl; } }
        public Uri Url { get { return m_Url; } }
        public string HttpMethod { get { return m_HttpMethod; } }
        public Stream InputStream { get { return m_InputStream; } }
    }
}
