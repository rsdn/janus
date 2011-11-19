using System;
using System.Drawing;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	public interface INavigationItemHeader
	{
		/// <summary>
		/// Отображаемый пользователю заголовок.
		/// </summary>
		[NotNull]
		string DisplayName { get; }

		/// <summary>
		/// Отображаемая пользователю дополнительная информация.
		/// </summary>
		[CanBeNull]
		string Info { get; }

		/// <summary>
		/// Пиктограмма.
		/// </summary>
		[CanBeNull]
		Image Image { get; }

		/// <summary>
		/// Подсветка.
		/// </summary>
		bool IsHighlighted { get; }

		/// <summary>
		/// Событие о том, что одно или несколько из свойств <see cref="DisplayName"/>,
		/// <see cref="Info"/> или <see cref="Image"/> изменилось.
		/// </summary>
		[NotNull]
		IObservable<EventArgs> Changed { get; }
	}
}