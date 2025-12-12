using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CarRental.Api.models;

namespace CarRental.Api
{
    public static class SeeData
    {
        public static void Initialize(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDBContext>();
            if (context.Users.Any())
            {
                return; // Datos ya existen
            }

            var users = new List<User>
            {
                new User { Username = "Alice", Email = "mail.com" }   ,
                new User { Username = "Bob", Email = "bob@mail..com"}
            };

            context.Users.AddRange(users);
            context.SaveChanges();
        }
    }
}