using Ex_Redis.API.Redises.Interfaces;
using Ex_Redis.Data.Entities;
using Ex_Redis.Data;
using Microsoft.EntityFrameworkCore;
using Ex_Redis.API.Services.Interfaces;

namespace Ex_Redis.API.Services.Implements
{
	public class BookService : IBookService
	{
		private readonly ExRedisDbContext _context;
		private readonly ICacheService _cacheService;
		private const string CacheKey = "books";

		public BookService(ExRedisDbContext context, ICacheService cacheService)
		{
			_context = context;
			_cacheService = cacheService;
		}

		public async Task<IEnumerable<Book>> GetAllAsync()
		{
			var cachedBooks = await _cacheService.GetAsync<IEnumerable<Book>>(CacheKey);
			if (cachedBooks != null) return cachedBooks;

			var books = await _context.Books.ToListAsync();
			await _cacheService.SetAsync(CacheKey, books, TimeSpan.FromMinutes(10));
			return books;
		}

		public async Task<Book> GetByIdAsync(int id)
		{
			var cacheKey = $"{CacheKey}:{id}";
			var cachedBook = await _cacheService.GetAsync<Book>(cacheKey);
			if (cachedBook != null) return cachedBook;

			var book = await _context.Books.FindAsync(id);
			if (book != null)
				await _cacheService.SetAsync(cacheKey, book, TimeSpan.FromMinutes(10));
			return book;
		}

		public async Task<Book> CreateAsync(Book book)
		{
			_context.Books.Add(book);
			await _context.SaveChangesAsync();
			await _cacheService.RemoveAsync(CacheKey);
			return book;
		}

		public async Task<Book> UpdateAsync(Book book)
		{
			_context.Entry(book).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			var cacheKey = $"{CacheKey}:{book.Id}";
			await _cacheService.RemoveAsync(CacheKey);
			await _cacheService.RemoveAsync(cacheKey);
			return book;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var book = await _context.Books.FindAsync(id);
			if (book == null) return false;

			_context.Books.Remove(book);
			await _context.SaveChangesAsync();

			var cacheKey = $"{CacheKey}:{id}";
			await _cacheService.RemoveAsync(CacheKey);
			await _cacheService.RemoveAsync(cacheKey);
			return true;
		}
	}
}
