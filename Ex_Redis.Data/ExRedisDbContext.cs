using Ex_Redis.Data.Configurations;
using Ex_Redis.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Ex_Redis.Data
{
	public class ExRedisDbContext : DbContext
	{
		public ExRedisDbContext(DbContextOptions<ExRedisDbContext> options) : base(options)
		{
		}

		public DbSet<Book> Books { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<BorrowRecord> BorrowRecords { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Apply configurations
			modelBuilder.ApplyConfiguration(new BookConfiguration());
			modelBuilder.ApplyConfiguration(new UserConfiguration());
			modelBuilder.ApplyConfiguration(new BorrowRecordConfiguration());

			base.OnModelCreating(modelBuilder);
		}
	}
}
