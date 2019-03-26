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
        public IEnumerable<Room> FreeRooms(DateTime startDate, DateTime endDate, Gender gender, int usedPlaces = 1)
        {
            using (var context = new BookingsContext())
            {
                var busyRooms = GetBusyRooms(context, startDate, endDate, gender, usedPlaces);

                var freeRooms = context.Rooms.Except(busyRooms).ToList();

                return freeRooms;
            }
        }

        [HttpGet("[action]")]
        public int GetMaxUsedPlaces(DateTime startDate, DateTime endDate, int roomId)
        {
            using (var context = new BookingsContext())
            {


                //return GetMaxUsedPlacesCountByPeriod(context, context.Rooms.FirstOrDefault(r => r.Id == roomId), startDate, endDate);
                return GetMaxUsedPlacesCountByPeriod(context, context.Rooms.FirstOrDefault(r => r.Id == roomId), startDate, endDate);
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
                if (IsFreeRoom(room, booking.StartDate, booking.EndDate, booking.Gender, booking.UsedPlaces))
                {

                    context.Bookings.Add(booking);
                    context.SaveChanges();

                    isAdded = true;
                }
            }

            return Ok(isAdded);

        }

        private bool IsFreeRoom(Room room, DateTime startDate, DateTime endDate, Gender gender, int usedPlaces)
        {
            using (var context = new BookingsContext())
            {

                var busyRooms = GetBusyRooms(context, startDate, endDate, gender, usedPlaces);

                return context.Rooms.ToList().Except(busyRooms).Any(r => r.Id == room.Id);
            }
        }

        private IQueryable<Room> GetBusyRooms(BookingsContext context, DateTime startDate, DateTime endDate, Gender gender, int usedPlaces)
        {
            var busyByPlacesRooms = context.Bookings.Where(b => b.StartDate < endDate && b.EndDate > startDate)
                .GroupBy(b => b.Room)
                .Where(g => g.Key.AvailablePlaces - g.Sum(b => b.UsedPlaces) < usedPlaces)
                .Select(r => r.Key);

            var busyByAnotherGenderRooms = context.Bookings.Where(b => b.StartDate < endDate && b.EndDate > startDate && b.Gender != gender)                
                .Select(b => b.Room).Distinct();

            //return context.Bookings.Where(b => b.StartDate < endDate && b.EndDate > startDate).GroupBy(b => b.Room).Count();// . Select(b => b.Room).Distinct();

            return busyByPlacesRooms.Union(busyByAnotherGenderRooms).Distinct();
        }

        private int GetMaxUsedPlacesCountByPeriod_Wrong(BookingsContext context, Room room, DateTime startDate, DateTime endDate)
        {
            if (room == null)
                throw new ArgumentNullException("Room");

            var bookings = context.Bookings.Where(b => b.StartDate < endDate && b.EndDate > startDate && b.RoomId == room.Id);

            if (!bookings.Any()) return 0;

            int maxUsedPlaces = 0;

            foreach (var bookingsOrdered in
                new List<IEnumerable<Booking>> { bookings.OrderBy(b => b.StartDate), bookings.OrderBy(b => b.EndDate) })
            {
                var firstBooking = bookingsOrdered.First();

                int usedPlacesCounter = firstBooking.UsedPlaces;
                var startDate1 = firstBooking.StartDate;
                var endDate1 = firstBooking.EndDate;



                foreach (var booking in bookingsOrdered.Skip(1))
                {
                    if (IsDateRangesIntersect(startDate1, endDate1, booking.StartDate, booking.EndDate))
                        usedPlacesCounter += booking.UsedPlaces;
                    else
                    {
                        maxUsedPlaces = Math.Max(maxUsedPlaces, usedPlacesCounter);
                        usedPlacesCounter = booking.UsedPlaces;
                    }

                    startDate1 = booking.StartDate;
                    endDate1 = booking.EndDate;

                }

                maxUsedPlaces = Math.Max(maxUsedPlaces, usedPlacesCounter);
            }

            return maxUsedPlaces;
        }

        private bool IsDateRangesIntersect(DateTime startDate1, DateTime endDate1, DateTime startDate2, DateTime endDate2)
        {
            return startDate1 < endDate2 && endDate1 > startDate2;
        }

        private int GetMaxUsedPlacesCountByPeriod(BookingsContext context, Room room, DateTime startDate, DateTime endDate)
        {
            var bookings = context.Bookings.Where(b => b.StartDate < endDate && b.EndDate > startDate && b.RoomId == room.Id);

            var arrivals = bookings.Select(b => new { Date = b.StartDate, Operation = Operation.Plus, b.UsedPlaces });
            var departures = bookings.Select(b => new { Date = b.EndDate, Operation = Operation.Minus, b.UsedPlaces });

            var maxUsedPlaces = 0;
            var counter = 0;
            foreach (var bookingEvent in arrivals.Union(departures).OrderBy(b => b.Date))
            {
                counter += bookingEvent.Operation == Operation.Plus ? bookingEvent.UsedPlaces : -1 * bookingEvent.UsedPlaces;
                maxUsedPlaces = Math.Max(counter, maxUsedPlaces);
            }

            return maxUsedPlaces;
        }

    }

    public enum Operation
    {
        Plus,
        Minus
    }

}