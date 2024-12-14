using Ex_Redis.API.Services.Interfaces;
using Ex_Redis.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ex_Redis.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class UsersController : ControllerBase
	{
		private readonly IUserService _userService;

		public UsersController(IUserService userService)
		{
			_userService = userService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<User>>> GetAll()
		{
			var users = await _userService.GetAllAsync();
			return Ok(users);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<User>> GetById(int id)
		{
			var user = await _userService.GetByIdAsync(id);
			if (user == null) return NotFound();
			return Ok(user);
		}

		[HttpPost]
		public async Task<ActionResult<User>> Create(User user)
		{
			var createdUser = await _userService.CreateAsync(user);
			return CreatedAtAction(nameof(GetById), new { id = createdUser.Id }, createdUser);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<User>> Update(int id, User user)
		{
			if (id != user.Id) return BadRequest();
			var updatedUser = await _userService.UpdateAsync(user);
			return Ok(updatedUser);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			var result = await _userService.DeleteAsync(id);
			if (!result) return NotFound();
			return NoContent();
		}
	}
}
