using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgresApplication.Model;

namespace PostgresApplication.Controllers
{
    [Route("books/")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookContext _context;

        public BookController(BookContext context)
        {
            _context = context;
        }

        // GET: api/Book
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Book>>> GetBook()
        {
            List<Book> Books = await _context.Book.ToListAsync();

            foreach (var book in Books)
            {
                book.Location = await _context.Location.Where(x => x.LocationId == book.BookId).ToListAsync();
            }
            return Books;
        }

        // GET: api/Book/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            var book = await _context.Book.FindAsync(id);


            if (book == null)
            {
                return NotFound();
            }

            List<Location> locations = await _context.Location.Where(x => x.LocationId == book.BookId).ToListAsync();

            book.Location = locations;

            return book;
        }

        // PUT: api/Book/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBook(int id, UpdateBook book)
        {
            if (!BookExists(id))
            {
                throw new NullReferenceException();
            }
            Book TempBook;
            try
            {
                TempBook = await _context.Book.FindAsync(id);
                if (book.BookName != null)
                {
                    TempBook.BookName = book.BookName;
                }
                if (book.Details != null)
                {
                    TempBook.Details = book.Details;
                }
                if (book.Price != TempBook.Price)
                {
                    TempBook.Price = book.Price;
                }
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw new NullReferenceException();
                }
            }

            return Ok(TempBook);
        }

        // POST: api/Book
        [HttpPost]
        public async Task<ActionResult<Book>> PostBook([FromBody]CreateBook book)
        {

            _context.Book.Add(new Book
            {
                BookName = book.BookName,
                Details = book.Details,
                Price = book.Price,
            });

            if (book.Location != null)
            {
                foreach (var location in book.Location)
                {
                    _context.Location.Add(new Location
                    {
                        LocationId = book.BookId,
                        Address = location.Address,
                        PinCode = location.PinCode,
                    });
                }
            }

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetBook", new { id = book.BookId }, book);
        }

        // DELETE: api/Book/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            var book = await _context.Book.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }

            _context.Book.Remove(book);

            // Removing Associated locations
            List<Location> locations = await _context.Location.Where(x => x.LocationId == book.BookId).ToListAsync();
            if (locations != null)
            {
                foreach (var location in locations)
                {
                    _context.Location.Remove(location);
                }
            }

            await _context.SaveChangesAsync();
            return NoContent();
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.BookId == id);
        }
    }
}
