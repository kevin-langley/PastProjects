﻿// <auto-generated />
using System;
using Group_18_Final_Project.DAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Group_18_Final_Project.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Group_18_Final_Project.Models.Book", b =>
                {
                    b.Property<int>("BookID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ActiveBook");

                    b.Property<string>("Author");

                    b.Property<decimal>("AverageRating");

                    b.Property<decimal>("BookPrice");

                    b.Property<int?>("BookReorderID");

                    b.Property<int>("CopiesOnHand");

                    b.Property<string>("Description");

                    b.Property<int?>("GenreID");

                    b.Property<DateTime>("PublicationDate");

                    b.Property<int>("TimesPurchased");

                    b.Property<string>("Title");

                    b.Property<int>("UniqueID");

                    b.Property<decimal>("WholesalePrice");

                    b.HasKey("BookID");

                    b.HasIndex("BookReorderID");

                    b.HasIndex("GenreID");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("Group_18_Final_Project.Models.BookOrder", b =>
                {
                    b.Property<int>("BookOrderID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BookID");

                    b.Property<decimal>("ExtendedPrice");

                    b.Property<int?>("OrderID");

                    b.Property<int>("OrderQuantity");

                    b.Property<decimal>("Price");

                    b.HasKey("BookOrderID");

                    b.HasIndex("BookID");

                    b.HasIndex("OrderID");

                    b.ToTable("BookOrders");
                });

            modelBuilder.Entity("Group_18_Final_Project.Models.BookReorder", b =>
                {
                    b.Property<int>("BookReorderID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ReorderQuantity");

                    b.HasKey("BookReorderID");

                    b.ToTable("BookReorders");
                });

            modelBuilder.Entity("Group_18_Final_Project.Models.Coupon", b =>
                {
                    b.Property<int>("CouponID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CouponCode")
                        .IsRequired()
                        .HasMaxLength(20);

                    b.Property<int>("CouponName");

                    b.HasKey("CouponID");

                    b.ToTable("Coupons");
                });

            modelBuilder.Entity("Group_18_Final_Project.Models.CreditCard", b =>
                {
                    b.Property<int>("CreditCardID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("CreditCardNumber");

                    b.Property<int>("CreditType");

                    b.Property<int?>("UserID");

                    b.HasKey("CreditCardID");

                    b.HasIndex("UserID");

                    b.ToTable("CreditCards");
                });

            modelBuilder.Entity("Group_18_Final_Project.Models.Genre", b =>
                {
                    b.Property<int>("GenreID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("GenreName");

                    b.HasKey("GenreID");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("Group_18_Final_Project.Models.Order", b =>
                {
                    b.Property<int>("OrderID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CouponID");

                    b.Property<int?>("CreditCardID");

                    b.Property<bool>("IsPending");

                    b.Property<DateTime>("OrderDate");

                    b.Property<decimal>("ShippingAdditionalPrice");

                    b.Property<decimal>("ShippingFirstPrice");

                    b.Property<int?>("UserID");

                    b.HasKey("OrderID");

                    b.HasIndex("CouponID");

                    b.HasIndex("CreditCardID");

                    b.HasIndex("UserID");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("Group_18_Final_Project.Models.Reorder", b =>
                {
                    b.Property<int>("ReorderID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BookReorderID");

                    b.Property<int?>("UserID");

                    b.HasKey("ReorderID");

                    b.HasIndex("BookReorderID");

                    b.HasIndex("UserID");

                    b.ToTable("Reorders");
                });

            modelBuilder.Entity("Group_18_Final_Project.Models.Review", b =>
                {
                    b.Property<int>("ReviewID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Approval");

                    b.Property<int?>("ApproverUserID");

                    b.Property<int?>("AuthorUserID");

                    b.Property<int?>("BookID");

                    b.Property<int>("Rating");

                    b.Property<string>("ReviewText")
                        .HasMaxLength(100);

                    b.HasKey("ReviewID");

                    b.HasIndex("ApproverUserID");

                    b.HasIndex("AuthorUserID");

                    b.HasIndex("BookID");

                    b.ToTable("Reviews");
                });

            modelBuilder.Entity("Group_18_Final_Project.Models.User", b =>
                {
                    b.Property<int>("UserID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("ActiveUser");

                    b.Property<string>("Address");

                    b.Property<string>("City");

                    b.Property<string>("Email");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.Property<string>("Password");

                    b.Property<long>("PhoneNumber");

                    b.Property<string>("State");

                    b.Property<string>("UserType");

                    b.Property<int>("ZipCode");

                    b.HasKey("UserID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Group_18_Final_Project.Models.Book", b =>
                {
                    b.HasOne("Group_18_Final_Project.Models.BookReorder")
                        .WithMany("Books")
                        .HasForeignKey("BookReorderID");

                    b.HasOne("Group_18_Final_Project.Models.Genre", "Genre")
                        .WithMany("Books")
                        .HasForeignKey("GenreID");
                });

            modelBuilder.Entity("Group_18_Final_Project.Models.BookOrder", b =>
                {
                    b.HasOne("Group_18_Final_Project.Models.Book", "Book")
                        .WithMany("BookOrders")
                        .HasForeignKey("BookID");

                    b.HasOne("Group_18_Final_Project.Models.Order", "Order")
                        .WithMany("BookOrders")
                        .HasForeignKey("OrderID");
                });

            modelBuilder.Entity("Group_18_Final_Project.Models.CreditCard", b =>
                {
                    b.HasOne("Group_18_Final_Project.Models.User", "User")
                        .WithMany("CreditCards")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("Group_18_Final_Project.Models.Order", b =>
                {
                    b.HasOne("Group_18_Final_Project.Models.Coupon", "Coupon")
                        .WithMany("Orders")
                        .HasForeignKey("CouponID");

                    b.HasOne("Group_18_Final_Project.Models.CreditCard", "CreditCard")
                        .WithMany("Orders")
                        .HasForeignKey("CreditCardID");

                    b.HasOne("Group_18_Final_Project.Models.User", "User")
                        .WithMany("Orders")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("Group_18_Final_Project.Models.Reorder", b =>
                {
                    b.HasOne("Group_18_Final_Project.Models.BookReorder", "BookReorder")
                        .WithMany("Reorders")
                        .HasForeignKey("BookReorderID");

                    b.HasOne("Group_18_Final_Project.Models.User", "User")
                        .WithMany("Reorders")
                        .HasForeignKey("UserID");
                });

            modelBuilder.Entity("Group_18_Final_Project.Models.Review", b =>
                {
                    b.HasOne("Group_18_Final_Project.Models.User", "Approver")
                        .WithMany("ReviewsApproved")
                        .HasForeignKey("ApproverUserID");

                    b.HasOne("Group_18_Final_Project.Models.User", "Author")
                        .WithMany("ReviewsWritten")
                        .HasForeignKey("AuthorUserID");

                    b.HasOne("Group_18_Final_Project.Models.Book", "Book")
                        .WithMany("Reviews")
                        .HasForeignKey("BookID");
                });
#pragma warning restore 612, 618
        }
    }
}
