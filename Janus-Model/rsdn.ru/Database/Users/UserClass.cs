namespace Rsdn.Janus
{
	/// <summary>
	/// Класс пользователя.
	/// </summary>
	public enum UserClass
	{
		/// <summary>
		/// Группа пользователей (используется для авторов статьи).
		/// </summary>
		Group = -2,

		/// <summary>
		/// Аноним.
		/// </summary>
		Anonym = -1,

		/// <summary>
		/// Админ.
		/// </summary>
		Admin = 0,

		/// <summary>
		/// Модератор.
		/// </summary>
		Moderator = 1,

		/// <summary>
		/// Тимер.
		/// </summary>
		Team = 2,

		/// <summary>
		/// Зарегистрированный пользователь.
		/// </summary>
		User = 3,

		/// <summary>
		/// Эксперт.
		/// </summary>
		Expert = 4
	}
}