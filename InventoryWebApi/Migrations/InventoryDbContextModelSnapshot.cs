﻿// <auto-generated />
using InventoryAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace InventoryAPI.Migrations
{
    [DbContext(typeof(InventoryDbContext))]
    partial class InventoryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.15")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("InventoryAPI.Models.Brand", b =>
                {
                    b.Property<int>("brandAutoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("brandId")
                        .HasColumnType("longtext");

                    b.Property<string>("brandName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("companyId")
                        .HasColumnType("longtext");

                    b.Property<string>("status")
                        .HasColumnType("longtext");

                    b.HasKey("brandAutoId");

                    b.ToTable("brands");
                });

            modelBuilder.Entity("InventoryAPI.Models.Category", b =>
                {
                    b.Property<int>("catAutoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("catId")
                        .HasColumnType("longtext");

                    b.Property<string>("catName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("companyId")
                        .HasColumnType("longtext");

                    b.Property<int>("groupAutoId")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .HasColumnType("longtext");

                    b.HasKey("catAutoId");

                    b.HasIndex("groupAutoId");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("InventoryAPI.Models.Equipment", b =>
                {
                    b.Property<int>("equipAutoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("catAutoId")
                        .HasColumnType("int");

                    b.Property<string>("companyId")
                        .HasColumnType("longtext");

                    b.Property<string>("equipCost")
                        .HasColumnType("longtext");

                    b.Property<string>("equipId")
                        .HasColumnType("longtext");

                    b.Property<string>("equipLeadTime")
                        .HasColumnType("longtext");

                    b.Property<string>("equipName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("groupAutoId")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .HasColumnType("longtext");

                    b.HasKey("equipAutoId");

                    b.HasIndex("catAutoId");

                    b.HasIndex("groupAutoId");

                    b.ToTable("equipments");
                });

            modelBuilder.Entity("InventoryAPI.Models.Group", b =>
                {
                    b.Property<int>("groupAutoID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("companyId")
                        .HasColumnType("longtext");

                    b.Property<string>("groupID")
                        .HasColumnType("longtext");

                    b.Property<string>("groupName")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("status")
                        .HasColumnType("longtext");

                    b.HasKey("groupAutoID");

                    b.ToTable("Inventorygroups");
                });

            modelBuilder.Entity("InventoryAPI.Models.Issuence", b =>
                {
                    b.Property<int>("issuenceAutoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("companyId")
                        .HasColumnType("longtext");

                    b.Property<string>("issuenceDescp")
                        .HasColumnType("longtext");

                    b.Property<string>("issuenceId")
                        .HasColumnType("longtext");

                    b.Property<int>("qty")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .HasColumnType("longtext");

                    b.Property<int>("userAutoId")
                        .HasColumnType("int");

                    b.HasKey("issuenceAutoId");

                    b.ToTable("issuences");
                });

            modelBuilder.Entity("InventoryAPI.Models.IssuenceandEquipment", b =>
                {
                    b.Property<int>("issueEquipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("companyId")
                        .HasColumnType("longtext");

                    b.Property<int>("equipAutoId")
                        .HasColumnType("int");

                    b.Property<int>("issuenceAutoId")
                        .HasColumnType("int");

                    b.HasKey("issueEquipId");

                    b.HasIndex("equipAutoId");

                    b.HasIndex("issuenceAutoId");

                    b.ToTable("issuenceandEquipment");
                });

            modelBuilder.Entity("InventoryAPI.Models.Purchase", b =>
                {
                    b.Property<int>("purchaseAutoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("companyId")
                        .HasColumnType("longtext");

                    b.Property<string>("purchaseId")
                        .HasColumnType("longtext");

                    b.Property<string>("purchasesDescp")
                        .HasColumnType("longtext");

                    b.Property<int>("qty")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .HasColumnType("longtext");

                    b.Property<int>("userAutoId")
                        .HasColumnType("int");

                    b.HasKey("purchaseAutoId");

                    b.ToTable("purchases");
                });

            modelBuilder.Entity("InventoryAPI.Models.PurchaseandEquipment", b =>
                {
                    b.Property<int>("purchaseEquipId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("companyId")
                        .HasColumnType("longtext");

                    b.Property<int>("equipAutoId")
                        .HasColumnType("int");

                    b.Property<int>("purchaseAutoId")
                        .HasColumnType("int");

                    b.HasKey("purchaseEquipId");

                    b.HasIndex("equipAutoId");

                    b.HasIndex("purchaseAutoId");

                    b.ToTable("purchaseandEquipment");
                });

            modelBuilder.Entity("InventoryAPI.Models.UnitOfMeasurement", b =>
                {
                    b.Property<int>("uomAutoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("companyId")
                        .HasColumnType("longtext");

                    b.Property<string>("status")
                        .HasColumnType("longtext");

                    b.Property<string>("uomId")
                        .HasColumnType("longtext");

                    b.Property<string>("uomName")
                        .HasColumnType("longtext");

                    b.HasKey("uomAutoId");

                    b.ToTable("unitOfMeasurements");
                });

            modelBuilder.Entity("InventoryAPI.Models.Category", b =>
                {
                    b.HasOne("InventoryAPI.Models.Group", "groups")
                        .WithMany()
                        .HasForeignKey("groupAutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("groups");
                });

            modelBuilder.Entity("InventoryAPI.Models.Equipment", b =>
                {
                    b.HasOne("InventoryAPI.Models.Category", "categories")
                        .WithMany()
                        .HasForeignKey("catAutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventoryAPI.Models.Group", "groups")
                        .WithMany()
                        .HasForeignKey("groupAutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("categories");

                    b.Navigation("groups");
                });

            modelBuilder.Entity("InventoryAPI.Models.IssuenceandEquipment", b =>
                {
                    b.HasOne("InventoryAPI.Models.Equipment", "Equipment")
                        .WithMany()
                        .HasForeignKey("equipAutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventoryAPI.Models.Issuence", "Issuence")
                        .WithMany()
                        .HasForeignKey("issuenceAutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("Issuence");
                });

            modelBuilder.Entity("InventoryAPI.Models.PurchaseandEquipment", b =>
                {
                    b.HasOne("InventoryAPI.Models.Equipment", "Equipment")
                        .WithMany()
                        .HasForeignKey("equipAutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("InventoryAPI.Models.Purchase", "Purchase")
                        .WithMany()
                        .HasForeignKey("purchaseAutoId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Equipment");

                    b.Navigation("Purchase");
                });
#pragma warning restore 612, 618
        }
    }
}
