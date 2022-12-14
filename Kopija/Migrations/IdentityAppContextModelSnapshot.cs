// <auto-generated />
using System;
using Kopija.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Kopija.Migrations
{
    [DbContext(typeof(IdentityAppContext))]
    partial class IdentityAppContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Kopija.Modeli.aspnetroles", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("aspnetroles");
                });

            modelBuilder.Entity("Kopija.Modeli.dostavljac", b =>
                {
                    b.Property<int>("dostavljac_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("dostavljac_adresa")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("dostavljac_ime")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("dostavljac_kontakt_osoba")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("dostavljac_kontakt_osoba_broj")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("dostavljac_kontakt_osoba_broj1")
                        .HasColumnType("longtext");

                    b.Property<string>("dostavljac_pib")
                        .HasColumnType("longtext");

                    b.Property<int>("dostavljac_sektor_id")
                        .HasColumnType("int");

                    b.HasKey("dostavljac_id");

                    b.ToTable("dostavljac");
                });

            modelBuilder.Entity("Kopija.Modeli.lokacija", b =>
                {
                    b.Property<int>("lokacija_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("lokacija_adresa")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("lokacija_ime")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("lokacija_mesto")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("lokacija_id");

                    b.ToTable("lokacija");
                });

            modelBuilder.Entity("Kopija.Modeli.na_servis", b =>
                {
                    b.Property<int>("na_servis_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("na_servis_datum")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime?>("na_servis_datum_pokupljeno")
                        .IsRequired()
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("na_servis_korisnik_id")
                        .HasColumnType("int");

                    b.Property<string>("na_servis_napomena")
                        .HasColumnType("longtext");

                    b.Property<string>("na_servis_napomena_posle")
                        .HasColumnType("longtext");

                    b.Property<string>("na_servis_opis_kvara")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("na_servis_opis_kvara_pre")
                        .HasColumnType("longtext");

                    b.Property<int?>("na_servis_oprema_id")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<int?>("na_servis_pokupio_korisnik_id")
                        .HasColumnType("int");

                    b.Property<int>("na_servis_pokupljeno")
                        .HasColumnType("int");

                    b.Property<int?>("na_servis_servis_id")
                        .IsRequired()
                        .HasColumnType("int");

                    b.HasKey("na_servis_id");

                    b.ToTable("na_servis");
                });

            modelBuilder.Entity("Kopija.Modeli.oprema", b =>
                {
                    b.Property<int>("oprema_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("oprema_cena")
                        .HasColumnType("longtext");

                    b.Property<DateTime>("oprema_datum_nabavke")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("oprema_dostavljac_id")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<DateTime>("oprema_garancija")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("oprema_kategorija_id")
                        .IsRequired()
                        .HasColumnType("int");

                    b.Property<string>("oprema_marka")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("oprema_model")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("oprema_napomena")
                        .HasColumnType("longtext");

                    b.Property<int>("oprema_otpisano")
                        .HasColumnType("int");

                    b.Property<string>("oprema_qr_kod")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("oprema_razlog_otpisa")
                        .HasColumnType("longtext");

                    b.Property<string>("oprema_serijski_broj")
                        .HasColumnType("longtext");

                    b.Property<int>("oprema_stanje")
                        .HasColumnType("int");

                    b.HasKey("oprema_id");

                    b.ToTable("oprema");
                });

            modelBuilder.Entity("Kopija.Modeli.oprema_kategorija", b =>
                {
                    b.Property<int>("oprema_kategorija_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("oprema_kategorija_ime")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("oprema_kategorija_sektor_id")
                        .HasColumnType("int");

                    b.HasKey("oprema_kategorija_id");

                    b.ToTable("oprema_kategorija");
                });

            modelBuilder.Entity("Kopija.Modeli.relokacija", b =>
                {
                    b.Property<int>("relokacija_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("relokacija_datum")
                        .HasColumnType("datetime(6)");

                    b.Property<int?>("relokacija_do_koga")
                        .HasColumnType("int");

                    b.Property<int?>("relokacija_do_lokacija_id")
                        .HasColumnType("int");

                    b.Property<int?>("relokacija_korisnik_do_koga_id")
                        .HasColumnType("int");

                    b.Property<int?>("relokacija_korisnik_od_koga_id")
                        .HasColumnType("int");

                    b.Property<string>("relokacija_napomena")
                        .HasColumnType("longtext");

                    b.Property<int?>("relokacija_od_lokacija_id")
                        .HasColumnType("int");

                    b.Property<int?>("relokacija_oprema_id")
                        .HasColumnType("int");

                    b.HasKey("relokacija_id");

                    b.ToTable("relokacija");
                });

            modelBuilder.Entity("Kopija.Modeli.sektor", b =>
                {
                    b.Property<int>("sektor_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int?>("sektor_bitan")
                        .HasColumnType("int");

                    b.Property<string>("sektor_ime")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("sektor_id");

                    b.ToTable("sektor");
                });

            modelBuilder.Entity("Kopija.Modeli.servis", b =>
                {
                    b.Property<int>("servis_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("servis_adresa")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("servis_ime")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("servis_kontakt_osoba")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("servis_kontakt_osoba_broj")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("servis_kontakt_osoba_broj1")
                        .HasColumnType("longtext");

                    b.Property<string>("servis_pib")
                        .HasColumnType("longtext");

                    b.Property<int>("servis_sektor_id")
                        .HasColumnType("int");

                    b.HasKey("servis_id");

                    b.ToTable("servis");
                });

            modelBuilder.Entity("Kopija.Models.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Kopija.Models.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("BrojTelefona")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("longtext");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Ime")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("LokacijaId")
                        .HasColumnType("int");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.Property<int>("OtpisanRadnik")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("longtext");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Prezime")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("RadnoMesto")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("longtext");

                    b.Property<int>("SektorId")
                        .HasColumnType("int");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("varchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ClaimType")
                        .HasColumnType("longtext");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("longtext");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Name")
                        .HasColumnType("varchar(255)");

                    b.Property<string>("Value")
                        .HasColumnType("longtext");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Kopija.Models.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Kopija.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Kopija.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Kopija.Models.AppRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kopija.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Kopija.Models.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
