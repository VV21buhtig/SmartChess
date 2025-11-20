using Microsoft.EntityFrameworkCore;
using SmartChess.Models.Entities;
using System;

namespace SmartChess.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Move> Moves { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(
                    @"Server=LAPTOP-EU7O01O0\SQLEXPRESS01;Database=SmartChessDB;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Game>()
                .HasOne(g => g.User)
                .WithMany(u => u.Games)
                .HasForeignKey(g => g.UserId);

            modelBuilder.Entity<Move>()
                .HasOne(m => m.Game)
                .WithMany(g => g.Moves)
                .HasForeignKey(m => m.GameId);
        }
    }
}