using System;

using JetBrains.Annotations;

namespace Rsdn.Janus
{
	[MeansImplicitUse]
	[BaseTypeRequired(typeof (IDBDriver))]
	public class JanusDBDriverAttribute : Attribute
	{
		public JanusDBDriverAttribute(
			string name,
			string textResourceFile,
			string displayNameResource,
			string descriptionResource)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));
			Name = name;
			TextResourceFile = textResourceFile;
			DisplayNameResource = displayNameResource;
			DescriptionResource = descriptionResource;
			LockRequired = false;
		}

		public string Name { get; }

		public string TextResourceFile { get; }

		public string DescriptionResource { get; }

		public string DisplayNameResource { get; }

		public bool LockRequired { get; set; }
	}
}