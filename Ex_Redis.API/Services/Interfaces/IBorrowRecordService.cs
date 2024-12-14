using Ex_Redis.Data.Entities;

namespace Ex_Redis.API.Services.Interfaces
{
	public interface IBorrowRecordService
	{
		Task<IEnumerable<BorrowRecord>> GetAllAsync();
		Task<BorrowRecord> GetByIdAsync(int id);
		Task<IEnumerable<BorrowRecord>> GetByUserIdAsync(int userId);
		Task<IEnumerable<BorrowRecord>> GetByBookIdAsync(int bookId);
		Task<BorrowRecord> CreateAsync(BorrowRecord borrowRecord);
		Task<BorrowRecord> UpdateAsync(BorrowRecord borrowRecord);
		Task<bool> DeleteAsync(int id);
	}
}
