namespace Rsdn.Janus
{
	public class Highlighting
	{
		public Highlighting(HighlightType type, int position, int length)
		{
			Type = type;
			Position = position;
			Length = length;
		}

		public HighlightType Type { get; }

		public int Position { get; }

		public int Length { get; }
	}
}