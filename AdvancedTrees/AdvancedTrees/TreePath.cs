namespace AdvancedTrees
{
	public class TreePath<T>
	{
		private readonly TreePath<T> _parentPath;
		private readonly T _lastPathComponent;
		private readonly int _length;

		public TreePath(T lastPathComponent) : this(null, lastPathComponent) { }

		public TreePath(TreePath<T> parentPath, T lastPathComponent)
		{
			_parentPath = parentPath;
			_lastPathComponent = lastPathComponent;
			_length = ParentPath != null ? ParentPath.Length + 1 : 1;
		}

		public T LastPathComponent
		{
			get { return _lastPathComponent; }
		}

		public TreePath<T> ParentPath
		{
			get { return _parentPath; }
		}

		public int Length
		{
			get { return _length; }
		}

		public TreePath<T> Add(T component)
		{
			return new TreePath<T>(this, component);
		}

		public T[] GetPath()
		{
			var i = Length;
			var result = new T[i--];
			for (var path = this; path != null; path = path.ParentPath)
				result[i--] = path.LastPathComponent;
			return result;
		}
	}
}