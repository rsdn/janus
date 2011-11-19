using System;
using System.Data.Common;
using System.Drawing;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	public partial class DBConfigControlBase: UserControl, IDBConfigControl
	{
		public DBConfigControlBase()
		{
			InitializeComponent();
		}

		protected virtual DbConnectionStringBuilder ConnectionStringBuilder
		{
			get { throw new NotImplementedException(); }
		}

		protected void OnConnectionStringChanged()
		{
			if (ConnectionStringChanged != null)
				ConnectionStringChanged(this, EventArgs.Empty);
		}

		#region IDBConfigControl Members

		public string ConnectionString
		{
			get { return ConnectionStringBuilder.ConnectionString; }
		}

		private bool _connectSuccess;
		public virtual bool ConnectSuccess
		{
			get { return _connectSuccess; }
		}

		public event EventHandler ConnectionStringChanged;

		public virtual void CustomInitialize(bool localize)
		{
			throw new NotImplementedException();
		}

		public void DockInPanel(Panel panel)
		{
			panel.Controls.Add(this);
			Dock = DockStyle.Fill;
			Location = new Point(0, 0);
		}

		public virtual void BuildConnectionString()
		{
			_connectSuccess = false;
		}

		public virtual bool PrepareCreateConnectionString()
		{
			BuildConnectionString();
			return true;
		}

		public virtual void OnConnectSucceeded()
		{
			_connectSuccess = true;
		}

		#endregion
	}
}
