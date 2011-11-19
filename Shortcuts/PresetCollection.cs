using System;
using System.Collections;
using System.Xml;

namespace Rsdn.Shortcuts
{
	/// <summary>
	/// Summary description for PresetCollection.
	/// </summary>
	public class PresetCollection : IEnumerable
	{
		private readonly Hashtable presets = new Hashtable();

		internal PresetCollection()
		{}

		public Preset this[string name]
		{
			get
			{
				if (!presets.ContainsKey(name))
					throw new ArgumentException("Invalid preset name", "name");

				return (Preset)presets[name];
			}
		}

		#region IEnumerable Members
		public IEnumerator GetEnumerator()
		{
			return presets.GetEnumerator();
		}
		#endregion

		internal void AddPreset(Preset p)
		{
			//presets.Add(p.Name,p);
			presets[p.Name] = p;
		}

		internal void RemovePreset(string name)
		{
			presets.Remove(name);
		}
	}

	public class Preset
	{
		private readonly string name;
		private readonly XmlNode nodes;

		internal Preset(string nm, XmlNode nds)
		{
			name = nm;
			nodes = nds;
		}

		public string Name
		{
			get { return name; }
		}


		public XmlNode Nodes
		{
			get { return nodes; }
		}

		public override string ToString()
		{
			return string.IsNullOrEmpty(Name)
				? Name
				: "[empty]";
		}
	}
}