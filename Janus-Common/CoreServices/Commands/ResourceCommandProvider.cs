using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Resources;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Rsdn.Janus
{
	public abstract class ResourceCommandProvider : ICommandProvider
	{
		private const string _commandDescriptorSchemaResource =
			"Rsdn.Janus.CoreServices.Commands.JanusCommandDescriptor.xsd";

		private static readonly XmlReaderSettings _readerSettings;

		private readonly string _menuResourceName;
		private readonly ResourceManager _resourceManager;

		protected ResourceCommandProvider(
			[Localizable(false)] string commandResourceName,
			[Localizable(false)] string stringResourceName)
		{
			if (string.IsNullOrEmpty(commandResourceName))
				throw new ArgumentException(
					@"Аргумент не должен быть null или пустой строкой.",
					"commandResourceName");
			if (string.IsNullOrEmpty(stringResourceName))
				throw new ArgumentException(
					@"Аргумент не должен быть null или пустой строкой.",
					"commandResourceName");

			_menuResourceName = commandResourceName;
			_resourceManager = new ResourceManager(stringResourceName, GetType().Assembly);
		}

		static ResourceCommandProvider()
		{
			_readerSettings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
			_readerSettings.Schemas.Add(CommonTypesSchema.Schema);
			_readerSettings.Schemas.Add(
				XmlSchema.Read(
					typeof(ResourceMenuProvider)
						.Assembly
						.GetRequiredResourceStream(_commandDescriptorSchemaResource),
					null));
		}

		#region ICommandProvider Members

		public IEnumerable<ICommandInfo> CreateCommands()
		{
			return XElement.Load(
				XmlReader.Create(
					GetType().Assembly.GetRequiredResourceStream(_menuResourceName),
					_readerSettings))
				.Elements()
				.Select<XElement, ICommandInfo>(CreateCommand);
		}

		#endregion

		private CommandInfo CreateCommand(XElement commandElement)
		{
			var commandName = (string)commandElement.Attribute(XName.Get(@"name"));
			return new CommandInfo(
				commandName,
				(CommandType)Enum.Parse(
					typeof(CommandType), (string)commandElement.Attribute(XName.Get(@"type"))),
				commandElement.Elements().Select<XElement, ICommandParameterInfo>(
					parameterElement =>
						new CommandParameterInfo(
							(string)parameterElement.Attribute(XName.Get(@"name")),
							(bool)parameterElement.Attribute(XName.Get(@"isOptional")),
							_resourceManager.GetDisplayString(
								(string)parameterElement.Attribute(XName.Get(@"description"))))),
				_resourceManager.GetDisplayString(
					(string)commandElement.Attribute(XName.Get(@"displayNameTextResource"))),
				_resourceManager.GetDisplayString(
					(string)commandElement.Attribute(XName.Get(@"descriptionTextResource"))));
		}
	}
}