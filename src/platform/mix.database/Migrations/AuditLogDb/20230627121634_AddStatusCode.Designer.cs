﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.Entities.AuditLog;

#nullable disable

namespace Mix.Database.Migrations.AuditLogDb
{
    [DbContext(typeof(AuditLogDbContext))]
    [Migration("20230627121634_AddStatusCode")]
    partial class AddStatusCode
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.5");

            modelBuilder.Entity("Mix.Database.Entities.AuditLog.AuditLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Body")
                        .HasColumnType("ntext");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("varchar(250)");

                    b.Property<DateTime>("CreatedDateTime")
                        .HasColumnType("datetime");

                    b.Property<string>("Endpoint")
                        .HasColumnType("TEXT");

                    b.Property<string>("Exception")
                        .HasColumnType("ntext");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("boolean");

                    b.Property<DateTime?>("LastModified")
                        .HasColumnType("datetime");

                    b.Property<string>("Method")
                        .HasColumnType("TEXT");

                    b.Property<string>("ModifiedBy")
                        .HasColumnType("varchar(250)");

                    b.Property<int>("Priority")
                        .HasColumnType("integer");

                    b.Property<string>("QueryString")
                        .HasColumnType("TEXT");

                    b.Property<string>("RequestIp")
                        .HasColumnType("TEXT");

                    b.Property<string>("Response")
                        .HasColumnType("ntext");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar(50)")
                        .HasAnnotation("MySql:CharSet", "utf8");

                    b.Property<int>("StatusCode")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("Success")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("AuditLog");
                });
#pragma warning restore 612, 618
        }
    }
}