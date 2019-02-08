﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Server;

namespace Server.Migrations
{
    [DbContext(typeof(SmartShareContext))]
    [Migration("20190206210903_UseFilenameAsPrimaryKey")]
    partial class UseFilenameAsPrimaryKey
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "2.2.1-servicing-10028")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Server.StorageModel", b =>
                {
                    b.Property<string>("Filename")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("file_name");

                    b.Property<int>("DownloadsRemaining")
                        .HasColumnName("downloads_left");

                    b.Property<string>("FileHash")
                        .HasColumnName("file_hash");

                    b.Property<string>("Password")
                        .HasColumnName("password");

                    b.Property<DateTime>("TimeCreated")
                        .HasColumnName("time_created");

                    b.Property<DateTime>("TimeExpiring")
                        .HasColumnName("time_expiring");

                    b.HasKey("Filename");

                    b.ToTable("storage");
                });
#pragma warning restore 612, 618
        }
    }
}
