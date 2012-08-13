using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

using JetBrains.Annotations;

using Rsdn.SmartApp;

namespace Rsdn.Janus
{
	internal static class XmlMenuLoader
	{
		private const string _menuDescriptorSchemaResource =
			"Rsdn.Janus.CoreServices.Menu.JanusMenuDescriptor.xsd";
		private const string _menuDescriptorSchemaUri =
			"http://rsdn.ru/projects/Janus/JanusMenuDescriptor.xsd";

		private static readonly XmlReaderSettings _readerSettings;

		static XmlMenuLoader()
		{
			_readerSettings = new XmlReaderSettings { ValidationType = ValidationType.Schema };
			_readerSettings.Schemas.Add(CommonTypesSchema.Schema);
			_readerSettings.Schemas.Add(
				XmlSchema.Read(
					typeof(XmlMenuLoader)
						.Assembly
						.GetRequiredResourceStream(_menuDescriptorSchemaResource),
					null));

		}

		public static IMenuRoot LoadMenu(
			[NotNull] IServiceProvider serviceProvider,
			[NotNull] Stream xmlMenuStream,
			[NotNull] Func<string, string> resourceStringGetter)
		{
			if (serviceProvider == null)
				throw new ArgumentNullException("serviceProvider");
			if (xmlMenuStream == null)
				throw new ArgumentNullException("xmlMenuStream");
			if (resourceStringGetter == null)
				throw new ArgumentNullException("resourceStringGetter");

			return LoadMenuRoot(
				serviceProvider,
				XElement.Load(XmlReader.Create(xmlMenuStream, _readerSettings)),
				resourceStringGetter);
		}

		private static IMenuRoot LoadMenuRoot(
			IServiceProvider serviceProvider,
			XElement rootElement,
			Func<string, string> resourceStringGetter)
		{
			if (rootElement.Name == XName.Get("root", _menuDescriptorSchemaUri))
				return new MenuRoot(
					(string)rootElement.Attribute(XName.Get("name")),
					rootElement
						.Elements()
						.Select(sub => LoadMenuGroup(serviceProvider, sub, resourceStringGetter)));

			throw new ApplicationException("Неизвестный элемент '{0}'.".FormatStr(rootElement));
		}

		private static IMenuGroup LoadMenuGroup(
			IServiceProvider serviceProvider,
			XElement groupElement,
			Func<string, string> resourceStringGetter)
		{
			return new MenuGroup(
				(string)groupElement.Attribute(XName.Get("name")),
				groupElement
					.Elements()
					.Select(itemElement => LoadMenuItem(serviceProvider, itemElement, resourceStringGetter)),
				(int)groupElement.Attribute(XName.Get("orderIndex")));
		}

