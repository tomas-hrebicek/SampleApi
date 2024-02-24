﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sample.Infrastructure.Data;

#nullable disable

namespace Sample.Infrastructure.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240211222656_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Sample.Domain.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .HasMaxLength(5000)
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImgUri")
                        .IsRequired()
                        .HasMaxLength(2048)
                        .HasColumnType("nvarchar(2048)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("nvarchar(300)");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 4)
                        .HasColumnType("decimal(18,4)");

                    b.HasKey("Id")
                        .HasName("PK_Product_Id");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ImgUri = "https://rohlik.img/",
                            Name = "Rohlík",
                            Price = 3.5m
                        },
                        new
                        {
                            Id = 2,
                            ImgUri = "https://houska.img/",
                            Name = "Houska",
                            Price = 3.5m
                        },
                        new
                        {
                            Id = 3,
                            Description = "houskový knedlík",
                            ImgUri = "https://knedlik.img/",
                            Name = "Knedlík",
                            Price = 24.9m
                        },
                        new
                        {
                            Id = 4,
                            ImgUri = "https://vanocka.img/",
                            Name = "Vánočka",
                            Price = 49.9m
                        },
                        new
                        {
                            Id = 5,
                            ImgUri = "https://pomeranc.img/",
                            Name = "Pomeranč",
                            Price = 5m
                        },
                        new
                        {
                            Id = 6,
                            ImgUri = "https://kiwi.img/",
                            Name = "Kiwi",
                            Price = 7m
                        },
                        new
                        {
                            Id = 7,
                            ImgUri = "https://syr.img/",
                            Name = "Sýr",
                            Price = 18.9m
                        },
                        new
                        {
                            Id = 8,
                            ImgUri = "https://jogurt.img/",
                            Name = "Jogurt",
                            Price = 25.9m
                        },
                        new
                        {
                            Id = 9,
                            ImgUri = "https://salam.img/",
                            Name = "Salám",
                            Price = 69.9m
                        },
                        new
                        {
                            Id = 10,
                            ImgUri = "https://mleko.img/",
                            Name = "Mléko",
                            Price = 14.9m
                        },
                        new
                        {
                            Id = 11,
                            ImgUri = "https://paprika.img/",
                            Name = "Paprika",
                            Price = 20m
                        },
                        new
                        {
                            Id = 12,
                            ImgUri = "https://patizon.img/",
                            Name = "Patizon",
                            Price = 22m
                        });
                });

            modelBuilder.Entity("Sample.Domain.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Roles")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Salt")
                        .IsRequired()
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id")
                        .HasName("PK_User_Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Active = true,
                            Password = "53C15AEBC31C921D5CEB4DF7E15E279C06606855F0D8BF8542A5D5CDEA71D74BA741BA116F8BAB4C6F40AA3076000027747522EE268B572E8411F85ABC7AF711",
                            Roles = "[1]",
                            Salt = "5D03756620AA910B167E97F8232967656A0DF57D36141C98B571F4059CEB8AB669288B3D10B8A0D49FF35C931477D02CEC89685ABB797918831E45F98A7A0DC2",
                            Username = "test"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}