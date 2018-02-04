namespace Janus.Db.DataContext {
	using System.Collections.Generic;
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("tag")]
	public partial class Tag {
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
		public Tag() {
			Messages = new HashSet<Message>();
		}

		public int Id { get; set; }

		[Required]
		[StringLength(1024)]
		[Column("tag_value")]
		public string TagValue { get; set; }

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
		public virtual ICollection<Message> Messages { get; set; }
	}
}
