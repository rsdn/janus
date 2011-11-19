namespace Rsdn.Janus
{
	/// <summary>
	/// Пустые значения в индексе.
	/// </summary>
	public enum IndexNullAllowance
	{
		/// <summary>
		/// Пустые значения разрешены.
		/// </summary>
		Allow,

		/// <summary>
		/// Пустые значения запрешены.
		/// </summary>
		Disallow,

		/// <summary>
		/// Пустые значения игнорируются.
		/// </summary>
		Ignore
	}
}