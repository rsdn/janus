namespace Janus.Db.DataContext {
	using System;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("rating")]
	public partial class Rating {
		public DateTime dte { get; set; }

		public int id { get; set; }

		public int mid { get; set; }

		public short rate { get; set; }

		public short rby { get; set; }

		public int tid { get; set; }

		public int uid { get; set; }
	}
}
