namespace Rsdn.Janus
{
	/// <summary>
	/// Навеяно постом orangy :)
	/// Принимаются предложения по усовершенствованию
	/// </summary>
	public sealed class QuoteAnalyzer
	{
		private readonly string _originalContent;
		private readonly int _diffWeight  = 10;
		private          int _minPostSize = 700;

		public QuoteAnalyzer(string originalContent)
		{
			_originalContent = originalContent;
		}

		public bool Enabled
		{
			get { return _originalContent.Length > _minPostSize; }
		}

		public int MinPostSize
		{
			get { return _minPostSize; }
			set { _minPostSize = value; }
		}
		
		public bool IsOverquoted(string content)
		{
			// отсекаем любые ответы без дописывания чего либо
			if (content.Trim().Length == _originalContent.Length
				&& content.Trim().StartsWith(_originalContent))
				return true;

			// если большое сообщение начинаем проверку
			if (!Enabled
				|| content.Length < _originalContent.Length
				|| !content.Trim().StartsWith(_originalContent))
				return false;

			// проверка на вес ответа
			int diff = content.Length - _originalContent.Length;

			if (diff >= _originalContent.Length)
				return false;

			return (_originalContent.Length / diff) > _diffWeight;
		}
	}
}