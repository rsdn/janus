using System;
using System.ComponentModel;

using WeifenLuo.WinFormsUI.Docking;

namespace Rsdn.Janus.Framework
{
	/// <summary>
	/// Summary description for JanusDockPane.
	/// </summary>
	public partial class JanusDockPane : DockContent, IComparable
	{
		private const int _defaultPriority = 0;

		public JanusDockPane()
		{
			InitializeComponent();
			CustomInitializeComponent();
		}

		private void CustomInitializeComponent()
		{
			DockAreas = ((((((DockAreas.Float | DockAreas.DockLeft)
			                 | DockAreas.DockRight)
			                | DockAreas.DockTop)
			               | DockAreas.DockBottom)));
		}

		/// <summary>
		/// Приоритет добавления панели в коллекцию.
		/// </summary>
		[DefaultValue(_defaultPriority)]
		public int Priority { get; set; }

		#region IComparable Members
		public int CompareTo(object obj)
		{
			return Priority - ((JanusDockPane)obj).Priority;
		}
		#endregion
		}
}