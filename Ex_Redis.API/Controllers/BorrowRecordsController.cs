using Ex_Redis.API.Services.Interfaces;
using Ex_Redis.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ex_Redis.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BorrowRecordsController : ControllerBase
	{
		private readonly IBorrowRecordService _borrowRecordService;

		public BorrowRecordsController(IBorrowRecordService borrowRecordService)
		{
			_borrowRecordService = borrowRecordService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<BorrowRecord>>> GetAll()
		{
			var records = await _borrowRecordService.GetAllAsync();
			return Ok(records);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<BorrowRecord>> GetById(int id)
		{
			var record = await _borrowRecordService.GetByIdAsync(id);
			if (record == null) return NotFound();
			return Ok(record);
		}

		[HttpGet("user/{userId}")]
		public async Task<ActionResult<IEnumerable<BorrowRecord>>> GetByUserId(int userId)
		{
			var records = await _borrowRecordService.GetByUserIdAsync(userId);
			return Ok(records);
		}

		[HttpGet("book/{bookId}")]
		public async Task<ActionResult<IEnumerable<BorrowRecord>>> GetByBookId(int bookId)
		{
			var records = await _borrowRecordService.GetByBookIdAsync(bookId);
			return Ok(records);
		}

		[HttpPost]
		public async Task<ActionResult<BorrowRecord>> Create(BorrowRecord borrowRecord)
		{
			var createdRecord = await _borrowRecordService.CreateAsync(borrowRecord);
			return CreatedAtAction(nameof(GetById), new { id = createdRecord.Id }, createdRecord);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<BorrowRecord>> Update(int id, BorrowRecord borrowRecord)
		{
			if (id != borrowRecord.Id) return BadRequest();
			var updatedRecord = await _borrowRecordService.UpdateAsync(borrowRecord);
			return Ok(updatedRecord);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			var result = await _borrowRecordService.DeleteAsync(id);
			if (!result) return NotFound();
			return NoContent();
		}
	}
}
