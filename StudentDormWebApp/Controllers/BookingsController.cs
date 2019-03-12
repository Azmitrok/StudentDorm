using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentDormWebApp.Models;

namespace StudentDormWebApp.Controllers
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
    }
}