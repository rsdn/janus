using System;
using System.Collections.Generic;
using System.Text;
using Peak.TreeGrid.Properties;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Peak.TreeGrid
{
	/// <summary>
	/// Visual info, given to understand which style of Cell must be given
	/// </summary>
	[Serializable]
	public class VisualInfo
	{
		/// <summary>
		/// Indicates if cell have alternate style
		/// </summary>
		[XmlAttribute]
		public bool IsAlternateRow { get; internal set; }

		/// <summary>
		/// Indicates if cell is selected
		/// </summary>
		[XmlAttribute]
		public bool IsSelected { get; internal set; }

		/// <summary>
		/// Indicates if cell is current (focused) cell
		/// </summary>
		[XmlAttribute]
		public bool IsFocused { get; internal set; }

		/// <summary>
		/// Indicates if control is active (focused)
		/// </summary>
		[XmlAttribute]
		public bool IsControlActive { get; internal set; }

		/// <summary>
		/// Indicates if control is enabled
		/// </summary>
		[XmlAttribute]
		public bool IsControlEnabled { get; internal set; }

		/// <summary>
		/// Constructor
		/// </summary>
		/// Initializes all properties values.
		/// <param name="IsAlternateRow">IsAlternateRow property value</param>
		/// <param name="IsSelected">IsSelected property value</param>
		/// <param name="IsFocused">IsFocused property value</param>
		/// <param name="IsControlActive">IsControlActive property value</param>
		/// <param name="IsControlEnabled">IsControlEnabled property value</param>
		public VisualInfo(bool IsAlternateRow, bool IsSelected, bool IsFocused,
			bool IsControlActive, bool IsControlEnabled)
		{
			this.IsAlternateRow = IsAlternateRow;
			this.IsSelected = IsSelected;
			this.IsFocused = IsFocused;
			this.IsControlActive = IsControlActive;
			this.IsControlEnabled = IsControlEnabled;
		}

		#region Small optimization to convert to string value not
		[DebuggerStepThrough]
		static int FromBoolean(bool value)
		{
			return (value ? 1 : 0);
		}
		#endregion

		#region Standard Object methods, ToString(), Equals(), GetHashCode()
		/// <summary>
		/// Calcs hash code
		/// </summary>
		/// <returns>Hash code of object</returns>
		public override int GetHashCode()
		{
			return (FromBoolean(IsAlternateRow) << 4) + (FromBoolean(IsSelected) << 3) +
				(FromBoolean(IsFocused) << 2) + (FromBoolean(IsControlActive) << 1) +
				(FromBoolean(IsControlEnabled));
		}

		/// <summary>
		/// Checks if object is equal to given
		/// </summary>
		/// <param name="obj">Object equality to which must be checked</param>
		/// <returns>true if objects are equal</returns>
		public override bool Equals(object obj)
		{
			return ((obj == null) ? false : obj.GetHashCode() == GetHashCode());
		}

		/// <summary>
		/// Converts object to string.
		/// </summary>
		/// <returns>String representation of object</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			IfAppend(sb, IsAlternateRow, Resources.Alternate);
			IfAppend(sb, IsSelected, Resources.Selected);
			IfAppend(sb, IsFocused, Resources.Focused);
			IfAppend(sb, IsControlActive, Resources.ControlActive);
			IfAppend(sb, IsControlEnabled, Resources.ControlEnabled);
			return sb.ToString();
		} 
		#endregion

		#region Utility function
		static StringBuilder IfAppend(StringBuilder sb, bool isFlagActive, string flagString)
		{
			if (isFlagActive)
			{
				if (sb.Length > 0)
					sb.Append(", ");
				sb.Append(flagString);
			}
			return sb;
		}
		#endregion
	}
}
