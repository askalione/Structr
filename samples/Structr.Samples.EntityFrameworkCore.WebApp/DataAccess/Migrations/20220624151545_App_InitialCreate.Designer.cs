﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Structr.Samples.EntityFrameworkCore.WebApp.DataAccess;

#nullable disable

namespace Structr.Samples.EntityFrameworkCore.WebApp.DataAccess.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20220624151545_App_InitialCreate")]
    partial class App_InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Structr.Samples.EntityFrameworkCore.WebApp.Domain.Issues.Issue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProjectId");

                    b.ToTable("Issues", (string)null);
                });

            modelBuilder.Entity("Structr.Samples.EntityFrameworkCore.WebApp.Domain.Projects.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Projects", (string)null);
                });

            modelBuilder.Entity("Structr.Samples.EntityFrameworkCore.WebApp.Domain.Users.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);
                });

            modelBuilder.Entity("Structr.Samples.EntityFrameworkCore.WebApp.Domain.Issues.Issue", b =>
                {
                    b.HasOne("Structr.Samples.EntityFrameworkCore.WebApp.Domain.Projects.Project", "Project")
                        .WithMany()
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.OwnsOne("Structr.Samples.EntityFrameworkCore.WebApp.Domain.Multilang", "Description", b1 =>
                        {
                            b1.Property<int>("IssueId")
                                .HasColumnType("int");

                            b1.Property<string>("En")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("DescriptionEn");

                            b1.Property<string>("Ru")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("DescriptionRu");

                            b1.HasKey("IssueId");

                            b1.ToTable("Issues");

                            b1.WithOwner()
                                .HasForeignKey("IssueId");
                        });

                    b.Navigation("Description")
                        .IsRequired();

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Structr.Samples.EntityFrameworkCore.WebApp.Domain.Projects.Project", b =>
                {
                    b.OwnsOne("Structr.Samples.EntityFrameworkCore.WebApp.Domain.Multilang", "Name", b1 =>
                        {
                            b1.Property<int>("ProjectId")
                                .HasColumnType("int");

                            b1.Property<string>("En")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("NameEn");

                            b1.Property<string>("Ru")
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("NameRu");

                            b1.HasKey("ProjectId");

                            b1.ToTable("Projects");

                            b1.WithOwner()
                                .HasForeignKey("ProjectId");
                        });

                    b.Navigation("Name")
                        .IsRequired();
                });

            modelBuilder.Entity("Structr.Samples.EntityFrameworkCore.WebApp.Domain.Users.User", b =>
                {
                    b.OwnsOne("Structr.Samples.EntityFrameworkCore.WebApp.Domain.Users.UserIdentity", "Identity", b1 =>
                        {
                            b1.Property<int>("UserId")
                                .HasColumnType("int");

                            b1.Property<DateTime>("DateOfBirth")
                                .HasColumnType("datetime2")
                                .HasColumnName("DateOfBirth");

                            b1.Property<string>("FirstName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("FirstName");

                            b1.Property<bool>("Gender")
                                .HasColumnType("BIT")
                                .HasColumnName("Gender");

                            b1.Property<string>("LastName")
                                .IsRequired()
                                .HasColumnType("nvarchar(max)")
                                .HasColumnName("LastName");

                            b1.HasKey("UserId");

                            b1.ToTable("Users");

                            b1.WithOwner()
                                .HasForeignKey("UserId");
                        });

                    b.Navigation("Identity")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
