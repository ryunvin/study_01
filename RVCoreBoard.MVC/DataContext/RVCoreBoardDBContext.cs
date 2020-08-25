using Microsoft.EntityFrameworkCore;
using RVCoreBoard.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.DataContext
{
    public class RVCoreBoardDBContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public DbSet<Board> Boards { get; set; }

        public DbSet<Comment> Comments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=localhost;Database=RVCoreBoardDb;User Id=sa;Password=vin931105;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Comment>()
                .HasOne(f => f.user)
                .WithMany()
                .HasForeignKey("UNo")
                .OnDelete(DeleteBehavior.Restrict); // no ON DELETE

            /*
            *   modelBuilder.Entity<Comment>()
            *     .HasOne(f => f.user)
            *     .WithMany()
            *     .HasForeignKey("UNo")
            *     .OnDelete(DeleteBehavior.Cascade); // set ON DELETE CASCADE
            */
        }
    }
}
