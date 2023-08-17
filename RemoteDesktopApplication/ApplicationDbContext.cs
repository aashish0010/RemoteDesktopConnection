using Microsoft.EntityFrameworkCore;

namespace RemoteDesktopApplication
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<Servermanager> servermanager { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Servermanager>().Property(x => x.ServerName).HasMaxLength(256);
			modelBuilder.Entity<Servermanager>().Property(x => x.ServerType).HasMaxLength(130);
			modelBuilder.Entity<Servermanager>().Property(x => x.ServerIpAddress).HasMaxLength(150);
			modelBuilder.Entity<Servermanager>().Property(x => x.ServerUsername).HasMaxLength(150);
			modelBuilder.Entity<Servermanager>().Property(x => x.ServerPassword).HasMaxLength(150);
			modelBuilder.Entity<Servermanager>().Property(x => x.ServerDescription).HasMaxLength(500);
			modelBuilder.Entity<Servermanager>().HasKey(x => x.Id);
			base.OnModelCreating(modelBuilder);
		}
	}
	public class Servermanager
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string? ServerName { get; set; }
		public string? ServerType { get; set; }
		public string? ServerIpAddress { get; set; }
		public string? ServerUsername { get; set; }
		public string? ServerPassword { get; set; }
		public string? ServerDescription { get; set; }
	}
	public class ServermanagerRequest
	{
		public string? ServerName { get; set; }
		public string? ServerType { get; set; }
		public string? ServerIpAddress { get; set; }
		public string? ServerUsername { get; set; }
		public string? ServerPassword { get; set; }
		public string? ServerDescription { get; set; }
	}
}
