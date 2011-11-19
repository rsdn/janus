using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace Rsdn.Janus
{

	public class DbsmIndex : DbsmNamedElement, IEquatable<DbsmIndex>
	{
		[XmlAttribute("columns")]
		public string Columns { get; set; }

		[XmlAttribute("clustered")]
		public bool Clustered { get; set; }

		[XmlAttribute("allow-nulls")]
		public DbsmAllowNulls AllowNulls { get; set; }

		private const bool _defaultPrimaryKey = false;

		[XmlAttribute("primary-key")]
		[DefaultValue(_defaultPrimaryKey)]
		public bool PrimaryKey { get; set; }

		[XmlAttribute("unique")]
		public bool Unique { get; set; }

		private const DbsmSortOrder _defaultSortOrder = DbsmSortOrder.SortAscending;
		private DbsmSortOrder _sortOrder = _defaultSortOrder;

		[XmlAttribute("sort")]
		[DefaultValue(_defaultSortOrder)]
		public DbsmSortOrder Sort
		{
			get { return _sortOrder; }
			set { _sortOrder = value; }
		}

		private const bool _defaultIsActive = true;
		private bool _isActive = _defaultIsActive;

		[XmlAttribute("is-active")]
		[DefaultValue(_defaultIsActive)]
		public bool IsActive
		{
			get { return _isActive; }
			set { _isActive = value; }
		}

		#region Equals & GetHashCode
		public static bool operator !=(DbsmIndex dbsmIndex1, DbsmIndex dbsmIndex2)
		{
			return !Equals(dbsmIndex1, dbsmIndex2);
		}

		public static bool operator ==(DbsmIndex dbsmIndex1, DbsmIndex dbsmIndex2)
		{
			return Equals(dbsmIndex1, dbsmIndex2);
		}

		public bool Equals(DbsmIndex dbsmIndex)
		{
			if (dbsmIndex == null)
				return false;
			if (!Equals(_sortOrder, dbsmIndex._sortOrder))
				return false;
			if (!Equals(_isActive, dbsmIndex._isActive))
				return false;
			if (!Equals(Columns, dbsmIndex.Columns))
				return false;
			if (!Equals(Clustered, dbsmIndex.Clustered))
				return false;
			if (!Equals(AllowNulls, dbsmIndex.AllowNulls))
				return false;
			return Equals(PrimaryKey, dbsmIndex.PrimaryKey) && Equals(Unique, dbsmIndex.Unique);
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj) || Equals(obj as DbsmIndex);
		}

		public override int GetHashCode()
		{
			var result = _sortOrder.GetHashCode();
			result = 29*result + _isActive.GetHashCode();
			result = 29*result + (Columns != null ? Columns.GetHashCode() : 0);
			result = 29*result + Clustered.GetHashCode();
			result = 29*result + AllowNulls.GetHashCode();
			result = 29*result + PrimaryKey.GetHashCode();
			result = 29*result + Unique.GetHashCode();
			return result;
		}
		#endregion
	}
}
