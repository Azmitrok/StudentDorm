using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Bookings.Web.Models
{
    [Table("Booking")]
    public class Booking
    {
        [Key]
        public int Id { get; set; }

        public int RoomId { get; set; }

        public Room Room { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int UsedPlaces { get; set; }

        public Gender Gender { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
