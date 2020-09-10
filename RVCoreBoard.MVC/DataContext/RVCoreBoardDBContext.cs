using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RVCoreBoard.MVC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCoreBoard.MVC.DataContext
{
    public class RVCoreBoardDBContext : DbContext
    {
        public RVCoreBoardDBContext(DbContextOptions<RVCoreBoardDBContext> options)
            : base(options)
        {
            //
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Board> Boards { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<Attach> Attachs { get; set; }

        public DbSet<Category> Catergorys { get; set; }

        public DbSet<CategoryGroup> CatergoryGroups { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("localDB"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Category>()
               .HasIndex(c => new { c.Gid, c.Id });

            modelBuilder.Entity<User>()
                .Property(u => u.Level)
                .HasDefaultValue(0);

            modelBuilder.Entity<Comment>()
                .HasOne(f => f.user)
                .WithMany()
                .HasForeignKey("UNo")
                .OnDelete(DeleteBehavior.Restrict); // no ON DELETE

            modelBuilder.Entity<Board>()
               .HasOne(f => f.category)
               .WithMany()
               .HasForeignKey("Id")
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
