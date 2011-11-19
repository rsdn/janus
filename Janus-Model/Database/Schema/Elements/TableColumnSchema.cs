using System.ComponentModel;
using System.Xml.Serialization;

namespace Rsdn.Janus
{
	public class TableColumnSchema : SchemaNamedElement
	{
		private const bool _defaultAutoIncrement = false;
		private const int _defaultDecimalPrecision = 0;
		private const int _defaultDecimalScale = 0;
		private const int _defaultIncrement = 0;
		private const int _defaultSeed = 0;
		private const ColumnType _defaultType = ColumnType.Unknown;
		private ColumnType _type = _defaultType;

		[XmlAttribute("type")]
		[DefaultValue(_defaultType)]
		public ColumnType Type
		{
			get { return _type; }
			set { _type = value; }
		}

		[XmlAttribute("auto-increment")]
		[DefaultValue(_defaultAutoIncrement)]
		public bool AutoIncrement { get; set; }

		[XmlAttribute("seed")]
		[DefaultValue(_defaultSeed)]
		public int Seed { get; set; }

		[XmlAttribute("increment")]
		[DefaultValue(_defaultIncrement)]
		public int Increment { get; set; }

		[XmlAttribute("nullable")]
		public bool Nullable { get; set; }

		[XmlAttribute("size")]
		public long Size { get; set; }

		[XmlAttribute("decimal-scale")]
		[DefaultValue(_defaultDecimalScale)]
		public int DecimalScale { get; set; }

		[XmlAttribute("decimal-precision")]
		[DefaultValue(_defaultDecimalPrecision)]
		public int DecimalPrecision { get; set; }

		[XmlAttribute("default-value")]
		public string DefaultValue { get; set; }

		#region Methods
		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;
			if (!(obj is TableColumnSchema))
				return false;

			var column = (TableColumnSchema)obj;
			if (!
				(column.Name == Name &&
				//column.ComputedBy == ComputedBy &&
				//column.DecimalPrecision == DecimalPrecision &&
				//column.DecimalScale == DecimalScale && 
				column.DefaultValue == DefaultValue &&
				column.AutoIncrement == AutoIncrement && column.Increment == Increment && column.Seed == Seed &&
				column.Nullable == Nullable && column.Type == Type))
				return false;

			switch (Type)
			{
				case ColumnType.Character:
				case ColumnType.CharacterVaring:
				case ColumnType.NCharacter:
				case ColumnType.NCharacterVaring:
				case ColumnType.Binary:
				case ColumnType.BinaryVaring:
					if (column.Size != Size)
						return false;
					break;
				case ColumnType.Float:
				case ColumnType.Real:
				case ColumnType.DoublePrecision:
					if (column.DecimalPrecision != DecimalPrecision)
						return false;
					break;
				case ColumnType.Decimal:
				case ColumnType.Numeric:
					if (column.DecimalPrecision != DecimalPrecision && column.DecimalScale != DecimalScale)
						return false;
					break;
			}

			return true;
		}

		public override int GetHashCode()
		{
			return
				(string.IsNullOrEmpty(Name) ? 0 : Name.GetHashCode()) ^
				(string.IsNullOrEmpty(DefaultValue) ? 0 : DefaultValue.GetHashCode()) ^
				(AutoIncrement ? 1 : 0) ^
				Increment ^
				Seed ^
				(Nullable ? 2 : 0) ^
				(int)Type;
		}
		#endregion
	}
}