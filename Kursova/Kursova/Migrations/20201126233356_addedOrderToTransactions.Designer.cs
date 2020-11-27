﻿// <auto-generated />
using System;
using Kursova;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Kursova.Migrations
{
    [DbContext(typeof(OutfitsContext))]
    [Migration("20201126233356_addedOrderToTransactions")]
    partial class addedOrderToTransactions
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("ClothingOutfits", b =>
                {
                    b.Property<int>("ClothesClothingID")
                        .HasColumnType("int");

                    b.Property<int>("OutfitsOutfitID")
                        .HasColumnType("int");

                    b.HasKey("ClothesClothingID", "OutfitsOutfitID");

                    b.HasIndex("OutfitsOutfitID");

                    b.ToTable("ClothingOutfits");
                });

            modelBuilder.Entity("CreditCardUser", b =>
                {
                    b.Property<int>("CreditCardsCreditCardID")
                        .HasColumnType("int");

                    b.Property<int>("UsersUserID")
                        .HasColumnType("int");

                    b.HasKey("CreditCardsCreditCardID", "UsersUserID");

                    b.HasIndex("UsersUserID");

                    b.ToTable("CreditCardUser");
                });

            modelBuilder.Entity("Kursova.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CategoryName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CategoryID");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("Kursova.Models.Clothing", b =>
                {
                    b.Property<int>("ClothingID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("CategoryID")
                        .HasColumnType("int");

                    b.Property<string>("ClothingName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Color")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Material")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ClothingID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Clothing");
                });

            modelBuilder.Entity("Kursova.Models.Context.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("DeliveryAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("UserID")
                        .HasColumnType("int");

                    b.HasKey("OrderID");

                    b.HasIndex("UserID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Kursova.Models.Context.OrderDetails", b =>
                {
                    b.Property<int>("OrderDetailsID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("ClothingID")
                        .HasColumnType("int");

                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.HasKey("OrderDetailsID");

                    b.HasIndex("ClothingID");

                    b.HasIndex("OrderID");

                    b.ToTable("OrderDetails");
                });

            modelBuilder.Entity("Kursova.Models.CreditCard", b =>
                {
                    b.Property<int>("CreditCardID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CardType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte>("ExpiryMonth")
                        .HasColumnType("tinyint");

                    b.Property<int>("ExpiryYear")
                        .HasColumnType("int");

                    b.HasKey("CreditCardID");

                    b.ToTable("CreditCards");
                });

            modelBuilder.Entity("Kursova.Models.Outfits", b =>
                {
                    b.Property<int>("OutfitID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("OutfitName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("OutfitID");

                    b.ToTable("Outfits");
                });

            modelBuilder.Entity("Kursova.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("CreditCardID")
                        .HasColumnType("int");

                    b.Property<int>("OrderID")
                        .HasColumnType("int");

                    b.Property<string>("TransactionType")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("TransactionID");

                    b.HasIndex("CreditCardID");

                    b.HasIndex("OrderID");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Kursova.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ClothingOutfits", b =>
                {
                    b.HasOne("Kursova.Models.Clothing", null)
                        .WithMany()
                        .HasForeignKey("ClothesClothingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kursova.Models.Outfits", null)
                        .WithMany()
                        .HasForeignKey("OutfitsOutfitID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("CreditCardUser", b =>
                {
                    b.HasOne("Kursova.Models.CreditCard", null)
                        .WithMany()
                        .HasForeignKey("CreditCardsCreditCardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kursova.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersUserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Kursova.Models.Clothing", b =>
                {
                    b.HasOne("Kursova.Models.Category", "Category")
                        .WithMany("Clothes")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Kursova.Models.Context.Order", b =>
                {
                    b.HasOne("Kursova.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Kursova.Models.Context.OrderDetails", b =>
                {
                    b.HasOne("Kursova.Models.Clothing", "Clothing")
                        .WithMany()
                        .HasForeignKey("ClothingID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kursova.Models.Context.Order", "Order")
                        .WithMany("Details")
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Clothing");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Kursova.Models.Transaction", b =>
                {
                    b.HasOne("Kursova.Models.CreditCard", "CreditCard")
                        .WithMany("transactions")
                        .HasForeignKey("CreditCardID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kursova.Models.Context.Order", "Order")
                        .WithMany()
                        .HasForeignKey("OrderID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreditCard");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Kursova.Models.Category", b =>
                {
                    b.Navigation("Clothes");
                });

            modelBuilder.Entity("Kursova.Models.Context.Order", b =>
                {
                    b.Navigation("Details");
                });

            modelBuilder.Entity("Kursova.Models.CreditCard", b =>
                {
                    b.Navigation("transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
