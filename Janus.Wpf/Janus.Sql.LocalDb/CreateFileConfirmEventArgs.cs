using System.ComponentModel;

namespace Janus.Sql.LocalDb {
	public class CreateFileConfirmEventArgs : CancelEventArgs {
		public CreateFileConfirmEventArgs(string dbFilePath) : base(true) {
			DbFilePath = dbFilePath;
		}

		public string DbFilePath { get; set; }
	}
}