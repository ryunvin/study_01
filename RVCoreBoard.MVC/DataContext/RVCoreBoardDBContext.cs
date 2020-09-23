using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RVCoreBoard.MVC.Models;
using System;

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

        public DbSet<Category> Categorys { get; set; }

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

            modelBuilder.Entity<User>()
                .Property(u => u.Level)
                .HasDefaultValue(0);

            modelBuilder.Entity<Comment>()
                .HasOne(f => f.user)
                .WithMany()
                .HasForeignKey("UNo")
                .OnDelete(DeleteBehavior.Restrict); // no ON DELETE

            //
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
