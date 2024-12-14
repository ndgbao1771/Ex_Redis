using Ex_Redis.API.Services.Interfaces;
using Ex_Redis.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ex_Redis.API.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class BooksController : ControllerBase
	{
		private readonly IBookService _bookService;

		public BooksController(IBookService bookService)
		{
			_bookService = bookService;
		}

		[HttpGet]
		public async Task<ActionResult<IEnumerable<Book>>> GetAll()
		{
			var books = await _bookService.GetAllAsync();
			return Ok(books);
		}

		[HttpGet("{id}")]
		public async Task<ActionResult<Book>> GetById(int id)
		{
			var book = await _bookService.GetByIdAsync(id);
			if (book == null) return NotFound();
			return Ok(book);
		}

		[HttpPost]
		public async Task<ActionResult<Book>> Create(Book book)
		{
			var createdBook = await _bookService.CreateAsync(book);
			return CreatedAtAction(nameof(GetById), new { id = createdBook.Id }, createdBook);
		}

		[HttpPut("{id}")]
		public async Task<ActionResult<Book>> Update(int id, Book book)
		{
			if (id != book.Id) return BadRequest();
			var updatedBook = await _bookService.UpdateAsync(book);
			return Ok(updatedBook);
		}

		[HttpDelete("{id}")]
		public async Task<ActionResult> Delete(int id)
		{
			var result = await _bookService.DeleteAsync(id);
			if (!result) return NotFound();
			return NoContent();
		}
	}
}
