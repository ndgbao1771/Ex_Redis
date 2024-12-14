using Ex_Redis.API.Redises.Implements;
using Ex_Redis.API.Redises.Interfaces;
using Ex_Redis.API.Services.Implements;
using Ex_Redis.API.Services.Interfaces;
using StackExchange.Redis;

namespace Ex_Redis.API.Extensions
{
	public static class ServiceExtensions
	{
		public static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration configuration)
		{
			services.AddSingleton<IConnectionMultiplexer>(opt =>
				ConnectionMultiplexer.Connect(configuration.GetConnectionString("Redis")));
			services.AddScoped<ICacheService, RedisCacheService>();

			return services;
		}

		public static IServiceCollection AddApplicationServices(this IServiceCollection services)
		{
			services.AddScoped<IBookService, BookService>();
			services.AddScoped<IUserService, UserService>();
			services.AddScoped<IBorrowRecordService, BorrowRecordService>();

			return services;
		}
	}
}
