
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace PostgresApplication.Model
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Book")]
        public int LocationId { get; set; }

        public string Address { get; set; }

        public int PinCode { get; set; }

        [JsonIgnore]
        public Book Book { get; set; }
    }

    public class PostLocation
    {
        public string Address { get; set; }

        public int PinCode { get; set; }
    }
}
