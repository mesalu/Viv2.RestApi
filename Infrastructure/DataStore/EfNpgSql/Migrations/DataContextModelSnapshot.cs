﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Viv2.API.Infrastructure.DataStore.EfNpgSql.Contexts;

namespace Viv2.API.Infrastructure.DataStore.EfNpgSql.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:Collation", "en_US.UTF-8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.3")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("EnvironmentUser", b =>
                {
                    b.Property<Guid>("BackedEnvironmentsId")
                        .HasColumnType("uuid");

                    b.Property<string>("BackedUsersId")
                        .HasColumnType("text");

                    b.HasKey("BackedEnvironmentsId", "BackedUsersId");

                    b.HasIndex("BackedUsersId");

                    b.ToTable("EnvironmentUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("Viv2.API.Core.ProtoEntities.RefreshToken", b =>
                {
                    b.Property<string>("Token")
                        .HasColumnType("text");

                    b.Property<string>("AccessCapacity")
                        .HasColumnType("text");

                    b.Property<DateTime>("ExpiresAt")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("IssuedBy")
                        .HasColumnType("text");

                    b.Property<Guid>("IssuedTo")
                        .HasColumnType("uuid");

                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.HasKey("Token");

                    b.HasIndex("UserId");

                    b.ToTable("RefreshToken");
                });

            modelBuilder.Entity("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Controller", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("RealOwnerId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RealOwnerId");

                    b.ToTable("Controllers");
                });

            modelBuilder.Entity("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.EnvDataSample", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("Captured")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp without time zone")
                        .HasDefaultValueSql("(NOW() AT TIME ZONE 'utc')");

                    b.Property<double>("ColdGlass")
                        .HasColumnType("double precision");

                    b.Property<double>("ColdMat")
                        .HasColumnType("double precision");

                    b.Property<double>("HotGlass")
                        .HasColumnType("double precision");

                    b.Property<double>("HotMat")
                        .HasColumnType("double precision");

                    b.Property<double>("MidGlass")
                        .HasColumnType("double precision");

                    b.Property<Guid?>("RealEnvironmentId")
                        .HasColumnType("uuid");

                    b.Property<int?>("RealOccupantId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RealEnvironmentId");

                    b.HasIndex("RealOccupantId");

                    b.ToTable("EnvDataSamples");
                });

            modelBuilder.Entity("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Environment", b =>
                {
                    b.Property<Guid?>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Descr")
                        .HasColumnType("text");

                    b.Property<string>("Model")
                        .HasColumnType("text");

                    b.Property<Guid?>("RealControllerId")
                        .HasColumnType("uuid");

                    b.Property<int?>("RealInhabitantId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RealControllerId");

                    b.HasIndex("RealInhabitantId");

                    b.ToTable("Environments");
                });

            modelBuilder.Entity("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Pet", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime?>("HatchDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Morph")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("RealCareTakerId")
                        .HasColumnType("text");

                    b.Property<int?>("RealSpeciesId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("RealCareTakerId");

                    b.HasIndex("RealSpeciesId");

                    b.ToTable("Pets");
                });

            modelBuilder.Entity("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Species", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("DefaultLatitude")
                        .HasColumnType("double precision");

                    b.Property<double>("DefaultLongitude")
                        .HasColumnType("double precision");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("ScientificName")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Species");
                });

            modelBuilder.Entity("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("EnvironmentUser", b =>
                {
                    b.HasOne("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Environment", null)
                        .WithMany()
                        .HasForeignKey("BackedEnvironmentsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("BackedUsersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Viv2.API.Core.ProtoEntities.RefreshToken", b =>
                {
                    b.HasOne("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.User", null)
                        .WithMany("RefreshTokens")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Controller", b =>
                {
                    b.HasOne("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.User", "RealOwner")
                        .WithMany("BackedControllers")
                        .HasForeignKey("RealOwnerId");

                    b.Navigation("RealOwner");
                });

            modelBuilder.Entity("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.EnvDataSample", b =>
                {
                    b.HasOne("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Environment", "RealEnvironment")
                        .WithMany("Samples")
                        .HasForeignKey("RealEnvironmentId");

                    b.HasOne("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Pet", "RealOccupant")
                        .WithMany()
                        .HasForeignKey("RealOccupantId");

                    b.Navigation("RealEnvironment");

                    b.Navigation("RealOccupant");
                });

            modelBuilder.Entity("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Environment", b =>
                {
                    b.HasOne("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Controller", "RealController")
                        .WithMany("BackedEnvironments")
                        .HasForeignKey("RealControllerId");

                    b.HasOne("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Pet", "RealInhabitant")
                        .WithMany()
                        .HasForeignKey("RealInhabitantId");

                    b.Navigation("RealController");

                    b.Navigation("RealInhabitant");
                });

            modelBuilder.Entity("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Pet", b =>
                {
                    b.HasOne("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.User", "RealCareTaker")
                        .WithMany("BackedPets")
                        .HasForeignKey("RealCareTakerId");

                    b.HasOne("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Species", "RealSpecies")
                        .WithMany("BackedPets")
                        .HasForeignKey("RealSpeciesId");

                    b.Navigation("RealCareTaker");

                    b.Navigation("RealSpecies");
                });

            modelBuilder.Entity("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Controller", b =>
                {
                    b.Navigation("BackedEnvironments");
                });

            modelBuilder.Entity("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Environment", b =>
                {
                    b.Navigation("Samples");
                });

            modelBuilder.Entity("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.Species", b =>
                {
                    b.Navigation("BackedPets");
                });

            modelBuilder.Entity("Viv2.API.Infrastructure.DataStore.EfNpgSql.Entities.User", b =>
                {
                    b.Navigation("BackedControllers");

                    b.Navigation("BackedPets");

                    b.Navigation("RefreshTokens");
                });
#pragma warning restore 612, 618
        }
    }
}
