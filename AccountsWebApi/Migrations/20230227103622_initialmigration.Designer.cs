﻿// <auto-generated />
using AccountsWebApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AccountsWebApi.Migrations
{
    [DbContext(typeof(UserDbContext))]
    [Migration("20230227103622_initialmigration")]
    partial class initialmigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("AccountsWebApi.Models.Company", b =>
                {
                    b.Property<string>("companyid")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("companyemail")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("companyname")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.Property<string>("companyphone")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("password")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("status")
                        .HasColumnType("longtext");

                    b.Property<string>("userfirstname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("userlastname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("companyid");

                    b.HasIndex("companyemail")
                        .IsUnique();

                    b.HasIndex("companyname")
                        .IsUnique();

                    b.ToTable("companies");
                });

            modelBuilder.Entity("AccountsWebApi.Models.Department", b =>
                {
                    b.Property<int>("deptautoid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("companyid")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("deptid")
                        .HasColumnType("longtext");

                    b.Property<string>("deptname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("status")
                        .HasColumnType("longtext");

                    b.HasKey("deptautoid");

                    b.HasIndex("companyid");

                    b.ToTable("departments");
                });

            modelBuilder.Entity("AccountsWebApi.Models.Employee", b =>
                {
                    b.Property<int>("employeeautoid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("companyid")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("deptautoid")
                        .HasColumnType("int");

                    b.Property<string>("employeecontactno")
                        .HasColumnType("longtext");

                    b.Property<string>("employeedesignation")
                        .HasColumnType("longtext");

                    b.Property<string>("employeeemail")
                        .HasColumnType("longtext");

                    b.Property<string>("employeefathername")
                        .HasColumnType("longtext");

                    b.Property<string>("employeeid")
                        .HasColumnType("longtext");

                    b.Property<string>("employeename")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("status")
                        .HasColumnType("longtext");

                    b.HasKey("employeeautoid");

                    b.HasIndex("companyid");

                    b.HasIndex("deptautoid");

                    b.ToTable("employees");
                });

            modelBuilder.Entity("AccountsWebApi.Models.Permission", b =>
                {
                    b.Property<string>("permissionid")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("permissionname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("status")
                        .HasColumnType("longtext");

                    b.HasKey("permissionid");

                    b.ToTable("permissions");
                });

            modelBuilder.Entity("AccountsWebApi.Models.Role", b =>
                {
                    b.Property<int>("roleautoid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("companyid")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("roleid")
                        .HasColumnType("longtext");

                    b.Property<string>("rolename")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("status")
                        .HasColumnType("longtext");

                    b.HasKey("roleautoid");

                    b.HasIndex("companyid");

                    b.ToTable("roles");
                });

            modelBuilder.Entity("AccountsWebApi.Models.RoleandDepartment", b =>
                {
                    b.Property<int>("roledeptid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("companyid")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("deptautoid")
                        .HasColumnType("int");

                    b.Property<int>("roleautoid")
                        .HasColumnType("int");

                    b.HasKey("roledeptid");

                    b.HasIndex("companyid");

                    b.HasIndex("deptautoid");

                    b.HasIndex("roleautoid");

                    b.ToTable("roleanddepartments");
                });

            modelBuilder.Entity("AccountsWebApi.Models.RoleandPermission", b =>
                {
                    b.Property<int>("rolepermissionid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("companyid")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("permissionid")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("roleautoid")
                        .HasColumnType("int");

                    b.HasKey("rolepermissionid");

                    b.HasIndex("companyid");

                    b.HasIndex("permissionid");

                    b.HasIndex("roleautoid");

                    b.ToTable("roleandpermissions");
                });

            modelBuilder.Entity("AccountsWebApi.Models.RoleandUser", b =>
                {
                    b.Property<int>("roleuserid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("companyid")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("roleautoid")
                        .HasColumnType("int");

                    b.Property<int>("userautoid")
                        .HasColumnType("int");

                    b.HasKey("roleuserid");

                    b.HasIndex("companyid");

                    b.HasIndex("roleautoid");

                    b.HasIndex("userautoid");

                    b.ToTable("userandroles");
                });

            modelBuilder.Entity("AccountsWebApi.Models.SubDepartment", b =>
                {
                    b.Property<int>("subdeptautoid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("companyid")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("deptautoid")
                        .HasColumnType("int");

                    b.Property<string>("status")
                        .HasColumnType("longtext");

                    b.Property<string>("subdeptid")
                        .HasColumnType("longtext");

                    b.Property<string>("subdeptname")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("subdeptautoid");

                    b.HasIndex("companyid");

                    b.HasIndex("deptautoid");

                    b.ToTable("sub_departments");
                });

            modelBuilder.Entity("AccountsWebApi.Models.User", b =>
                {
                    b.Property<int>("userautoid")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("companyid")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("deptautoid")
                        .HasColumnType("int");

                    b.Property<int>("employeeautoid")
                        .HasColumnType("int");

                    b.Property<string>("password")
                        .HasColumnType("longtext");

                    b.Property<string>("role")
                        .HasColumnType("longtext");

                    b.Property<string>("status")
                        .HasColumnType("longtext");

                    b.Property<string>("userid")
                        .HasColumnType("longtext");

                    b.Property<string>("username")
                        .IsRequired()
                        .HasColumnType("varchar(255)");

                    b.HasKey("userautoid");

                    b.HasIndex("companyid");

                    b.HasIndex("deptautoid");

                    b.HasIndex("employeeautoid");

                    b.HasIndex("username")
                        .IsUnique();

                    b.ToTable("users");
                });

            modelBuilder.Entity("AccountsWebApi.Models.Department", b =>
                {
                    b.HasOne("AccountsWebApi.Models.Company", "companies")
                        .WithMany()
                        .HasForeignKey("companyid");

                    b.Navigation("companies");
                });

            modelBuilder.Entity("AccountsWebApi.Models.Employee", b =>
                {
                    b.HasOne("AccountsWebApi.Models.Company", "companies")
                        .WithMany()
                        .HasForeignKey("companyid");

                    b.HasOne("AccountsWebApi.Models.Department", "department")
                        .WithMany()
                        .HasForeignKey("deptautoid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("companies");

                    b.Navigation("department");
                });

            modelBuilder.Entity("AccountsWebApi.Models.Role", b =>
                {
                    b.HasOne("AccountsWebApi.Models.Company", "companies")
                        .WithMany()
                        .HasForeignKey("companyid");

                    b.Navigation("companies");
                });

            modelBuilder.Entity("AccountsWebApi.Models.RoleandDepartment", b =>
                {
                    b.HasOne("AccountsWebApi.Models.Company", "companies")
                        .WithMany()
                        .HasForeignKey("companyid");

                    b.HasOne("AccountsWebApi.Models.Department", "department")
                        .WithMany()
                        .HasForeignKey("deptautoid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccountsWebApi.Models.Role", "role")
                        .WithMany()
                        .HasForeignKey("roleautoid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("companies");

                    b.Navigation("department");

                    b.Navigation("role");
                });

            modelBuilder.Entity("AccountsWebApi.Models.RoleandPermission", b =>
                {
                    b.HasOne("AccountsWebApi.Models.Company", "companies")
                        .WithMany()
                        .HasForeignKey("companyid");

                    b.HasOne("AccountsWebApi.Models.Permission", "permission")
                        .WithMany()
                        .HasForeignKey("permissionid");

                    b.HasOne("AccountsWebApi.Models.Role", "role")
                        .WithMany()
                        .HasForeignKey("roleautoid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("companies");

                    b.Navigation("permission");

                    b.Navigation("role");
                });

            modelBuilder.Entity("AccountsWebApi.Models.RoleandUser", b =>
                {
                    b.HasOne("AccountsWebApi.Models.Company", "companies")
                        .WithMany()
                        .HasForeignKey("companyid");

                    b.HasOne("AccountsWebApi.Models.Role", "role")
                        .WithMany()
                        .HasForeignKey("roleautoid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccountsWebApi.Models.User", "user")
                        .WithMany()
                        .HasForeignKey("userautoid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("companies");

                    b.Navigation("role");

                    b.Navigation("user");
                });

            modelBuilder.Entity("AccountsWebApi.Models.SubDepartment", b =>
                {
                    b.HasOne("AccountsWebApi.Models.Company", "companies")
                        .WithMany()
                        .HasForeignKey("companyid");

                    b.HasOne("AccountsWebApi.Models.Department", "department")
                        .WithMany()
                        .HasForeignKey("deptautoid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("companies");

                    b.Navigation("department");
                });

            modelBuilder.Entity("AccountsWebApi.Models.User", b =>
                {
                    b.HasOne("AccountsWebApi.Models.Company", "companies")
                        .WithMany()
                        .HasForeignKey("companyid");

                    b.HasOne("AccountsWebApi.Models.Department", "department")
                        .WithMany()
                        .HasForeignKey("deptautoid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AccountsWebApi.Models.Employee", "employee")
                        .WithMany()
                        .HasForeignKey("employeeautoid")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("companies");

                    b.Navigation("department");

                    b.Navigation("employee");
                });
#pragma warning restore 612, 618
        }
    }
}
