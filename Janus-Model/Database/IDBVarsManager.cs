namespace Rsdn.Janus
{
	public interface IDBVarsManager
	{
		string this[string name] { get; set; }
	}
}