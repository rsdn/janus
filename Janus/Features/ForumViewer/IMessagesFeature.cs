using System;
using System.Collections.Generic;

using Rsdn.Janus.ObjectModel;

namespace Rsdn.Janus
{
	internal interface IMessagesFeature : IFeature
	{
		/// <summary>
		/// Активные сообщения.
		/// </summary>
		IEnumerable<IMsg> ActiveMessages { get; }

		/// <summary>
		/// Возникает когда активные сообщения изменились.
		/// </summary>
		event EventHandler ActiveMessagesChanged;
	}
}