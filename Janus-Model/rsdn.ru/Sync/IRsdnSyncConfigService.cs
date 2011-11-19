namespace Rsdn.Janus
{
	/// <summary>
	/// Temp service. Must be eliminated after config extensibility refactoring complete.
	/// </summary>
	public interface IRsdnSyncConfigService
	{
		IRsdnSyncConfig GetConfig();
		void SetSelfID(int id);
	}
}