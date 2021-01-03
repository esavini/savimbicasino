using Microsoft.EntityFrameworkCore;
using SavimbiCasino.WebApi.Models;

namespace SavimbiCasino.WebApi
{
    public class SavimbiCasinoDbContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }

        public DbSet<Player> Players { get; set; }

        public SavimbiCasinoDbContext(DbContextOptions<SavimbiCasinoDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Room>().HasKey(p => p.Id);
            modelBuilder.Entity<Player>().HasKey(p => p.Id);

            modelBuilder.Entity<Room>().Property(p => p.Id).ValueGeneratedOnAdd();
            modelBuilder.Entity<Player>().Property(p => p.Id).ValueGeneratedOnAdd();

            modelBuilder.Entity<Player>().Property(p => p.Username).IsRequired();
            modelBuilder.Entity<Player>().Property(p => p.HashedPassword).IsRequired();
            modelBuilder.Entity<Room>().Property(p => p.DealerId).IsRequired();
            modelBuilder.Entity<Room>().Property(p => p.Name).IsRequired();
            
            modelBuilder.Entity<Player>().Property(p => p.Username).IsUnicode();
            modelBuilder.Entity<Room>().Property(p => p.Name).IsUnicode();

            modelBuilder.Entity<Player>().HasMany(p => p.DealerRooms)
                .WithOne(p => p.Dealer)
                .HasForeignKey(p => p.DealerId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}