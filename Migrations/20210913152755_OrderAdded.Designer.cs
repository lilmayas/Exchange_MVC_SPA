﻿// <auto-generated />
using System;
using MVC_SPA.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MVC_SPA.Migrations
{
    [DbContext(typeof(StockContext))]
    [Migration("20210913152755_OrderAdded")]
    partial class OrderAdded
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MVC_SPA.Models.StockItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Amount");

                    b.Property<double>("BuyRate");

                    b.Property<string>("Description");

                    b.Property<bool>("IsVisible");

                    b.Property<string>("LogoFilename");

                    b.Property<double>("SellRate");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.ToTable("StockItems");
                });

            modelBuilder.Entity("MVC_SPA.Models.StockOrder", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Cnt");

                    b.Property<int>("IdItem");

                    b.Property<DateTime>("Moment");

                    b.HasKey("Id");

                    b.ToTable("StockOrders");
                });
#pragma warning restore 612, 618
        }
    }
}
