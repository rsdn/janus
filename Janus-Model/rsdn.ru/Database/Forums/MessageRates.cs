namespace Rsdn.Janus
{
	/// <summary>
	/// Оценки сообщения
	/// </summary>
	public enum MessageRates
	{
		/// <summary>
		/// Оценка 1
		/// </summary>
		Rate1 = 1,

		/// <summary>
		/// Оценка 2
		/// </summary>
		Rate2 = 2,

		/// <summary>
		/// Оценка 3
		/// </summary>
		Rate3 = 3,

		/// <summary>
		/// Смешно
		/// </summary>
		Smile = -2,

		/// <summary>
		/// Не согласен
		/// </summary>
		DisAgree = 0,

		/// <summary>
		/// Согласен
		/// </summary>
		Agree = -4,

		/// <summary>
		/// +1
		/// </summary>
		Plus1 = -3,

		/// <summary>
		/// Удалить оценку
		/// </summary>
		DeleteRate = -1,

		/// <summary>
		/// Удалить оценку из локальной БД
		/// </summary>
		DeleteLocally = 1000
	}
}
