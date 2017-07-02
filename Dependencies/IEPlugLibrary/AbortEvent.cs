using System;

namespace Mihailik.InternetExplorer
{
	public class AbortEventArgs
	{
        readonly Exception m_AbortException;

		public AbortEventArgs(Exception abortException)
		{
            this.m_AbortException=abortException;
		}

        public Exception AbortException
        {
            get { return m_AbortException; }
        }
	}

    public delegate void AbortEventHandler(object Sender, AbortEventArgs e);
}
