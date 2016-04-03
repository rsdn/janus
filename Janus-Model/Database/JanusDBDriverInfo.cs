using System;

using CodeJam.Extensibility.Registration;

namespace Rsdn.Janus
{
	public class JanusDBDriverInfo : IKeyedElementInfo<string>
	{
		private readonly Func<string> _descriptionGetter;
		private readonly Func<string> _displayNameGetter;
		private readonly string _name;

		public JanusDBDriverInfo(
			string name,
			Func<string> displayNameGetter,
			Func<string> descriptionGetter,
			Type type,
			bool lockRequired)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));
			if (displayNameGetter == null)
				throw new ArgumentNullException(nameof(displayNameGetter));
			if (descriptionGetter == null)
				throw new ArgumentNullException(nameof(descriptionGetter));
			if (type == null)
				throw new ArgumentNullException(nameof(type));

			_name = name;
			LockRequired = lockRequired;
			_displayNameGetter = displayNameGetter;
			_descriptionGetter = descriptionGetter;
			Type = type;
		}

		public string Name => _name;

		public Type Type { get; }

		public bool LockRequired { get; }

		#region IKeyedElementInfo<string> Members
		///<summary>
		/// Ключ.
		///</summary>
		string IKeyedElementInfo<string>.Key => _name;
		#endregion

		public string GetDisplayName()
		{
			return _displayNameGetter();
		}

		public string GetDescription()
		{
			return _descriptionGetter();
		}

		///<summary>
		///Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
		///</summary>
		public override string ToString()
		{
			return GetDisplayName();
		}
	}
}