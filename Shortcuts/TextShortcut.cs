using System.Windows.Forms;

namespace Rsdn.Shortcuts
{
	/// <summary>
	/// Класс с полным описанием конкретного метода
	/// к которому привязана горячая клавиша.
	/// </summary>
	public class TextShortcut
	{
		#region  Class Variables
		private Shortcut _shortcut;
		#endregion

		#region Constructors
		public TextShortcut(string methodName, Shortcut shortcut)
			: this(methodName, string.Empty, shortcut)
		{}

		public TextShortcut(string methodName, string shortName, Shortcut shortcut)
			: this(methodName, shortName, string.Empty, shortcut)
		{}

		public TextShortcut(string methodName,
			string shortName, string longName, Shortcut shortcut)
		{
			MethodName = methodName;
			_shortcut = shortcut;
			ShortName = shortName;
			LongName = longName;
		}
		#endregion

		#region Properties
		public Shortcut Shortcut
		{
			get { return _shortcut; }
			set { _shortcut = value; }
		}

		public string MethodName { get; set; }

		public string ShortName { get; set; }

		public string LongName { get; set; }
		#endregion

		#region Overrides
		public override string ToString()
		{
			return MethodName + " " + _shortcut;
		}
		#endregion
	}
}