using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace RemoteDesktopApplication
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

		public DbSet<Servermanager> servermanager { get; set; }
		public DbSet<SiteManager> siteManagers { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<Servermanager>().Property(x => x.ServerName).HasMaxLength(256);
			modelBuilder.Entity<Servermanager>().Property(x => x.ServerType).HasMaxLength(130);
			modelBuilder.Entity<Servermanager>().Property(x => x.ServerIpAddress).HasMaxLength(150);
			modelBuilder.Entity<Servermanager>().Property(x => x.ServerUsername).HasMaxLength(150);
			modelBuilder.Entity<Servermanager>().Property(x => x.ServerPassword).HasMaxLength(150);
			modelBuilder.Entity<Servermanager>().Property(x => x.ServerDescription).HasMaxLength(500);
			modelBuilder.Entity<Servermanager>().HasKey(x => x.Id);



			modelBuilder.Entity<SiteManager>().Property(x => x.SiteName).HasMaxLength(256);
			modelBuilder.Entity<SiteManager>().Property(x => x.SiteType).HasMaxLength(130);
			modelBuilder.Entity<SiteManager>().Property(x => x.SiteRoot).HasMaxLength(150);
			modelBuilder.Entity<SiteManager>().Property(x => x.SiteLink).HasMaxLength(150);
			modelBuilder.Entity<SiteManager>().Property(x => x.UserName).HasMaxLength(150);
			modelBuilder.Entity<SiteManager>().Property(x => x.Password).HasMaxLength(500);
			modelBuilder.Entity<SiteManager>().HasKey(x => x.Id);
			base.OnModelCreating(modelBuilder);

		}
	}
	public class Servermanager
	{
		public Guid Id { get; set; } = Guid.NewGuid();
		public string ServerName { get; set; }
		public string ServerType { get; set; }
		public string ServerHost { get; set; }
		public string ServerIpAddress { get; set; }
		public string ServerUsername { get; set; }
		public string ServerPassword { get; set; }
		public string ServerDescription { get; set; }
		
	}

	public class SiteManager
	{
		public Guid Id { get; set; }= Guid.NewGuid();
		public string SiteName { get; set; }
		public string SiteLink { get; set; }
		public string SiteRoot { get; set; }

		public string SiteType { get; set; }

		public string UserName { get; set; }
		public string Password { get; set; }
		public string AccessCode { get; set; }
		
	}
	public class ServermanagerRequest
	{
		[Required]
		public string ServerName { get; set; }
		[Required]
		public string ServerType { get; set; }
		[Required]
		public string ServerHost { get; set; }
		[Required]
		public string ServerIpAddress { get; set; }
		[Required]
		public bool IsAdServer { get; set; }
		[Required]
		public string ServerUsername { get; set; }
		[Required]
		public string ServerPassword { get; set; }
		public string ServerDescription { get; set; }
	}
}
