using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[MeansImplicitUse]
	[BaseTypeRequired(typeof (IDBDriver))]
	public class JanusDBDriverAttribute : Attribute
	{
		private readonly string _descriptionResource;
		private readonly string _displayNameResource;
		private readonly string _name;
		private readonly string _textResourceFile;

		public JanusDBDriverAttribute(
			string name,
			string textResourceFile,
			string displayNameResource,
			string descriptionResource)
		{
			if (name == null)
				throw new ArgumentNullException("name");
			_name = name;
			_textResourceFile = textResourceFile;
			_displayNameResource = displayNameResource;
			_descriptionResource = descriptionResource;
			LockRequired = false;
		}

		public string Name
		{
			get { return _name; }
		}

		public string TextResourceFile
		{
			get { return _textResourceFile; }
		}

		public string DescriptionResource
		{
			get { return _descriptionResource; }
		}

		public string DisplayNameResource
		{
			get { return _displayNameResource; }
		}

		public bool LockRequired { get; set; }
	}
}