﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Parasale.Data;

namespace Parasale.Data.Migrations
{
    [DbContext(typeof(ParasaleDbContext))]
    [Migration("20191231073533_PagePath")]
    partial class PagePath
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.11-servicing-32099")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Parasale.Models.AppRole", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<DateTime>("CreatedDate");

                    b.Property<string>("Description");

                    b.Property<string>("Name")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Parasale.Models.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("AssginedAdmin");

                    b.Property<string>("AssignedManager");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasMaxLength(256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("IsCompanyAdmin");

                    b.Property<bool>("IsManager");

                    b.Property<DateTime>("LastNotifiedOn");

                    b.Property<int>("LastObjectionCount");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("Name");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256);

                    b.Property<string>("currentStep");

                    b.Property<string>("path");

                    b.Property<bool?>("voiceOnboarding");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("Parasale.Models.AssignedCollection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssignedAdmin");

                    b.Property<int>("CollectionId");

                    b.Property<string>("UserId");

                    b.Property<string>("appUserId");

                    b.HasKey("Id");

                    b.HasIndex("CollectionId");

                    b.HasIndex("appUserId");

                    b.ToTable("AssignedCollections");
                });

            modelBuilder.Entity("Parasale.Models.AuditLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AdminId");

                    b.Property<string>("Field");

                    b.Property<string>("ManagerId");

                    b.Property<string>("ModifiedBy");

                    b.Property<DateTime>("ModifiedDate");

                    b.Property<string>("NewData");

                    b.Property<string>("PreviousData");

                    b.Property<string>("Type");

                    b.Property<string>("UserId");

                    b.HasKey("Id");

                    b.ToTable("AuditLog");
                });

            modelBuilder.Entity("Parasale.Models.CCS", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double?>("RPA");

                    b.Property<double?>("RPAScore");

                    b.Property<double?>("RPATarget");

                    b.Property<DateTime?>("TimeStamp");

                    b.Property<double?>("TotalScore");

                    b.Property<double?>("WPMMeasure");

                    b.Property<double?>("WPMScore");

                    b.Property<double?>("WPMTarget");

                    b.Property<double?>("WPRMeasure");

                    b.Property<double?>("WPRScore");

                    b.Property<double?>("WPRTarget");

                    b.Property<string>("appUserId");

                    b.Property<int?>("objectionId");

                    b.HasKey("Id");

                    b.HasIndex("appUserId");

                    b.HasIndex("objectionId");

                    b.ToTable("CCS");
                });

            modelBuilder.Entity("Parasale.Models.Collection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssignedAdmin");

                    b.Property<string>("CollectionName")
                        .IsRequired();

                    b.Property<string>("appUserId");

                    b.HasKey("Id");

                    b.HasIndex("appUserId");

                    b.ToTable("Collections");
                });

            modelBuilder.Entity("Parasale.Models.Company", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Address");

                    b.Property<string>("CompanyName");

                    b.Property<string>("Email");

                    b.Property<string>("Phone");

                    b.Property<string>("appUserId");

                    b.HasKey("Id");

                    b.HasIndex("appUserId");

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Parasale.Models.Invites", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssignedAdmin");

                    b.Property<string>("Email");

                    b.Property<string>("Guid");

                    b.Property<bool>("IsSigned");

                    b.Property<string>("Link");

                    b.HasKey("Id");

                    b.ToTable("Invites");
                });

            modelBuilder.Entity("Parasale.Models.InvitesByManager", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssignedAdmin");

                    b.Property<string>("AssignedManager");

                    b.Property<string>("Email");

                    b.Property<string>("Guid");

                    b.Property<bool>("IsSigned");

                    b.Property<string>("Link");

                    b.HasKey("Id");

                    b.ToTable("GetInvitesByManagers");
                });

            modelBuilder.Entity("Parasale.Models.Objection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssignedAdmin");

                    b.Property<string>("BadPitchResponse");

                    b.Property<string>("InitialObjection");

                    b.Property<string>("PitchKeyword");

                    b.Property<string>("UserId");

                    b.Property<string>("ValidPitchResponse");

                    b.Property<int?>("collectionId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("collectionId");

                    b.ToTable("Objection");
                });

            modelBuilder.Entity("Parasale.Models.ObjectionLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppUserId");

                    b.Property<string>("AssignedAdmin");

                    b.Property<bool>("IsCompleted");

                    b.Property<int?>("ObjectionId");

                    b.Property<DateTime>("ObjectionTime");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.HasIndex("ObjectionId");

                    b.ToTable("ObjectionLogs");
                });

            modelBuilder.Entity("Parasale.Models.ObjectionNotification", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssignedAdmin");

                    b.Property<DateTime>("CreatedTime");

                    b.Property<bool>("IsCleared");

                    b.Property<int?>("ObjectionId");

                    b.Property<string>("PushedByUserId");

                    b.Property<string>("PushedToUserId");

                    b.HasKey("Id");

                    b.HasIndex("ObjectionId");

                    b.ToTable("ObjectionNotification");
                });

            modelBuilder.Entity("Parasale.Models.QuickCollection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssignedAdmin");

                    b.Property<string>("CollectionName")
                        .IsRequired();

                    b.Property<bool?>("QuickStart");

                    b.Property<string>("appUserId");

                    b.Property<bool>("voiceboarding");

                    b.HasKey("Id");

                    b.HasIndex("appUserId");

                    b.ToTable("qCollections");
                });

            modelBuilder.Entity("Parasale.Models.QuickStartObjections", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssignedAdmin");

                    b.Property<string>("BadPitchResponse");

                    b.Property<string>("InitialObjection");

                    b.Property<string>("PitchKeyword");

                    b.Property<string>("UserId");

                    b.Property<string>("ValidPitchResponse");

                    b.Property<int?>("collectionId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.HasIndex("collectionId");

                    b.ToTable("qObjection");
                });

            modelBuilder.Entity("Parasale.Models.SessionObjection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("cCSId");

                    b.Property<int?>("objectionId");

                    b.Property<int?>("objectionLogId");

                    b.Property<int?>("sessionId");

                    b.HasKey("Id");

                    b.HasIndex("cCSId");

                    b.HasIndex("objectionId");

                    b.HasIndex("objectionLogId");

                    b.HasIndex("sessionId");

                    b.ToTable("sessionObjections");
                });

            modelBuilder.Entity("Parasale.Models.speechToText", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AppUserId");

                    b.Property<string>("AssignedAdmin");

                    b.Property<bool>("Recieved");

                    b.Property<string>("Text");

                    b.HasKey("Id");

                    b.HasIndex("AppUserId");

                    b.ToTable("SpeechToText");
                });

            modelBuilder.Entity("Parasale.Models.UserSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime?>("SessionEnd");

                    b.Property<int>("SessionID");

                    b.Property<string>("SessionName");

                    b.Property<DateTime?>("SessionStart");

                    b.Property<string>("appUserId");

                    b.HasKey("Id");

                    b.HasIndex("appUserId");

                    b.ToTable("userSessions");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Parasale.Models.AppRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Parasale.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Parasale.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Parasale.Models.AppRole")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Parasale.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Parasale.Models.AppUser")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Parasale.Models.AssignedCollection", b =>
                {
                    b.HasOne("Parasale.Models.Collection", "collection")
                        .WithMany()
                        .HasForeignKey("CollectionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Parasale.Models.AppUser", "appUser")
                        .WithMany()
                        .HasForeignKey("appUserId");
                });

            modelBuilder.Entity("Parasale.Models.CCS", b =>
                {
                    b.HasOne("Parasale.Models.AppUser", "appUser")
                        .WithMany()
                        .HasForeignKey("appUserId");

                    b.HasOne("Parasale.Models.Objection", "objection")
                        .WithMany()
                        .HasForeignKey("objectionId");
                });

            modelBuilder.Entity("Parasale.Models.Collection", b =>
                {
                    b.HasOne("Parasale.Models.AppUser", "appUser")
                        .WithMany()
                        .HasForeignKey("appUserId");
                });

            modelBuilder.Entity("Parasale.Models.Company", b =>
                {
                    b.HasOne("Parasale.Models.AppUser", "appUser")
                        .WithMany()
                        .HasForeignKey("appUserId");
                });

            modelBuilder.Entity("Parasale.Models.Objection", b =>
                {
                    b.HasOne("Parasale.Models.AppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.HasOne("Parasale.Models.Collection", "collection")
                        .WithMany()
                        .HasForeignKey("collectionId");
                });

            modelBuilder.Entity("Parasale.Models.ObjectionLog", b =>
                {
                    b.HasOne("Parasale.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");

                    b.HasOne("Parasale.Models.Objection", "Objection")
                        .WithMany()
                        .HasForeignKey("ObjectionId");
                });

            modelBuilder.Entity("Parasale.Models.ObjectionNotification", b =>
                {
                    b.HasOne("Parasale.Models.Objection", "Objection")
                        .WithMany()
                        .HasForeignKey("ObjectionId");
                });

            modelBuilder.Entity("Parasale.Models.QuickCollection", b =>
                {
                    b.HasOne("Parasale.Models.AppUser", "appUser")
                        .WithMany()
                        .HasForeignKey("appUserId");
                });

            modelBuilder.Entity("Parasale.Models.QuickStartObjections", b =>
                {
                    b.HasOne("Parasale.Models.AppUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.HasOne("Parasale.Models.QuickCollection", "collection")
                        .WithMany()
                        .HasForeignKey("collectionId");
                });

            modelBuilder.Entity("Parasale.Models.SessionObjection", b =>
                {
                    b.HasOne("Parasale.Models.CCS", "cCS")
                        .WithMany()
                        .HasForeignKey("cCSId");

                    b.HasOne("Parasale.Models.Objection", "objection")
                        .WithMany()
                        .HasForeignKey("objectionId");

                    b.HasOne("Parasale.Models.ObjectionLog", "objectionLog")
                        .WithMany()
                        .HasForeignKey("objectionLogId");

                    b.HasOne("Parasale.Models.UserSession", "session")
                        .WithMany()
                        .HasForeignKey("sessionId");
                });

            modelBuilder.Entity("Parasale.Models.speechToText", b =>
                {
                    b.HasOne("Parasale.Models.AppUser", "AppUser")
                        .WithMany()
                        .HasForeignKey("AppUserId");
                });

            modelBuilder.Entity("Parasale.Models.UserSession", b =>
                {
                    b.HasOne("Parasale.Models.AppUser", "appUser")
                        .WithMany()
                        .HasForeignKey("appUserId");
                });
#pragma warning restore 612, 618
        }
    }
}
