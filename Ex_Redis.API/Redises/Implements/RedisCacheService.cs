using Ex_Redis.API.Redises.Interfaces;
using StackExchange.Redis;
using System.Text.Json;

namespace Ex_Redis.API.Redises.Implements
{
	public class RedisCacheService : ICacheService
	{
		private readonly IConnectionMultiplexer _redis;
		private readonly IDatabase _db;

		public RedisCacheService(IConnectionMultiplexer redis)
		{
			_redis = redis;
			_db = redis.GetDatabase();
		}

		public async Task<T> GetAsync<T>(string key)
		{
			var value = await _db.StringGetAsync(key);
			if (value.IsNull)
				return default;
			return JsonSerializer.Deserialize<T>(value);
		}

		public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null)
		{
			var serializedValue = JsonSerializer.Serialize(value);
			if (expiration.HasValue)
				await _db.StringSetAsync(key, serializedValue, expiration);
			else
				await _db.StringSetAsync(key, serializedValue);
		}

		public async Task RemoveAsync(string key)
		{
			await _db.KeyDeleteAsync(key);
		}

		public async Task<bool> ExistsAsync(string key)
		{
			return await _db.KeyExistsAsync(key);
		}
	}
}
