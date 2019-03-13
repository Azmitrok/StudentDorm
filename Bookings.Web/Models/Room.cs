using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bookings.Web.Models
{
    [Table("Room")]
    public class Room
    {
        [Key]
        public int Id { get; set; }
        public int Number { get; set; }
        public int AvailablePlaces { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
