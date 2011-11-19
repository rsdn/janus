using System;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	public class JanusDBDriverInfo : IKeyedElementInfo<string>
	{
		private readonly Func<string> _descriptionGetter;
		private readonly Func<string> _displayNameGetter;
		private readonly string _name;
		private readonly Type _type;
		private readonly bool _lockRequired;

		public JanusDBDriverInfo(
			string name,
			Func<string> displayNameGetter,
			Func<string> descriptionGetter,
			Type type,
			bool lockRequired)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			if (displayNameGetter == null)
				throw new ArgumentNullException("displayNameGetter");
			if (descriptionGetter == null)
				throw new ArgumentNullException("descriptionGetter");
			if (type == null)
				throw new ArgumentNullException("type");

			_name = name;
			_lockRequired = lockRequired;
			_displayNameGetter = displayNameGetter;
			_descriptionGetter = descriptionGetter;
			_type = type;
		}

		public string Name
		{
			get { return _name; }
		}

		public Type Type
		{
			get { return _type; }
		}

		public bool LockRequired
		{
			get { return _lockRequired; }
		}

		#region IKeyedElementInfo<string> Members
		///<summary>
		/// Ключ.
		///</summary>
		string IKeyedElementInfo<string>.Key
		{
			get { return _name; }
		}
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