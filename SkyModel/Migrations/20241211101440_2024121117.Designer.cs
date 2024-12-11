﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SkyModel;

#nullable disable

namespace SkyModel.Migrations
{
    [DbContext(typeof(SkyDbContext))]
    [Migration("20241211101440_2024121117")]
    partial class _2024121117
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Proxies:ChangeTracking", false)
                .HasAnnotation("Proxies:CheckEquality", false)
                .HasAnnotation("Proxies:LazyLoading", true)
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SkyModel.Models.Category", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("SkyModel.Models.MinIO", b =>
                {
                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<string>("ETag")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<long?>("ProductDetailId")
                        .HasColumnType("bigint");

                    b.Property<long>("Size")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("TimeUp")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("ProductDetailId");

                    b.ToTable("MinIOs");
                });

            modelBuilder.Entity("SkyModel.Models.NewsArticle", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("ThumbnailImageId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("TimeUp")
                        .HasColumnType("datetime2");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ThumbnailImageId");

                    b.ToTable("NewsArticles");
                });

            modelBuilder.Entity("SkyModel.Models.Product", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("CategoryId")
                        .HasColumnType("bigint");

                    b.Property<long>("DetailId")
                        .HasColumnType("bigint");

                    b.Property<bool>("Hot")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<long?>("ThumbnailImageId")
                        .HasColumnType("bigint");

                    b.Property<DateTime>("TimeUp")
                        .HasColumnType("datetime2");

                    b.Property<long>("UserId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("DetailId");

                    b.HasIndex("Name");

                    b.HasIndex("ThumbnailImageId");

                    b.HasIndex("UserId");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("SkyModel.Models.ProductDetail", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Address")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Area")
                        .HasColumnType("float");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Features")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Length")
                        .HasColumnType("float");

                    b.Property<double?>("PricePerSquareMeter")
                        .HasColumnType("float");

                    b.Property<string>("Structure")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double?>("Width")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("ProductDetails");
                });

            modelBuilder.Entity("SkyModel.Models.User", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<string>("Avatar")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsActive")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Role")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SkyModel.Models.MinIO", b =>
                {
                    b.HasOne("SkyModel.Models.ProductDetail", "ObjProductDetail")
                        .WithMany("Images")
                        .HasForeignKey("ProductDetailId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("ObjProductDetail");
                });

            modelBuilder.Entity("SkyModel.Models.NewsArticle", b =>
                {
                    b.HasOne("SkyModel.Models.MinIO", "ThumbnailImage")
                        .WithMany()
                        .HasForeignKey("ThumbnailImageId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.Navigation("ThumbnailImage");
                });

            modelBuilder.Entity("SkyModel.Models.Product", b =>
                {
                    b.HasOne("SkyModel.Models.Category", "ObjCategory")
                        .WithMany("Products")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("SkyModel.Models.ProductDetail", "ObjDetail")
                        .WithMany()
                        .HasForeignKey("DetailId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("SkyModel.Models.MinIO", "ObjThumbnailImage")
                        .WithMany()
                        .HasForeignKey("ThumbnailImageId")
                        .OnDelete(DeleteBehavior.ClientCascade);

                    b.HasOne("SkyModel.Models.User", "ObjUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.Navigation("ObjCategory");

                    b.Navigation("ObjDetail");

                    b.Navigation("ObjThumbnailImage");

                    b.Navigation("ObjUser");
                });

            modelBuilder.Entity("SkyModel.Models.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("SkyModel.Models.ProductDetail", b =>
                {
                    b.Navigation("Images");
                });
#pragma warning restore 612, 618
        }
    }
}
