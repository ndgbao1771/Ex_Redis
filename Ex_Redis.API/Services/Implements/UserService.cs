using Ex_Redis.API.Redises.Interfaces;
using Ex_Redis.API.Services.Interfaces;
using Ex_Redis.Data.Entities;
using Ex_Redis.Data;
using Microsoft.EntityFrameworkCore;

namespace Ex_Redis.API.Services.Implements
{
	public class UserService : IUserService
	{
		private readonly ExRedisDbContext _context;
		private readonly ICacheService _cacheService;
		private const string CacheKey = "users";

		public UserService(ExRedisDbContext context, ICacheService cacheService)
		{
			_context = context;
			_cacheService = cacheService;
		}

		public async Task<IEnumerable<User>> GetAllAsync()
		{
			var cachedUsers = await _cacheService.GetAsync<IEnumerable<User>>(CacheKey);
			if (cachedUsers != null) return cachedUsers;

			var users = await _context.Users.ToListAsync();
			await _cacheService.SetAsync(CacheKey, users, TimeSpan.FromMinutes(10));
			return users;
		}

		public async Task<User> GetByIdAsync(int id)
		{
			var cacheKey = $"{CacheKey}:{id}";
			var cachedUser = await _cacheService.GetAsync<User>(cacheKey);
			if (cachedUser != null) return cachedUser;

			var user = await _context.Users.FindAsync(id);
			if (user != null)
				await _cacheService.SetAsync(cacheKey, user, TimeSpan.FromMinutes(10));
			return user;
		}

		public async Task<User> CreateAsync(User user)
		{
			_context.Users.Add(user);
			await _context.SaveChangesAsync();
			await _cacheService.RemoveAsync(CacheKey);
			return user;
		}

		public async Task<User> UpdateAsync(User user)
		{
			_context.Entry(user).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			var cacheKey = $"{CacheKey}:{user.Id}";
			await _cacheService.RemoveAsync(CacheKey);
			await _cacheService.RemoveAsync(cacheKey);
			return user;
		}

		public async Task<bool> DeleteAsync(int id)
		{
			var user = await _context.Users.FindAsync(id);
			if (user == null) return false;

			_context.Users.Remove(user);
			await _context.SaveChangesAsync();

			var cacheKey = $"{CacheKey}:{id}";
			await _cacheService.RemoveAsync(CacheKey);
			await _cacheService.RemoveAsync(cacheKey);
			return true;
		}
	}
}
