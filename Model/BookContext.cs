using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostgresApplication.Model
{
    public class BookContext : DbContext
    {

        public BookContext(DbContextOptions<BookContext> options) : base(options) { }

        public DbSet<Book> Book { get; set; }

        public DbSet<Location> Location { get; set; }

        public DbSet<RegisterUser> RegisterUser { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>().HasData(
                new Book
                {
                    BookId = 1,
                    BookName = "Book 1",
                    Details = "Details",
                    Price = 100
                },
                new Book
                {
                    BookId = 2,
                    BookName = "Book 2",
                    Details = "Details 2",
                    Price = 100
                },
                new Book
                {
                    BookId = 3,
                    BookName = "Book 3",
                    Details = "Details 3",
                    Price = 100
                },
                new Book
                {
                    BookId = 4,
                    BookName = "Book 4",
                    Details = "Details 4",
                    Price = 100
                });

            modelBuilder.Entity<Location>().HasData(
                new Location
                {
                    Id = 1,
                    LocationId = 1,
                    Address = "Address 1",
                    PinCode = 123
                },
                new Location
                {
                    Id = 2,
                    LocationId = 2,
                    Address = "Address 2",
                    PinCode = 123
                },
                new Location
                {
                    Id = 3,
                    LocationId = 1,
                    Address = "Address 2",
                    PinCode = 123
                });
        }
    }
}
