using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Rsdn.Janus
{
	public class IndexSchema : SchemaNamedElement, IEquatable<IndexSchema>
	{
		private const bool _defaultIsActive = true;
		private const bool _defaultPrimaryKey = false;
		private const SortOrder _defaultSortOrder = SortOrder.Ascending;
		private bool _isActive = _defaultIsActive;
		private SortOrder _sortOrder = _defaultSortOrder;

		[XmlAttribute("columns")]
		public string Columns { get; set; }

		[XmlAttribute("clustered")]
		public bool Clustered { get; set; }

		[XmlAttribute("allow-nulls")]
		public IndexNullAllowance NullAllowances { get; set; }

		[XmlAttribute("primary-key")]
		[DefaultValue(_defaultPrimaryKey)]
		public bool PrimaryKey { get; set; }

		[XmlAttribute("unique")]
		public bool Unique { get; set; }

		[XmlAttribute("sort")]
		[DefaultValue(_defaultSortOrder)]
		public SortOrder Sort
		{
			get { return _sortOrder; }
			set { _sortOrder = value; }
		}

		[XmlAttribute("is-active")]
		[DefaultValue(_defaultIsActive)]
		public bool IsActive
		{
			get { return _isActive; }
			set { _isActive = value; }
		}

		#region Equals & GetHashCode
		public bool Equals(IndexSchema dbIndex)
		{
			if (dbIndex == null)
				return false;
			if (!Equals(_sortOrder, dbIndex._sortOrder))
				return false;
			if (!Equals(_isActive, dbIndex._isActive))
				return false;
			if (!Equals(Columns, dbIndex.Columns))
				return false;
			if (!Equals(Clustered, dbIndex.Clustered))
				return false;
			if (!Equals(NullAllowances, dbIndex.NullAllowances))
				return false;
			return Equals(PrimaryKey, dbIndex.PrimaryKey) && Equals(Unique, dbIndex.Unique);
		}

		public static bool operator !=(IndexSchema dbIndex1, IndexSchema dbIndex2)
		{
			return !Equals(dbIndex1, dbIndex2);
		}

		public static bool operator ==(IndexSchema dbIndex1, IndexSchema dbIndex2)
		{
			return Equals(dbIndex1, dbIndex2);
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj) || Equals(obj as IndexSchema);
		}

		public override int GetHashCode()
		{
			// ReSharper disable NonReadonlyMemberInGetHashCode
			var result = Columns?.GetHashCode() ?? 0;
			result = 29*result + Clustered.GetHashCode();
			result = 29*result + NullAllowances.GetHashCode();
			result = 29*result + PrimaryKey.GetHashCode();
			result = 29*result + Unique.GetHashCode();
			// ReSharper restore NonReadonlyMemberInGetHashCode
			return result;
		}
		#endregion
	}
}