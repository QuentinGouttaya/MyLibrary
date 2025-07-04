﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BookApi.Migrations
{
    [DbContext(typeof(MediaDb))]
    [Migration("20250610135352_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("Models.Media", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Author")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Author");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Title");

                    b.HasKey("Id");

                    b.ToTable("Media");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Media");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("Models.Ebook.Ebook", b =>
                {
                    b.HasBaseType("Models.Media");

                    b.ToTable("Media");

                    b.HasDiscriminator().HasValue("Ebook");
                });

            modelBuilder.Entity("Models.Paperbook.PaperBook", b =>
                {
                    b.HasBaseType("Models.Media");

                    b.ToTable("Media");

                    b.HasDiscriminator().HasValue("PaperBook");
                });
#pragma warning restore 612, 618
        }
    }
}
