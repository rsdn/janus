namespace Rsdn.Janus
{
	public delegate ParsingResult<TInput, TValue> Parser<TInput, TValue>(TInput input);
}