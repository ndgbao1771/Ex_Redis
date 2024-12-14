using Ex_Redis.API.Redises.Interfaces;
using Ex_Redis.API.Services.Interfaces;
using Ex_Redis.Data.Entities;
using Ex_Redis.Data;
using Microsoft.EntityFrameworkCore;

namespace Ex_Redis.API.Services.Implements
{
	public class BorrowRecordService : IBorrowRecordService
	{
		private readonly ExRedisDbContext _context;
		private readonly ICacheService _cacheService;
		private const string CacheKey = "borrowrecords";

		public BorrowRecordService(ExRedisDbContext context, ICacheService cacheService)
		{
			_context = context;
			_cacheService = cacheService;
		}

		public async Task<IEnumerable<BorrowRecord>> GetAllAsync()
		{
			var cached = await _cacheService.GetAsync<IEnumerable<BorrowRecord>>(CacheKey);
			if (cached != null) return cached;

			var records = await _context.BorrowRecords
				.Include(b => b.Book)
				.Include(b => b.User)
				.ToListAsync();
			await _cacheService.SetAsync(CacheKey, records, TimeSpan.FromMinutes(10));
			return records;
		}

		public async Task<BorrowRecord> GetByIdAsync(int id)
		{
			var cacheKey = $"{CacheKey}:{id}";
			var cached = await _cacheService.GetAsync<BorrowRecord>(cacheKey);
			if (cached != null) return cached;

			var record = await _context.BorrowRecords
				.Include(b => b.Book)
				.Include(b => b.User)
				.FirstOrDefaultAsync(b => b.Id == id);
			if (record != null)
				await _cacheService.SetAsync(cacheKey, record, TimeSpan.FromMinutes(10));
			return record;
		}

		public async Task<IEnumerable<BorrowRecord>> GetByUserIdAsync(int userId)
		{
			var cacheKey = $"{CacheKey}:user:{userId}";
			var cached = await _cacheService.GetAsync<IEnumerable<BorrowRecord>>(cacheKey);
			if (cached != null) return cached;

			var records = await _context.BorrowRecords
				.Include(b => b.Book)
				.Include(b => b.User)
				.Where(b => b.UserId == userId)
				.ToListAsync();
			await _cacheService.SetAsync(cacheKey, records, TimeSpan.FromMinutes(10));
			return records;
		}

		public async Task<IEnumerable<BorrowRecord>> GetByBookIdAsync(int bookId)
		{
			var cacheKey = $"{CacheKey}:book:{bookId}";
			var cached = await _cacheService.GetAsync<IEnumerable<BorrowRecord>>(cacheKey);
			if (cached != null) return cached;

			var records = await _context.BorrowRecords
				.Include(b => b.Book)
				.Include(b => b.User)
				.Where(b => b.BookId == bookId)
				.ToListAsync();
			await _cacheService.SetAsync(cacheKey, records, TimeSpan.FromMinutes(10));
			return records;
		}

		public async Task<BorrowRecord> CreateAsync(BorrowRecord borrowRecord)
		{
			_context.BorrowRecords.Add(borrowRecord);
			await _context.SaveChangesAsync();
			await _cacheService.RemoveAsync(CacheKey);
			await _cacheService.RemoveAsync($"{CacheKey}:user:{borrowRecord.UserId}");
			await _cacheService.RemoveAsync($"{CacheKey}:book:{borrowRecord.BookId}");
			return borrowRecord;
		}

		public async Task<BorrowRecord> UpdateAsync(BorrowRecord borrowRecord)
		{
			_context.Entry(borrowRecord).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			await _cacheService.RemoveAsync(CacheKey);
			await _cacheService.RemoveAsync($"{CacheKey}:{borrowRecord.Id}");
			await _cacheService.RemoveAsync($"{CacheKey}:user:{borrowRecord.UserId}");
			await _cacheService.RemoveAsync($"{CacheKey}:book:{borrowRecord.BookId}");
			return borrowRecord;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var record = await _context.BorrowRecords.FindAsync(id);
			if (record == null) return false;

			_context.BorrowRecords.Remove(record);
			await _context.SaveChangesAsync();

			await _cacheService.RemoveAsync(CacheKey);
			await _cacheService.RemoveAsync($"{CacheKey}:{id}");
			await _cacheService.RemoveAsync($"{CacheKey}:user:{record.UserId}");
			await _cacheService.RemoveAsync($"{CacheKey}:book:{record.BookId}");
			return true;
		}
	}
}
