using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PostgresApplication.Model
{
    public class Book
    {
        public int BookId { get; set; }

        public string BookName { get; set; }

        public string Details { get; set; }

        public int Price { get; set; }

        public virtual List<Location> Location { get; set; }
    }

    public class UpdateBook
    {
        public string BookName { get; set; }

        public string Details { get; set; }

        public int Price { get; set; }

        public virtual List<Location> Location { get; set; }
    }
}
