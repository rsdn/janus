using System;

namespace Rsdn.Janus
{
	/// <summary>
	/// Контекст команды.
	/// </summary>
	public interface ICommandContext : IServiceProvider
	{
		/// <summary>
		/// Параметры команды.
		/// </summary>
		object this[string name] { get; }
		
		/// <summary>
		/// Проверяет, имеется ли в комнтексте параметр с заданным именем.
		/// </summary>
		bool IsParameterExists(string name);

		/// <summary>
		/// Пишет заданный текст в вывод команды.
		/// </summary>
		void WriteToOutput(string text);
	}
}