﻿// <auto-generated />
using System;
using Foodbank_Project.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Foodbank_Project.Migrations
{
    [DbContext(typeof(FoodbankContext))]
    [Migration("20220505152237_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Foodbank_Project.Models.Foodbank", b =>
                {
                    b.Property<int?>("FoodbankId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("FoodbankId"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("AltName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CharityNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CharityRegisterUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool?>("Closed")
                        .IsRequired()
                        .HasColumnType("bit");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("Created")
                        .IsRequired()
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Homepage")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LatLng")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Network")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Notes")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Postcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Protected")
                        .HasColumnType("bit");

                    b.Property<int>("Provider")
                        .HasColumnType("int");

                    b.Property<string>("SecondaryPhone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ShoppingList")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("FoodbankId");

                    b.ToTable("Foodbanks");
                });

            modelBuilder.Entity("Foodbank_Project.Models.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("LocationId"), 1L, 1);

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("FoodbankId")
                        .HasColumnType("int");

                    b.Property<string>("LatLng")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Postcode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slug")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("LocationId");

                    b.HasIndex("FoodbankId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Foodbank_Project.Models.Need", b =>
                {
                    b.Property<int>("NeedId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NeedId"), 1L, 1);

                    b.Property<string>("NeedStr")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("NeedId");

                    b.ToTable("Needs");
                });

            modelBuilder.Entity("FoodbankNeed", b =>
                {
                    b.Property<int>("FoodbanksFoodbankId")
                        .HasColumnType("int");

                    b.Property<int>("NeedsNeedId")
                        .HasColumnType("int");

                    b.HasKey("FoodbanksFoodbankId", "NeedsNeedId");

                    b.HasIndex("NeedsNeedId");

                    b.ToTable("FoodbankNeed");
                });

            modelBuilder.Entity("Foodbank_Project.Models.Location", b =>
                {
                    b.HasOne("Foodbank_Project.Models.Foodbank", "Foodbank")
                        .WithMany("Locations")
                        .HasForeignKey("FoodbankId");

                    b.Navigation("Foodbank");
                });

            modelBuilder.Entity("FoodbankNeed", b =>
                {
                    b.HasOne("Foodbank_Project.Models.Foodbank", null)
                        .WithMany()
                        .HasForeignKey("FoodbanksFoodbankId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Foodbank_Project.Models.Need", null)
                        .WithMany()
                        .HasForeignKey("NeedsNeedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Foodbank_Project.Models.Foodbank", b =>
                {
                    b.Navigation("Locations");
                });
#pragma warning restore 612, 618
        }
    }
}
