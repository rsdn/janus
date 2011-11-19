using System;
using System.Runtime.Serialization;
namespace Rsdn.Janus
{
	/// <summary>
	/// Исключение, возникающее при обработке схемы.
	/// </summary>
	public class DBSchemaException : ApplicationException
	{
		// TODO: Добавить конструктор без параметров с дефолтным сообщением из ресурсов

		public DBSchemaException(string message, Exception innerException)
			: base(message, innerException)
		{
		}

		/// <summary>
		/// Конструктор десериализации.
		/// </summary>
		protected DBSchemaException(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{ }
	}
}
