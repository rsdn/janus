using System;
using System.Windows.Forms;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Страница навигации.
	/// </summary>
	public interface INavigationPage : IDisposable
	{
		/// <summary>
		/// Имя страницы.
		/// </summary>
		[NotNull]
		string Name { get; }

		/// <summary>
		/// Состояние.
		/// </summary>
		[CanBeNull]
		NavigationPageState State { get; }

		[NotNull]
		INavigationItemHeader Header { get; }

		/// <summary>
		/// Закрыта ли страница.
		/// </summary>
		bool IsDisposed { get; }

		/// <summary>
		/// Событие о том, что страница была уничтожена.
		/// </summary>
		[NotNull]
		IObservable<EventArgs> Disposed { get; }

		/// <summary>
		/// Создает <see cref="Control"/>, представляющий интерфейс станицы.
		/// </summary>
		[NotNull]
		Control CreateControl();
	}
}