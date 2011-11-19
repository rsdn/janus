using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Rsdn.Janus
{
	public class DbsmKey : DbsmNamedElement, IEquatable<DbsmKey>
	{
		[XmlAttribute("columns")]
		public string Columns { get; set; }

		private const DbsmRule _defaultDeleteRule = DbsmRule.None;
		private DbsmRule _deleteRule = _defaultDeleteRule;

		[XmlAttribute("delete-rule")]
		[DefaultValue(_defaultDeleteRule)]
		public DbsmRule DeleteRule
		{
			get { return _deleteRule; }
			set { _deleteRule = value; }
		}

		private const DbsmRule _defaultUpdateRule = DbsmRule.None;
		private DbsmRule _updateRule = _defaultUpdateRule;

		[XmlAttribute("update-rule")]
		public DbsmRule UpdateRule
		{
			get { return _updateRule; }
			set { _updateRule = value; }
		}

		[XmlAttribute("key-type")]
		public DbsmConstraintType KeyType { get; set; }

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
		public static bool operator !=(DbsmKey dbsmKey1, DbsmKey dbsmKey2)
		{
			return !Equals(dbsmKey1, dbsmKey2);
		}

		public static bool operator ==(DbsmKey dbsmKey1, DbsmKey dbsmKey2)
		{
			return Equals(dbsmKey1, dbsmKey2);
		}

		public bool Equals(DbsmKey dbsmKey)
		{
			if (dbsmKey == null)
				return false;
			if (!Equals(_deleteRule, dbsmKey._deleteRule))
				return false;
			if (!Equals(_updateRule, dbsmKey._updateRule))
				return false;
			if (!Equals(Columns, dbsmKey.Columns))
				return false;
			if (!Equals(KeyType, dbsmKey.KeyType))
				return false;
			if (!Equals(RelTable, dbsmKey.RelTable))
				return false;
			if (!Equals(RelColumns, dbsmKey.RelColumns))
				return false;
			if (!Equals(Clustered, dbsmKey.Clustered))
				return false;
			if (!Equals(Source, dbsmKey.Source))
				return false;
			return true;
		}

		public override bool Equals(object obj)
		{
			if (ReferenceEquals(this, obj))
				return true;
			return Equals(obj as DbsmKey);
		}

		public override int GetHashCode()
		{
			int result = _deleteRule.GetHashCode();
			result = 29*result + _updateRule.GetHashCode();
			result = 29*result + (Columns != null ? Columns.GetHashCode() : 0);
			result = 29*result + KeyType.GetHashCode();
			result = 29*result + (RelTable != null ? RelTable.GetHashCode() : 0);
			result = 29*result + (RelColumns != null ? RelColumns.GetHashCode() : 0);
			result = 29*result + Clustered.GetHashCode();
			result = 29*result + (Source != null ? Source.GetHashCode() : 0);
			return result;
		}
		#endregion

		#region Methods
		public bool EqualsType(DbsmKey key)
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
