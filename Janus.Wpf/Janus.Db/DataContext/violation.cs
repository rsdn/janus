namespace Janus.Db.DataContext {
	using System;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("violation")]
	public partial class Violation {
		[Key]
		[Column(Order = 0)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int MessageId { get; set; }

		[Key]
		[Column(Order = 1, TypeName = "ntext")]
		public string Reason { get; set; }

		[Key]
		[Column(Order = 2)]
		[DatabaseGenerated(DatabaseGeneratedOption.None)]
		public int PenaltyType { get; set; }

		[Key]
		[Column(Order = 3)]
		public DateTime Create { get; set; }
	}
}
