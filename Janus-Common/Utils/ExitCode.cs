namespace Rsdn.Janus
{
	/// <summary>
	/// Код завершения процесса
	/// </summary>
	public enum ExitCode
	{
		ErrorUnhandledException = int.MinValue,

		ErrorRegisterGoJanusNet = -129,

		Ok = 0,

		AnotherInstanceDetected = 1,
		CheckEnvironmentFailed = 2
	}
}