namespace Rsdn.TreeGrid
{
	/// <summary>
	/// Хранит уникальный ключ пути.
	/// </summary>
	public class PathKey
	{
		private readonly string[] _nodeKeys;

		internal PathKey(string[] nodeKeys)
		{
			_nodeKeys = nodeKeys;
		}

		internal string[] NodeKeys
		{
			get { return _nodeKeys; }
		}
	}
}