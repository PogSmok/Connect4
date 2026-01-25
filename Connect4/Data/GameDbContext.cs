using Microsoft.EntityFrameworkCore;

using Connect4.Models;

namespace Connect4.Data
{
    public class GameDbContext : DbContext
    {
        public DbSet<GameDbRecord> Games => Set<GameDbRecord>();

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite("Data Source=Connect4.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<GameDbRecord>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.SettingsJson).IsRequired();

                entity.Property(e => e.Result).IsRequired();

                entity.Property(e => e.PlayedAt).IsRequired();

                // Ignore non-mapped properties
                entity.Ignore(e => e.Settings);
                entity.Ignore(e => e.Moves);
                entity.Ignore(e => e.PlayersText);
                entity.Ignore(e => e.ResultText);
                entity.Ignore(e => e.PlayedAtText);
            });
        }
    }
}
