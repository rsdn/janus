namespace Janus.Model.Perist {
	public interface IConnectionSettings {
		string ConnectionString { get; set; }
		bool IsValid { get; }
		bool Build();
	}
}