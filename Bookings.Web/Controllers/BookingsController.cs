using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookings.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Bookings.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        [HttpGet("[action]")]
        public IEnumerable<Booking> Index()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new Booking
            {
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddDays(4),
                Gender = Gender.Female,
                UsedPlaces = 1
            });
        }

        [HttpPost("[action]")]
        public IActionResult Add(Booking booking)
        {

            var s = booking.ToString();

            s += "!";

            return Ok();

        }
    }


}