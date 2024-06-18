using System;
using cloud.core.database.Config;
using cloud.core.objects.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace cloud.core.database.DbContexts
{
	public class CloudDbContext:DbContext
	{
        public CloudDbContext(DbContextOptions<CloudDbContext> options)
    : base(options)
        { }
        public virtual DbSet<DbUser> Users { get; set; }
		public virtual DbSet<DbSubscription> Subscriptions { get; set; }
		public virtual DbSet<DbUserFilesData> UserFilesData { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.Entity<DbUser>(entity =>
			{
				entity.HasKey(x => x.Id);
				entity.ToTable("users");
				entity.HasOne(x => x.Data).WithOne(x => x.User);
				entity.HasOne(x => x.Subscription).WithMany(x => x.Users);
			});
			modelBuilder.Entity<DbSubscription>(entity =>
			{
				entity.HasKey(x => x.Id);
				entity.ToTable("subscriptions");
				entity.HasMany(x => x.Users).WithOne(x => x.Subscription);
				entity.HasData(new DbSubscription() { Id = 1, MaximmumSpace = 1000000, Name = "Test subscription" });



            });
			modelBuilder.Entity<DbUserFilesData>(entity =>
			{
				entity.HasKey(x => x.UserId);
				entity.ToTable("user_files_data");
				entity.HasOne(x => x.User).WithOne(x => x.Data);
			});

            
        }
	}
}

