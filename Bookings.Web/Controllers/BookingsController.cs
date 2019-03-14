using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bookings.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        [HttpGet("[action]")]
        public IEnumerable<Booking> Index()
        {            
            using (var context = new BookingsContext())
            {
                var list = context.Bookings.Include(b => b.Room).ToList();
                
                return list;
            }
        }

        [HttpGet("[action]")]
        public IEnumerable<Room> FreeRooms(DateTime startDate, DateTime endDate)
        {
            using (var context = new BookingsContext())
            {
                var busyRooms = context.Bookings.Where(b => b.StartDate < endDate && b.EndDate > startDate).Select(b => b.Room).Distinct();

                var freeRooms = context.Rooms.Except(busyRooms).ToList();

                return freeRooms;
            }
        }

        [HttpGet("[action]")]
        public IEnumerable<Room> AllRooms()
        {
            using (var context = new BookingsContext())
            {
                return context.Rooms.ToList();
            }
        }

        [HttpPost]
        public IActionResult Add(Booking booking)
        {
            using (var context = new BookingsContext())
            {
                context.Bookings.Add(booking);
                context.SaveChanges();
            }

            return Ok();

        }

        //[HttpPost]
        //public IActionResult Add(string name)
        //{

        //    var s = name;
            



        //    return Ok();

        //}
    }


}