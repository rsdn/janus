namespace Janus.Db.DataContext {
	using System.ComponentModel.DataAnnotations;
	using System.ComponentModel.DataAnnotations.Schema;

	[Table("download_topics")]
	public partial class DownloadTopics {
		public int id { get; set; }

		[StringLength(32)]
		public string source { get; set; }

		public int messageid { get; set; }

		[StringLength(128)]
		public string hint { get; set; }
	}
}
