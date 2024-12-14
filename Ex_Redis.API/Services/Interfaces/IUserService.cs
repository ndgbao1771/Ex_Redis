using Ex_Redis.Data.Entities;

namespace Ex_Redis.API.Services.Interfaces
{
	public interface IUserService
	{
		Task<IEnumerable<User>> GetAllAsync();
		Task<User> GetByIdAsync(int id);
		Task<User> CreateAsync(User user);
		Task<User> UpdateAsync(User user);
		Task<bool> DeleteAsync(int id);
	}
}
