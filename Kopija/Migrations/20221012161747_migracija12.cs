using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Kopija.Migrations
{
    public partial class migracija12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "relokacija_do_koga_id",
                table: "relokacija",
                newName: "relokacija_korisnik_do_koga_id");

            migrationBuilder.RenameColumn(
                name: "na_servis_pokupio_korisnik",
                table: "na_servis",
                newName: "na_servis_pokupio_korisnik_id");

            migrationBuilder.RenameColumn(
                name: "lokacija_broj_objekta",
                table: "lokacija",
                newName: "lokacija_mesto");

            migrationBuilder.AddColumn<int>(
                name: "servis_sektor_id",
                table: "servis",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "sektor_bitan",
                table: "sektor",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "relokacija_do_koga",
                table: "relokacija",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "relokacija_napomena",
                table: "relokacija",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "oprema_cena",
                table: "oprema",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "oprema_napomena",
                table: "oprema",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "oprema_serijski_broj",
                table: "oprema",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "na_servis_napomena",
                table: "na_servis",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "na_servis_korisnik_id",
                table: "na_servis",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "na_servis_datum_pokupljeno",
                table: "na_servis",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "na_servis_napomena_posle",
                table: "na_servis",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "na_servis_opis_kvara_pre",
                table: "na_servis",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "dostavljac_sektor_id",
                table: "dostavljac",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "servis_sektor_id",
                table: "servis");

            migrationBuilder.DropColumn(
                name: "sektor_bitan",
                table: "sektor");

            migrationBuilder.DropColumn(
                name: "relokacija_do_koga",
                table: "relokacija");

            migrationBuilder.DropColumn(
                name: "relokacija_napomena",
                table: "relokacija");

            migrationBuilder.DropColumn(
                name: "oprema_napomena",
                table: "oprema");

            migrationBuilder.DropColumn(
                name: "oprema_serijski_broj",
                table: "oprema");

            migrationBuilder.DropColumn(
                name: "na_servis_napomena_posle",
                table: "na_servis");

            migrationBuilder.DropColumn(
                name: "na_servis_opis_kvara_pre",
                table: "na_servis");

            migrationBuilder.DropColumn(
                name: "dostavljac_sektor_id",
                table: "dostavljac");

            migrationBuilder.RenameColumn(
                name: "relokacija_korisnik_do_koga_id",
                table: "relokacija",
                newName: "relokacija_do_koga_id");

            migrationBuilder.RenameColumn(
                name: "na_servis_pokupio_korisnik_id",
                table: "na_servis",
                newName: "na_servis_pokupio_korisnik");

            migrationBuilder.RenameColumn(
                name: "lokacija_mesto",
                table: "lokacija",
                newName: "lokacija_broj_objekta");

            migrationBuilder.UpdateData(
                table: "oprema",
                keyColumn: "oprema_cena",
                keyValue: null,
                column: "oprema_cena",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "oprema_cena",
                table: "oprema",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "na_servis",
                keyColumn: "na_servis_napomena",
                keyValue: null,
                column: "na_servis_napomena",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "na_servis_napomena",
                table: "na_servis",
                type: "longtext",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<int>(
                name: "na_servis_korisnik_id",
                table: "na_servis",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "na_servis_datum_pokupljeno",
                table: "na_servis",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)");
        }
    }
}
