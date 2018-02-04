namespace Janus.Db.DataContext {
	using System.Data.Common;
	using System.Data.Entity;

	public partial class JanusContext : DbContext {
		public JanusContext(string connectionString)
			: base(connectionString) { }

		public JanusContext(DbConnection existingConnection)
			: base(existingConnection, true) { }

		public virtual DbSet<DownloadTopics> DownloadTopics { get; set; }
		public virtual DbSet<Favorite> Favorites { get; set; }
		public virtual DbSet<FavoritesFolders> FavoritesFolders { get; set; }
		public virtual DbSet<MarksOutbox> MarksOutbox { get; set; }
		public virtual DbSet<Message> Messages { get; set; }
		public virtual DbSet<MessagesOutbox> MessagesOutbox { get; set; }
		public virtual DbSet<Moderatorial> Moderatorials { get; set; }
		public virtual DbSet<RateOutbox> RateOutbox { get; set; }
		public virtual DbSet<Rating> Ratings { get; set; }
		public virtual DbSet<ServerForums> ServerForums { get; set; }
		public virtual DbSet<SubscribedForums> SubscribedForums { get; set; }
		public virtual DbSet<Tag> Tags { get; set; }
		public virtual DbSet<TopicInfo> TopicInfo { get; set; }
		public virtual DbSet<User> Users { get; set; }
		public virtual DbSet<Var> Vars { get; set; }
		public virtual DbSet<Violation> Violations { get; set; }

		protected override void OnModelCreating(DbModelBuilder modelBuilder) {

			modelBuilder.Entity<Message>()
				.HasMany(e => e.Tags)
				.WithMany(e => e.Messages)
				.Map(m => m.ToTable("message_tags").MapLeftKey("message_id"));
		}
	}
}
