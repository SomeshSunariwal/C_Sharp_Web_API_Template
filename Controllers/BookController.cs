using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PostgresApplication.Helper;
using PostgresApplication.Model;

namespace PostgresApplication.Controllers
{

    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookContext _context;

        private readonly JwtService _jwtService;

        public BookController(BookContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }

        // GET: api/Book
        [HttpGet("/book")]
        [Authorize]
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
        [HttpGet("/book/{id}")]
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
        [HttpPut("/book/{id}")]
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
        [HttpPost("/book")]
        public async Task<ActionResult<Book>> PostBook([FromBody] CreateBook book)
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
        [HttpDelete("/book/{id}")]
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

        [Route("/Register")]
        [HttpPost]
        public ActionResult<RegisterUser> RegisterUserRequest([FromBody] RegisterUser UserRegisterRequest)
        {
            try
            {
                // Hasing Password
                UserRegisterRequest.Password = BCrypt.Net.BCrypt.HashPassword(UserRegisterRequest.Password);
                _context.RegisterUser.Add(UserRegisterRequest);
                _context.SaveChanges();
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Internal Server Error" });
            }
            return Created("Success", UserRegisterRequest);
        }

        [Route("/Login")]
        [HttpPost]
        public IActionResult Login([FromBody] Login LoginRequest)
        {
            var user = _context.RegisterUser.FirstOrDefault(user => user.Email == LoginRequest.Email);

            // User Not Available in database
            if (user == null) return Unauthorized(new { message = "Unauthorized" });

            // Password Match to the user
            if (!BCrypt.Net.BCrypt.Verify(LoginRequest.Password, user.Password))
            {
                return Unauthorized(new { message = "Unauthorized" });
            }

            var token = _jwtService.TokenGenerator(LoginRequest.Email, user.FirstName, user.LastName);

            return Ok(new { token = token });
        }

        private bool BookExists(int id)
        {
            return _context.Book.Any(e => e.BookId == id);
        }
    }
}
