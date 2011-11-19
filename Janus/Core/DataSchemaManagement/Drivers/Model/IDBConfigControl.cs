using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Rsdn.Janus
{
	/// <summary>
	/// Интерфейс контрола конфигурации БД (включаемого в качестве 
	/// вкладки в диалог конфигурации БД)
	/// </summary>
	public interface IDBConfigControl
	{
		string ConnectionString { get; }
		bool ConnectSuccess { get; }

		event EventHandler ConnectionStringChanged;

		void CustomInitialize(bool localize);
		void DockInPanel(Panel panel);

		void BuildConnectionString();
		bool PrepareCreateConnectionString();
		void OnConnectSucceeded();
	}
}
