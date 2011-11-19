using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	/// <summary>
	/// Атрибут Category, выбирающий локализацию из Janus.exe.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	[MeansImplicitUse]
	public sealed class JanusDisplayNameAttribute : DisplayNameAttribute
	{
		/// <summary>
		/// Инициализирует экземпляр.
		/// </summary>
		public JanusDisplayNameAttribute(string displayName) : base(displayName)
		{}

		/// <summary>
		/// Локализует строку.
		/// </summary>
		protected override string GetLocalizedString(string value)
		{
			var ls = SR.ResourceManager.GetString(value);
			return ls ?? "<" + value + ">";
		}
	}
}
