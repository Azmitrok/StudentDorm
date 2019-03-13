using System;
using System.Collections.Generic;
using System.Linq;
using Bookings.Web.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Bookings.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            if (Configuration["GenerateTestData"] == "True")
                GenerateTestData();
        }

        

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            // In production, the Angular files will be served from this directory
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "ClientApp/dist";
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });

            app.UseSpa(spa =>
            {
                // To learn more about options for serving an Angular SPA from ASP.NET Core,
                // see https://go.microsoft.com/fwlink/?linkid=864501

                spa.Options.SourcePath = "ClientApp";

                if (env.IsDevelopment())
                {
                    spa.UseAngularCliServer(npmScript: "start");
                }
            });
        }

        private void GenerateTestData()
        {
            //GenerateRooms();
            GenerateBookings();

        }

        private void GenerateBookings()
        {
            var random = new Random();
            using (var context = new BookingsContext())
            {
                var rooms = context.Rooms.ToListAsync();

                for (int i = 0; i < 100; i++)
                {
                    DateTime startDate = GetRandomStartDate(random);
                    context.Bookings.Add(new Booking() { Room = GetRandomRoom(random, rooms.Result), StartDate = startDate, EndDate = startDate.AddDays(random.Next(2,10)), Gender = GetRamdomGender(random), UsedPlaces = 1 });
                }

                context.SaveChanges();
            }
        }

        private Gender GetRamdomGender(Random random)
        {
            return (Gender)random.Next(0, 1);
        }

        private DateTime GetRandomStartDate(Random random)
        {
            return DateTime.Today.AddDays(random.Next(10, 60));
        }

        private Room GetRandomRoom(Random random, List<Room> rooms)
        {
            return rooms[random.Next(rooms.Count - 1)];
        }

        private void GenerateRooms()
        {
            using (var context = new BookingsContext())
            {
                for (int i = 0; i < 50; i++)
                    context.Rooms.Add(new Room() { AvailablePlaces = 1, Number = 200 + i });

                context.SaveChanges();
            }
        }
    }
}