		private static IMenuItem LoadMenuItem(
			IServiceProvider serviceProvider,
			XElement itemElement,
			Func<string, string> resourceStringGetter)
		{
			if (itemElement.Name == XName.Get("menu", _menuDescriptorSchemaUri))
				return
					new Menu(
						(string)itemElement.Attribute(XName.Get("name")),
						itemElement
							.Elements()
							.Select(sub => LoadMenuGroup(serviceProvider, sub, resourceStringGetter)),
						resourceStringGetter((string)itemElement.Attribute(XName.Get("textResource"))),
						(string)itemElement.Attribute(XName.Get("image")),
						(string)itemElement.Attribute(XName.Get("description")),
						(MenuItemDisplayStyle)Enum.Parse(
							typeof(MenuItemDisplayStyle),
							(string)itemElement.Attribute(XName.Get("displayStyle"))),
						(int)itemElement.Attribute(XName.Get("orderIndex")));

			if (itemElement.Name == XName.Get("menuCommand", _menuDescriptorSchemaUri))
			{
				var commandName = (string)itemElement.Attribute(XName.Get("command"));

				string text;
				string description;
				GetMenuCommandTextAndDescription(
					serviceProvider,
					commandName,
					(string)itemElement.Attribute(XName.Get("textResource")),
					(string)itemElement.Attribute(XName.Get("description")),
					resourceStringGetter,
					out text,
					out description);

				return new MenuCommand(
					commandName,
					LoadCommandParameters(itemElement),
					text,
					(string)itemElement.Attribute(XName.Get("image")),
					description,
					(MenuItemDisplayStyle)Enum.Parse(
						typeof(MenuItemDisplayStyle),
						(string)itemElement.Attribute(XName.Get("displayStyle"))),
					(int)itemElement.Attribute(XName.Get("orderIndex")));
			}

			if (itemElement.Name == XName.Get("splitButton", _menuDescriptorSchemaUri))
			{
				var commandName = (string)itemElement.Attribute(XName.Get("command"));
				string text;
				string description;
				GetMenuCommandTextAndDescription(
					serviceProvider,
					commandName,
					(string)itemElement.Attribute(XName.Get("textResource")),
					(string)itemElement.Attribute(XName.Get("description")),
					resourceStringGetter,
					out text,
					out description);

				return new MenuSplitButton(
					(string)itemElement.Attribute(XName.Get("name")),
					itemElement
						.Elements(XName.Get("group", _menuDescriptorSchemaUri))
						.Select(sub => LoadMenuGroup(serviceProvider, sub, resourceStringGetter)),
					commandName,
					LoadCommandParameters(itemElement),
					text,
					(string)itemElement.Attribute(XName.Get("image")),
					description,
					(MenuItemDisplayStyle)Enum.Parse(
						typeof(MenuItemDisplayStyle),
						(string)itemElement.Attribute(XName.Get("displayStyle"))),
					(int)itemElement.Attribute(XName.Get("orderIndex")));
			}

			if (itemElement.Name == XName.Get("menuCheckCommand", _menuDescriptorSchemaUri))
			{
				var checkCommandElement = itemElement.RequiredElement(
					XName.Get("checkCommand", _menuDescriptorSchemaUri));
				var checkCommandName = (string)checkCommandElement.Attribute(XName.Get("name"));
				var checkCommandParameters = LoadCommandParameters(checkCommandElement);

				var uncheckCommandElement = itemElement.RequiredElement(
					XName.Get("uncheckCommand", _menuDescriptorSchemaUri));
				var uncheckCommandName = (string)uncheckCommandElement.Attribute(XName.Get("name"));
				var uncheckCommandParameters = LoadCommandParameters(uncheckCommandElement);

				return new MenuCheckCommand(
					(string)itemElement.Attribute(XName.Get("checkStateName")),
					checkCommandName,
					checkCommandParameters,
					uncheckCommandName,
					uncheckCommandParameters,
					resourceStringGetter((string)itemElement.Attribute(XName.Get("textResource"))),
					(string)itemElement.Attribute(XName.Get("image")),
					(string)itemElement.Attribute(XName.Get("description")),
					(MenuItemDisplayStyle)Enum.Parse(
						typeof(MenuItemDisplayStyle),
						(string)itemElement.Attribute(XName.Get("displayStyle"))),
					(int)itemElement.Attribute(XName.Get("orderIndex")));
			}

			throw new ApplicationException("Неизвестный элемент '{0}'.".FormatStr(itemElement));
		}

		private static void GetMenuCommandTextAndDescription(
			IServiceProvider serviceProvider,
			string commandName,
			string textResource,
			string descriptionResource,
			Func<string, string> resourceStringGetter,
			out string text,
			out string description)
		{
			var command = serviceProvider.GetRequiredService<ICommandService>().GetCommandInfo(commandName);

			text = textResource != null
				? resourceStringGetter(textResource)
				: command.DisplayName;

			if (command.Type == CommandType.RequiresInteraction)
				text += "...";

			if (descriptionResource != null)
				description = resourceStringGetter(descriptionResource);
			else if (textResource == null)
				description = command.Description;
			else
				description = null;
		}

		private static IDictionary<string, object> LoadCommandParameters(XContainer xe)
		{
			return
				xe
					.Elements(XName.Get("parameter", _menuDescriptorSchemaUri))
					.ToDictionary(
						parameter => (string)parameter.Attribute(XName.Get("name")),
						parameter => (object)parameter.Value,
						StringComparer.OrdinalIgnoreCase)
					.AsReadOnly();
		}
	}
}