using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Атрибут для обработчика ресурса протокола janus://
	/// в виде пары [enum, id].
	/// </summary>
	/// 
	[AttributeUsage(AttributeTargets.Method)]
	[MeansImplicitUse]
	public class JanusProtocolEventHandlerAttribute : Attribute
	{
		private readonly string _id;

		public JanusProtocolEventHandlerAttribute(string id)
		{
			_id = id;
		}

		/// <summary>
		/// Строковый идентификатор ресурса.
		/// </summary>
		public string Id
		{
			get { return _id; }
		}
	}
}
