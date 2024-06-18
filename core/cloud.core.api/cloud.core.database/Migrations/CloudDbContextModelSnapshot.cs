﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using cloud.core.database.DbContexts;

#nullable disable

namespace cloud.core.database.Migrations
{
    [DbContext(typeof(CloudDbContext))]
    partial class CloudDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("cloud.core.objects.Database.DbSubscription", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<double>("MaximmumSpace")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("subscriptions", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            MaximmumSpace = 1000000.0,
                            Name = "Test subscription"
                        });
                });

            modelBuilder.Entity("cloud.core.objects.Database.DbUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("RefreshToken")
                        .HasColumnType("text");

                    b.Property<int>("StatusId")
                        .HasColumnType("integer");

                    b.Property<int>("SubscriptionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("SubscriptionId");

                    b.ToTable("users", (string)null);
                });

            modelBuilder.Entity("cloud.core.objects.Database.DbUserFilesData", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.Property<int>("FileSaved")
                        .HasColumnType("integer");

                    b.Property<double>("SpaceUsed")
                        .HasColumnType("double precision");

                    b.HasKey("UserId");

                    b.ToTable("user_files_data", (string)null);
                });

            modelBuilder.Entity("cloud.core.objects.Database.DbUser", b =>
                {
                    b.HasOne("cloud.core.objects.Database.DbSubscription", "Subscription")
                        .WithMany("Users")
                        .HasForeignKey("SubscriptionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Subscription");
                });

            modelBuilder.Entity("cloud.core.objects.Database.DbUserFilesData", b =>
                {
                    b.HasOne("cloud.core.objects.Database.DbUser", "User")
                        .WithOne("Data")
                        .HasForeignKey("cloud.core.objects.Database.DbUserFilesData", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("cloud.core.objects.Database.DbSubscription", b =>
                {
                    b.Navigation("Users");
                });

            modelBuilder.Entity("cloud.core.objects.Database.DbUser", b =>
                {
                    b.Navigation("Data")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
