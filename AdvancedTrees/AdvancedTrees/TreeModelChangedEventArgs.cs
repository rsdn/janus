namespace AdvancedTrees
{
	public class TreeModelChangedEventArgs<T>
	{
		public T Parent { get; private set; }
		public int ChildIndex { get; private set; }
		public int RemovedCount { get; private set; }
		public int AddedCount { get; private set; }

		public TreeModelChangedEventArgs(T parent, int childIndex, int removedCount, int addedCount)
		{
			Parent = parent;
			ChildIndex = childIndex;
			RemovedCount = removedCount;
			AddedCount = addedCount;
		}
	}
}