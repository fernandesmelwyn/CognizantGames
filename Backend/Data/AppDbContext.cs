using Microsoft.EntityFrameworkCore;
using Backend.Models;

namespace Backend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Registration> Registrations { get; set; }
        public DbSet<Fixture> Fixtures { get; set; }
        public DbSet<Score> Scores { get; set; }
        public DbSet<LeaderboardEntry> LeaderboardEntries { get; set; }
        public DbSet<GameFormatType> GameFormatTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasMany(g => g.SupportedFormats)
                .WithMany(f => f.Games);
        }
    }
}
