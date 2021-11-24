﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using StatusAppBackend.Database;

#nullable disable

namespace StatusAppBackend.Migrations
{
    [DbContext(typeof(StatusAppContext))]
    [Migration("20211124174233_UserDeleteBehaviour")]
    partial class UserDeleteBehaviour
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("StatusAppBackend.Database.Model.Service", b =>
                {
                    b.Property<int>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Key"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Url")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Key");

                    b.ToTable("Services");

                    b.HasData(
                        new
                        {
                            Key = 1,
                            Name = "Steam Server Info",
                            Url = "https://api.steampowered.com/ISteamWebAPIUtil/GetServerInfo/v1/"
                        },
                        new
                        {
                            Key = 2,
                            Name = "GitHub API",
                            Url = "https://api.github.com"
                        });
                });

            modelBuilder.Entity("StatusAppBackend.Database.Model.ServiceInformation", b =>
                {
                    b.Property<int>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Key"));

                    b.Property<double>("ResponseTime")
                        .HasColumnType("double precision");

                    b.Property<int>("ServiceKey")
                        .HasColumnType("integer");

                    b.Property<int>("StatusCode")
                        .HasColumnType("integer");

                    b.Property<DateTime>("TimeRequested")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Key");

                    b.HasIndex("ServiceKey");

                    b.ToTable("ServiceInformations");
                });

            modelBuilder.Entity("StatusAppBackend.Database.Model.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("integer");

                    b.Property<byte[]>("Hash")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("bytea");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("CreatedUserId")
                        .IsUnique();

                    b.ToTable("Users");
                });

            modelBuilder.Entity("StatusAppBackend.Database.Model.UserCreationToken", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int?>("CreatedUserId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("IssuedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int?>("IssuerId")
                        .HasColumnType("integer");

                    b.Property<string>("Token")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("IssuerId");

                    b.ToTable("UserCreationTokens");
                });

            modelBuilder.Entity("StatusAppBackend.Database.Model.ServiceInformation", b =>
                {
                    b.HasOne("StatusAppBackend.Database.Model.Service", "Service")
                        .WithMany()
                        .HasForeignKey("ServiceKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");
                });

            modelBuilder.Entity("StatusAppBackend.Database.Model.User", b =>
                {
                    b.HasOne("StatusAppBackend.Database.Model.UserCreationToken", null)
                        .WithOne("CreatedUser")
                        .HasForeignKey("StatusAppBackend.Database.Model.User", "CreatedUserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("StatusAppBackend.Database.Model.UserCreationToken", b =>
                {
                    b.HasOne("StatusAppBackend.Database.Model.User", "Issuer")
                        .WithMany()
                        .HasForeignKey("IssuerId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Issuer");
                });

            modelBuilder.Entity("StatusAppBackend.Database.Model.UserCreationToken", b =>
                {
                    b.Navigation("CreatedUser");
                });
#pragma warning restore 612, 618
        }
    }
}
