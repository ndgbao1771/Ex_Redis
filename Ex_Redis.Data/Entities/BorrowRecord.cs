namespace Ex_Redis.Data.Entities
{
	public class BorrowRecord
	{
		public int Id { get; set; }
		public int UserId { get; set; }
		public int BookId { get; set; }
		public DateTime? BorrowDate { get; set; }
		public DateTime DueDate { get; set; }
		public DateTime? ReturnDate { get; set; }
		public bool IsReturned { get; set; } = false;
		public virtual User? User { get; set; }
		public virtual Book? Book { get; set; }
	}
}
