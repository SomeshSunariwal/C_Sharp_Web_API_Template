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
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookContext _context;

        public BookController(BookContext context)
        {
            _context = context;
            _context.Database.EnsureCreated();
        }

        // GET: api/Books
        [HttpGet]
        public  List<Book> GetBook()
        {
            var Books = _context.Book.ToList();

            foreach (var book in Books)
            {
                book.Location = _context.Location.Where(x => x.LocationId == book.BookId).ToList();
            }

            return Books;
        }

    }
}
