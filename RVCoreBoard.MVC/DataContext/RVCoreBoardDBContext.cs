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

            modelBuilder.Entity<Category>(entity =>
            {
                entity.HasKey(c => new { c.Gid, c.Id });
                entity.HasIndex(c => new { c.Gid, c.Id }).IsUnique();
            });
            
            modelBuilder.Entity<Board>(entity =>
            {
                // Fluent API for column properties
                entity.HasKey(b => new { b.Gid, b.Id, b.BNo });
                entity.HasIndex(b => new { b.Gid, b.Id, b.BNo }).IsUnique();

                entity.HasOne(b => b.category)
                    .WithMany()
                    .HasForeignKey(b => new { b.Gid, b.Id })
                    .HasPrincipalKey(c => new { c.Gid, c.Id })
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_Board_Category_Gid_Id");
            });

            modelBuilder.Entity<Attach>()
               .HasOne(a => a.board)
               .WithMany()
               .HasForeignKey(a => new { a.Gid, a.Id, a.BNo })
               .HasPrincipalKey(b => new { b.Gid, b.Id, b.BNo })
               .OnDelete(DeleteBehavior.Cascade)
               .HasConstraintName("FK_Attach_Board_Gid_Id_BNo");

            modelBuilder.Entity<Comment>()
               .HasOne(c => c.board)
               .WithMany()
               .HasForeignKey(c => new { c.Gid, c.Id, c.BNo })
               .HasPrincipalKey(b => new { b.Gid, b.Id, b.BNo })
               .OnDelete(DeleteBehavior.Cascade)
               .HasConstraintName("FK_Comment_Board_Gid_Id_BNo");

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
