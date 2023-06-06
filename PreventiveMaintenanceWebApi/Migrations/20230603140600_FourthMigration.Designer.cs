﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PreventiveMaintenanceWebApi.Models;

#nullable disable

namespace PreventiveMaintenanceWebApi.Migrations
{
    [DbContext(typeof(PreventiveMaintenanceDbContext))]
    [Migration("20230603140600_FourthMigration")]
    partial class FourthMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PreventiveMaintenanceWebApi.Models.Inspection", b =>
                {
                    b.Property<int>("inspectionAutoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("assetId")
                        .HasColumnType("longtext");

                    b.Property<int>("assetModelId")
                        .HasColumnType("int");

                    b.Property<string>("companyId")
                        .HasColumnType("longtext");

                    b.Property<string>("eventIdCalendar")
                        .HasColumnType("longtext");

                    b.Property<int>("frequencyDays")
                        .HasColumnType("int");

                    b.Property<DateTime>("initialDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("options")
                        .HasColumnType("longtext");

                    b.Property<string>("question")
                        .HasColumnType("longtext");

                    b.Property<string>("workRequestOfOptions")
                        .HasColumnType("longtext");

                    b.HasKey("inspectionAutoId");

                    b.ToTable("inspections");
                });

            modelBuilder.Entity("PreventiveMaintenanceWebApi.Models.InspectionEntry", b =>
                {
                    b.Property<int>("inspectionEntryAutoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("assetId")
                        .HasColumnType("longtext");

                    b.Property<int>("assetModelId")
                        .HasColumnType("int");

                    b.Property<string>("companyId")
                        .HasColumnType("longtext");

                    b.Property<string>("question")
                        .HasColumnType("longtext");

                    b.Property<string>("remarks")
                        .HasColumnType("longtext");

                    b.Property<string>("selectedOption")
                        .HasColumnType("longtext");

                    b.HasKey("inspectionEntryAutoId");

                    b.ToTable("inspectionEntries");
                });

            modelBuilder.Entity("PreventiveMaintenanceWebApi.Models.MeterReading", b =>
                {
                    b.Property<int>("mrAutoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("assetId")
                        .HasColumnType("longtext");

                    b.Property<int>("assetModelId")
                        .HasColumnType("int");

                    b.Property<string>("companyId")
                        .HasColumnType("longtext");

                    b.Property<string>("eventIdCalendar")
                        .HasColumnType("longtext");

                    b.Property<int>("frequencyDays")
                        .HasColumnType("int");

                    b.Property<DateTime>("initialDate")
                        .HasColumnType("datetime(6)");

                    b.Property<double>("maxValue")
                        .HasColumnType("double");

                    b.Property<double>("minValue")
                        .HasColumnType("double");

                    b.Property<string>("paramName")
                        .HasColumnType("longtext");

                    b.Property<string>("paramType")
                        .HasColumnType("longtext");

                    b.HasKey("mrAutoId");

                    b.ToTable("meterReadings");
                });

            modelBuilder.Entity("PreventiveMaintenanceWebApi.Models.MeterReadingEntry", b =>
                {
                    b.Property<int>("mreAutoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("assetId")
                        .HasColumnType("longtext");

                    b.Property<int>("assetModelId")
                        .HasColumnType("int");

                    b.Property<string>("companyId")
                        .HasColumnType("longtext");

                    b.Property<string>("paramName")
                        .HasColumnType("longtext");

                    b.Property<string>("remarks")
                        .HasColumnType("longtext");

                    b.Property<double>("value")
                        .HasColumnType("double");

                    b.HasKey("mreAutoId");

                    b.ToTable("meterReadingEntries");
                });

            modelBuilder.Entity("PreventiveMaintenanceWebApi.Models.ScheduledWorkRequest", b =>
                {
                    b.Property<int>("swrAutoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("assetId")
                        .HasColumnType("longtext");

                    b.Property<int>("assetModelId")
                        .HasColumnType("int");

                    b.Property<string>("companyId")
                        .HasColumnType("longtext");

                    b.Property<string>("description")
                        .HasColumnType("longtext");

                    b.Property<string>("eventIdCalendar")
                        .HasColumnType("longtext");

                    b.Property<int>("frequencyDays")
                        .HasColumnType("int");

                    b.Property<string>("headOfProblem")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("initialDateTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("swrAutoId");

                    b.ToTable("scheduledWorkRequests");
                });
#pragma warning restore 612, 618
        }
    }
}
