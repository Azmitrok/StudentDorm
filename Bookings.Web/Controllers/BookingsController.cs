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
                var busyRooms = GetBusyRooms(context, startDate, endDate);

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
            bool isAdded = false;
            using (var context = new BookingsContext())
            {
                var room = context.Rooms.First(r => r.Id == booking.RoomId);
                if (IsFreeRoom(context, room, booking.StartDate, booking.EndDate))
                {

                    context.Bookings.Add(booking);
                    context.SaveChanges();

                    isAdded = true;
                }
            }

            return Ok(isAdded);

        }

        private bool IsFreeRoom(BookingsContext context, Room room, DateTime startDate, DateTime endDate)
        {
            var busyRooms = GetBusyRooms(context, startDate, endDate);
            return context.Rooms.ToList().Except(busyRooms).Any(r => r.Id == room.Id);
        }

        private IQueryable<Room> GetBusyRooms(BookingsContext context, DateTime startDate, DateTime endDate)
        {
            return context.Bookings.Where(b => b.StartDate < endDate && b.EndDate > startDate).Select(b => b.Room).Distinct();
        }
        
    }

}