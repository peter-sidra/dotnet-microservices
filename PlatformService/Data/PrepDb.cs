// !This is just for testing purposes

using System;
using System.Linq;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlatformService.Models;

namespace PlatformService.Data {
    // https://youtu.be/DgVjEo3OGBI?t=4613
    public static class PrepDb {
        public static void PrepPopulation(IApplicationBuilder app, bool isProduction) {
            using var serviceScope = app.ApplicationServices.CreateScope();
            SeedData(serviceScope.ServiceProvider.GetService<AppDbContext>(), isProduction);
        }

        private static void SeedData(AppDbContext context, bool isProduction) {
            if (isProduction) {
                Console.WriteLine("---> Attempting to apply migrations...");
                try {
                    context.Database.Migrate();
                }
                catch (Exception ex) {
                    Console.WriteLine($"--> Could not run migrations: {ex.Message}");
                }
            }
            if (!context.Platforms.Any()) {
                Console.WriteLine("--> Seeding Data...");
                context.AddRange(
                    new Platform {
                        Name = "Dot Net",
                        Publisher = "Microsoft",
                        Cost = "Free",
                    },
                    new Platform {
                        Name = "SQL Server Express",
                        Publisher = "Microsoft",
                        Cost = "Free",
                    },
                    new Platform {
                        Name = "Kubernetes",
                        Publisher = "Cloud Native Computing Foundation",
                        Cost = "Free",
                    }
                );
                context.SaveChanges();
            }
            else {
                Console.WriteLine("--> We already have data");
            }
        }
    }
}