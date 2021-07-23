﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PostgresApplication.Model;

namespace PostgresApplication.Migrations
{
    [DbContext(typeof(BookContext))]
    [Migration("20210723175407_init2")]
    partial class init2
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.8")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("PostgresApplication.Model.Book", b =>
                {
                    b.Property<int>("BookId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("BookName")
                        .HasColumnType("text");

                    b.Property<string>("Details")
                        .HasColumnType("text");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.HasKey("BookId");

                    b.ToTable("Book");

                    b.HasData(
                        new
                        {
                            BookId = 1,
                            BookName = "Book 1",
                            Details = "Details",
                            Price = 100
                        },
                        new
                        {
                            BookId = 2,
                            BookName = "Book 2",
                            Details = "Details 2",
                            Price = 100
                        },
                        new
                        {
                            BookId = 3,
                            BookName = "Book 3",
                            Details = "Details 3",
                            Price = 100
                        },
                        new
                        {
                            BookId = 4,
                            BookName = "Book 4",
                            Details = "Details 4",
                            Price = 100
                        });
                });

            modelBuilder.Entity("PostgresApplication.Model.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Address")
                        .HasColumnType("text");

                    b.Property<int>("LocationId")
                        .HasColumnType("integer");

                    b.Property<int>("PinCode")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Location");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "Address 1",
                            LocationId = 1,
                            PinCode = 123
                        },
                        new
                        {
                            Id = 2,
                            Address = "Address 2",
                            LocationId = 2,
                            PinCode = 123
                        },
                        new
                        {
                            Id = 3,
                            Address = "Address 2",
                            LocationId = 1,
                            PinCode = 123
                        });
                });

            modelBuilder.Entity("PostgresApplication.Model.RegisterUser", b =>
                {
                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<int>("ID")
                        .HasColumnType("integer");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.HasKey("Email");

                    b.ToTable("RegisterUser");
                });

            modelBuilder.Entity("PostgresApplication.Model.Location", b =>
                {
                    b.HasOne("PostgresApplication.Model.Book", "Book")
                        .WithMany("Location")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Book");
                });

            modelBuilder.Entity("PostgresApplication.Model.Book", b =>
                {
                    b.Navigation("Location");
                });
#pragma warning restore 612, 618
        }
    }
}
