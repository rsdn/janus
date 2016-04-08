using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Rsdn.Janus
{
	public class KeySchema : SchemaNamedElement, IEquatable<KeySchema>
	{
		private const LinkRule _defaultDeleteRule = LinkRule.None;

		private const LinkRule _defaultUpdateRule = LinkRule.None;
		private LinkRule _deleteRule = _defaultDeleteRule;
		private LinkRule _updateRule = _defaultUpdateRule;

		[XmlAttribute("columns")]
		public string Columns { get; set; }

		[XmlAttribute("delete-rule")]
		[DefaultValue(_defaultDeleteRule)]
		public LinkRule DeleteRule
		{
			get { return _deleteRule; }
			set { _deleteRule = value; }
		}

		[XmlAttribute("update-rule")]
		public LinkRule UpdateRule
		{
			get { return _updateRule; }
			set { _updateRule = value; }
		}

		[XmlAttribute("key-type")]
		public ConstraintType KeyType { get; set; }

		[XmlAttribute("rel-table")]
		[DefaultValue(null)]
		public string RelTable { get; set; }

		[XmlAttribute("rel-columns")]
		[DefaultValue(null)]
		public string RelColumns { get; set; }

		[XmlAttribute("clustered")]
		[DefaultValue(null)]
		public bool Clustered { get; set; }

		[XmlAttribute("source")]
		[DefaultValue(null)]
		public string Source { get; set; }

		#region Equals & GetHashCode
		public bool Equals(KeySchema dbKey)
		{
			if (dbKey == null)
				return false;
			if (!Equals(_deleteRule, dbKey._deleteRule))
				return false;
			if (!Equals(_updateRule, dbKey._updateRule))
				return false;
			if (!Equals(Columns, dbKey.Columns))
				return false;
			if (!Equals(KeyType, dbKey.KeyType))
				return false;
			if (!Equals(RelTable, dbKey.RelTable))
				return false;
			if (!Equals(RelColumns, dbKey.RelColumns))
				return false;
			if (!Equals(Clustered, dbKey.Clustered))
				return false;
			if (!Equals(Source, dbKey.Source))
				return false;
			return true;
		}

		public static bool operator !=(KeySchema dbKey1, KeySchema dbKey2)
		{
			return !Equals(dbKey1, dbKey2);
		}

		public static bool operator ==(KeySchema dbKey1, KeySchema dbKey2)
		{
			return Equals(dbKey1, dbKey2);
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj) || Equals(obj as KeySchema);
		}

		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyMemberInGetHashCode
			var result = Columns?.GetHashCode() ?? 0;
			result = 29*result + KeyType.GetHashCode();
			result = 29*result + (RelTable?.GetHashCode() ?? 0);
			result = 29*result + (RelColumns?.GetHashCode() ?? 0);
			result = 29*result + Clustered.GetHashCode();
			result = 29*result + (Source?.GetHashCode() ?? 0);
			// ReSharper restore NonReadonlyMemberInGetHashCode
			return result;
		}
		#endregion

		#region Methods
		public bool EqualsType(KeySchema key)
		{
			if (key == null)
				return false;

			return (
					KeyType == key.KeyType && Columns == key.Columns &&
					DeleteRule == key.DeleteRule && UpdateRule == key.UpdateRule &&
					RelTable == key.RelTable && RelColumns == key.RelColumns);
		}
		#endregion
	}
}