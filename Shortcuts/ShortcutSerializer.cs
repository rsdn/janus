using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Rsdn.Shortcuts
{
	public class ShortcutSerializer
	{
		private const string _version = "1.0";
		private readonly ShortcutManager _manager;

		public ShortcutSerializer(ShortcutManager manager)
		{
			_manager = manager;
		}

		#region Save
		public bool SaveToStream(Stream stream)
		{
			return SaveCore(stream, _manager.Nodes, false);
		}

		public bool SaveForPreset(Stream stream)
		{
			return SaveCore(stream, _manager.Nodes, true);
		}

		private bool SaveCore(Stream stream,
			ShortcutCollection nodes, bool forPreset)
		{
			var writer = new XmlTextWriter(stream, Encoding.UTF8) {Formatting = Formatting.Indented};

			writer.WriteStartElement("SerializeShortcuts");
			writer.WriteAttributeString("version", _version);

			SerializeNodes(writer, nodes);

			if (!forPreset)
				SerializePresets(writer);

			writer.WriteEndElement();
			writer.Flush();

			return true;
		}

		private static void SerializeNodes(XmlWriter writer, ShortcutCollection nodes)
		{
			foreach (CustomShortcut cs in nodes)
			{
				writer.WriteStartElement("node");
				writer.WriteAttributeString("name", cs.Name);
				writer.WriteAttributeString("type", cs.OwnerType.ToString());

				HashtableToString(writer, cs.HashTable);

				writer.WriteEndElement();

				SerializeNodes(writer, cs.Nodes);
			}
		}

		private void SerializePresets(XmlWriter writer)
		{
			writer.WriteStartElement("presets");

			foreach (DictionaryEntry de in _manager.Presets)
			{
				writer.WriteStartElement("preset");
				writer.WriteAttributeString("name", (string)de.Key);
				//SerializeNodes(writer, ((Preset)de.Value).Nodes);
				writer.WriteRaw(((Preset)de.Value).Nodes.InnerXml);
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

		private static void HashtableToString(XmlWriter writer,
			IEnumerable<KeyValuePair<Shortcut, string>> ht)
		{
			foreach (var de in ht)
			{
				writer.WriteStartElement("entry_hashtable");
				writer.WriteAttributeString("key", de.Key.ToString());
				writer.WriteAttributeString("value", de.Value);
				writer.WriteEndElement();
			}
		}
		#endregion

		#region Load
		public void LoadFromStream(Stream stream)
		{
			var doc = new XmlDocument();
			doc.Load(stream);

			LoadNodes(doc.DocumentElement);
			LoadPresets(doc.DocumentElement);
		}

		public void LoadNodes(XmlNode node)
		{
			LoadNodes(node, _manager.Nodes);
		}

		private static void LoadNodes(XmlNode pnode, ShortcutCollection nodes)
		{
			foreach (XmlNode node in pnode.ChildNodes)
			{
				if (node.Name != "node")
					continue;

				//string name = node.Attributes["name"].Value;
				var typeName = node.Attributes["type"].Value;

				if (node.HasChildNodes)
				{
					var customShortcut = FindNodeType(typeName, nodes);

					if (customShortcut == null)
						continue;

					foreach (XmlNode entry in node.ChildNodes)
					{
						var key = entry.Attributes["key"].Value;
						var value = entry.Attributes["value"].Value;
						var sc = (Shortcut)Enum
							.Parse(typeof (Shortcut), key);

						customShortcut.HashTable[sc] = value;
					}
				}
			}
		}

		private static CustomShortcut FindNodeType(
			string typeName, ShortcutCollection nodes)
		{
			foreach (CustomShortcut cs in nodes)
			{
				if (cs.OwnerType.ToString() == typeName)
					return cs;

				return FindNodeType(typeName, cs.Nodes);
			}

			return null;
		}

		private void LoadPresets(XmlNode nodes)
		{
			foreach (XmlNode node in nodes.ChildNodes)
			{
				if (node.Name != "presets")
					continue;

				LoadPreset(node);
			}
		}

		private void LoadPreset(XmlNode nodes)
		{
			foreach (XmlNode node in nodes.ChildNodes)
			{
				if (node.Name != "preset" || node.ChildNodes.Count == 0)
					continue;

				var p = new Preset(node.Attributes["name"].Value, node);
				//LoadNodes(node,p.Nodes);
				_manager.Presets.AddPreset(p);
			}
		}
		#endregion
	}
}