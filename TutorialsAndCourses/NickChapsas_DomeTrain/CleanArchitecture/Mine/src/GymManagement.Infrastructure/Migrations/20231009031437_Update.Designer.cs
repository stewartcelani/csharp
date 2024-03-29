﻿// <auto-generated />
using System;
using GymManagement.Infrastructure.Common.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GymManagement.Infrastructure.Migrations
{
    [DbContext(typeof(GymManagementDbContext))]
    [Migration("20231009031437_Update")]
    partial class Update
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.11");

            modelBuilder.Entity("GymManagement.Domain.Subscriptions.SubscriptionEntity", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("AdminId")
                        .HasColumnType("TEXT");

                    b.Property<int>("SubscriptionType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Subscriptions");
                });
#pragma warning restore 612, 618
        }
    }
}
