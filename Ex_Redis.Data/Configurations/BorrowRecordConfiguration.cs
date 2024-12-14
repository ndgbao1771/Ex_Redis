using Ex_Redis.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ex_Redis.Data.Configurations
{
	public class BorrowRecordConfiguration : IEntityTypeConfiguration<BorrowRecord>
	{
		public void Configure(EntityTypeBuilder<BorrowRecord> builder)
		{
			builder.ToTable("BorrowRecords");
			builder.HasKey(x => x.Id);

			// Relationships
			builder.HasOne(x => x.User)
				   .WithMany(x => x.BorrowRecords)
				   .HasForeignKey(x => x.UserId)
				   .OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(x => x.Book)
				   .WithMany(x => x.BorrowRecords)
				   .HasForeignKey(x => x.BookId)
				   .OnDelete(DeleteBehavior.Restrict);
		}
	}
}
