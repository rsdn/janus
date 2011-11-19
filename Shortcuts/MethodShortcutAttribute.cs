using System;
using System.Windows.Forms;

using JetBrains.Annotations;

namespace Rsdn.Shortcuts
{
	[AttributeUsage(AttributeTargets.Method)]
	[MeansImplicitUse]
	public class MethodShortcutAttribute : Attribute
	{
		#region  Class Variables
		private readonly Shortcut _key;
		private readonly string _longName;
		private readonly string _shortName;
		#endregion

		#region  Constructors
		public MethodShortcutAttribute()
			: this(Shortcut.None, string.Empty, string.Empty)
		{}

		public MethodShortcutAttribute(string shortName)
			: this(Shortcut.None, shortName, string.Empty)
		{}

		public MethodShortcutAttribute(string shortName, string longName)
			: this(Shortcut.None, shortName, longName)
		{}

		public MethodShortcutAttribute(Shortcut key)
			: this(key, string.Empty, string.Empty)
		{}

		public MethodShortcutAttribute(Shortcut key, string shortName)
			: this(key, shortName, string.Empty)
		{}

		public MethodShortcutAttribute(Shortcut key, string shortName, string longName)
		{
			_key = key;
			_shortName = shortName;
			_longName = longName;
		}
		#endregion

		#region Properties
		public Shortcut Shortcut
		{
			get { return _key; }
		}

		public string ShortName
		{
			get { return _shortName; }
		}

		public string LongName
		{
			get { return _longName; }
		}
		#endregion
	}
}