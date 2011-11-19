using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Сервис расширяемого меню.
	/// </summary>
	public interface IMenuService
	{
		/// <summary>
		/// Получить меню.
		/// </summary>
		[NotNull]
		IMenuRoot GetMenu([NotNull] string menuName);

		/// <summary>
		/// Cобытие об изменении меню.
		/// </summary>
		[NotNull]
		IObservable<string> MenuChanged { get; }
	}
}