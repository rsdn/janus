namespace Rsdn.Janus
{
	/// <summary>
	/// Режим восстановления веток
	/// </summary>
	public enum TopicRepairMode
	{
		/// <summary>
		/// До корня.
		/// </summary>
		[JanusDisplayName("BranchRepairModeUpToRoot")]
		UpToRoot,

		/// <summary>
		/// Топик целиком.
		/// </summary>
		[JanusDisplayName("BranchRepairModeWholeTopic")]
		WholeTopic
	}
}
