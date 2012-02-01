namespace Rsdn.Janus
{
	/// <summary>
	/// Temp service. Separate some config data.
	/// </summary>
	public interface IBrowserConfigService
	{
		UrlBehavior Behavior { get; }
	}
}