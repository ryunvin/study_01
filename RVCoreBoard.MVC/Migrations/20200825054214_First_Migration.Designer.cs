﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RVCoreBoard.MVC.DataContext;

namespace RVCoreBoard.MVC.Migrations
{
    [DbContext(typeof(RVCoreBoardDBContext))]
    [Migration("20200825054214_First_Migration")]
    partial class First_Migration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.14-servicing-32113")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RVCoreBoard.MVC.Models.Board", b =>
                {
                    b.Property<int>("BNo")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cnt_Read");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("Reg_Date");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.Property<int>("UNo");

                    b.HasKey("BNo");

                    b.HasIndex("UNo");

                    b.ToTable("Boards");
                });

            modelBuilder.Entity("RVCoreBoard.MVC.Models.Comment", b =>
                {
                    b.Property<int>("CNo")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BNo");

                    b.Property<string>("Content")
                        .IsRequired();

                    b.Property<DateTime>("Reg_Date");

                    b.Property<int>("UNo");

                    b.HasKey("CNo");

                    b.HasIndex("BNo");

                    b.HasIndex("UNo");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("RVCoreBoard.MVC.Models.User", b =>
                {
                    b.Property<int>("UNo")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address")
                        .IsRequired();

                    b.Property<DateTime>("Birth");

                    b.Property<string>("DetailAddress")
                        .IsRequired();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("Id")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<string>("Phone")
                        .IsRequired();

                    b.Property<string>("Tel")
                        .IsRequired();

                    b.HasKey("UNo");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("RVCoreBoard.MVC.Models.Board", b =>
                {
                    b.HasOne("RVCoreBoard.MVC.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("UNo")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("RVCoreBoard.MVC.Models.Comment", b =>
                {
                    b.HasOne("RVCoreBoard.MVC.Models.Board", "board")
                        .WithMany()
                        .HasForeignKey("BNo")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("RVCoreBoard.MVC.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("UNo")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}