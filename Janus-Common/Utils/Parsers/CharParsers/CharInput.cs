namespace Rsdn.Janus
{
	public abstract class CharInput
	{
		public abstract char Current { get; }
		public abstract int Position { get; }
		public abstract CharInput Next();
	}
}