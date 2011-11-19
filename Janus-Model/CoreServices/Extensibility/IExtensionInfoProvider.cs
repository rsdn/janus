using System.Drawing;

namespace Rsdn.Janus
{
	/// <summary>
	/// Поставщик информации о расширении.
	/// </summary>
	public interface IExtensionInfoProvider
	{
		string GetDisplayName();
		Image GetIcon();
	}
}