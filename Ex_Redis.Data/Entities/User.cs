namespace Ex_Redis.Data.Entities
{
	public class User
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public string? Email { get; set; }
		public string? Phone { get; set; }
		public string? Address { get; set; }
		public DateTime RegisterDate { get; set; } = DateTime.Now;

		public virtual ICollection<BorrowRecord>? BorrowRecords { get; set; }
	}
}
