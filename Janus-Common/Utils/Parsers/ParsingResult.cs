namespace Rsdn.Janus
{
	public class ParsingResult<TInput, TValue>
	{
		private readonly TInput _inputRest;
		private readonly TValue _value;

		public ParsingResult(TInput inputRest, TValue value)
		{
			_inputRest = inputRest;
			_value = value;
		}

		public TInput InputRest
		{
			get { return _inputRest; }
		}

		public TValue Value
		{
			get { return _value; }
		}
	}
}