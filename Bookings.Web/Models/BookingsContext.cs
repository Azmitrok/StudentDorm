using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Bookings.Web.Models
{

    public class BookingsContext : DbContext
    {
        private const string connectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=BookingsDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {


            //services.AddDbContext<BookingsContext>(opt => opt.UseInMemoryDatabase("BookingsDB"));
            //services.AddDbContext<BookingsContext>(opt => opt.UseSqlServer(connectionString));
            optionsBuilder.UseSqlServer(connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>()
                .Ignore(r => r.Bookings);
        }


        public DbSet<Booking> Bookings { get; set; }

        public DbSet<Room> Rooms { get; set; }
    }

}
