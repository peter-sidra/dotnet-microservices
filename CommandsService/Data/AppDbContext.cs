using CommandsService.Models;
using Microsoft.EntityFrameworkCore;

namespace CommandsService.Data {
    public class AppDbContext : DbContext {
        public AppDbContext(DbContextOptions<AppDbContext> opt) : base(opt) {

        }

        public DbSet<Platform> Platforms { get; set; }

        public DbSet<Command> Commands { get; set; }

        // Although it's not needed, we will explicitly define the relationship
        // between the platforms and commands, this would have been handeled automatically
        // by entity framework otherwise
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder
                .Entity<Platform>()
                .HasMany(p => p.Commands)
                .WithOne(p => p.Platform!)
                .HasForeignKey(p => p.PlatformId);

            modelBuilder
                .Entity<Command>()
                .HasOne(p => p.Platform)
                .WithMany(p => p.Commands)
                .HasForeignKey(p => p.PlatformId);
        }
    }
}