using System;
using System.Net.Mime;

using Rsdn.Janus.Framework.Ipc;
using Rsdn.Janus.Framework.Networking;

namespace Rsdn.Janus.Protocol
{
	/// <summary>
	/// Поставщик ресурсов, работиающий через пайп.
	/// </summary>
	internal class JanusPipeResourceProvider : IResourceProvider
	{
		private const string _pipeName = "JanusPipe";

		#region IResourceProvider Members
		public string Name
		{
			get { return "Janus pipe resource provider"; }
		}

		public Resource GetData(string uri)
		{
			using (var cpc = new ClientPipeConnection(_pipeName))
				try
				{
					cpc.Connect();
					cpc.Write("<protocol-request><path>" + uri + "</path></protocol-request>");
					return Resource.Unpack(cpc.ReadBytes());
				}
				catch (Exception e)
				{
					return new Resource(MediaTypeNames.Text.Html,
						"<html><h3>Ошибка. Источник данных не обнаружен. Возможно RSDN@Home не запущен." +
							"</h3><pre style='background-color: #EEEEEE'>" + e + "</pre></html>");
				}
		}
		#endregion
	}
}