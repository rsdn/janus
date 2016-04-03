using System;
using System.Resources;

using CodeJam;
using CodeJam.Extensibility;
using CodeJam.Extensibility.Registration;

namespace Rsdn.Janus
{
	internal class DBDriverRegStrategy
		: RegKeyedElementsStrategy<string, JanusDBDriverInfo, JanusDBDriverAttribute>
	{
		public DBDriverRegStrategy(IServicePublisher publisher) : base(publisher) {}

		///<summary>
		/// Создать элемент.
		///</summary>
		public override JanusDBDriverInfo CreateElement(
			ExtensionAttachmentContext context,
			JanusDBDriverAttribute attr)
		{
			if (!typeof (IDBDriver).IsAssignableFrom(context.Type))
				throw new ExtensibilityException($"Type '{context.Type}' must implement interface '{typeof(IDBDriver)}'");
			return new JanusDBDriverInfo(
				attr.Name,
				() => RetreiveResource(
					context.Type,
					attr.TextResourceFile,
					attr.DisplayNameResource,
					attr.Name),
				() => RetreiveResource(
					context.Type,
					attr.TextResourceFile,
					attr.DescriptionResource,
					""),
				context.Type,
				attr.LockRequired);
		}

		private static string RetreiveResource(Type type, string resource, string valueName, string defValue)
		{
			if (resource.IsNullOrEmpty())
				return defValue;
			var rm = new ResourceManager(resource, type.Assembly);
			return rm.GetString(valueName);
		}
	}
}
