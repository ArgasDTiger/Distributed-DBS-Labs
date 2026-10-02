using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Producer.Migrations
{
    /// <inheritdoc />
    public partial class AddSyncCursor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "title",
                table: "Book",
                newName: "Title");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Book",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "isbn",
                table: "Book",
                newName: "Isbn");

            migrationBuilder.RenameColumn(
                name: "genre",
                table: "Book",
                newName: "Genre");

            migrationBuilder.RenameColumn(
                name: "author",
                table: "Book",
                newName: "Author");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Book",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updated_at",
                table: "Book",
                newName: "UpdatedAt");

            migrationBuilder.RenameColumn(
                name: "published_year",
                table: "Book",
                newName: "PublishedYear");

            migrationBuilder.RenameColumn(
                name: "is_available",
                table: "Book",
                newName: "IsAvailable");

            migrationBuilder.RenameIndex(
                name: "IX_Book_isbn",
                table: "Book",
                newName: "IX_Book_Isbn");

            migrationBuilder.RenameIndex(
                name: "IX_Book_updated_at",
                table: "Book",
                newName: "IX_Book_UpdatedAt");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Book",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(300)");

            migrationBuilder.AlterColumn<string>(
                name: "Isbn",
                table: "Book",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)");

            migrationBuilder.AlterColumn<string>(
                name: "Genre",
                table: "Book",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "Author",
                table: "Book",
                type: "character varying(300)",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(300)");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "UpdatedAt",
                table: "Book",
                type: "timestamp with time zone",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamptz");

            migrationBuilder.CreateTable(
                name: "SyncCursor",
                columns: table => new
                {
                    Id = table.Column<byte>(type: "smallint", nullable: false),
                    LastProcessedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyncCursor", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SyncCursor");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Book",
                newName: "title");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Book",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Isbn",
                table: "Book",
                newName: "isbn");

            migrationBuilder.RenameColumn(
                name: "Genre",
                table: "Book",
                newName: "genre");

            migrationBuilder.RenameColumn(
                name: "Author",
                table: "Book",
                newName: "author");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Book",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatedAt",
                table: "Book",
                newName: "updated_at");

            migrationBuilder.RenameColumn(
                name: "PublishedYear",
                table: "Book",
                newName: "published_year");

            migrationBuilder.RenameColumn(
                name: "IsAvailable",
                table: "Book",
                newName: "is_available");

            migrationBuilder.RenameIndex(
                name: "IX_Book_Isbn",
                table: "Book",
                newName: "IX_Book_isbn");

            migrationBuilder.RenameIndex(
                name: "IX_Book_UpdatedAt",
                table: "Book",
                newName: "IX_Book_updated_at");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "Book",
                type: "varchar(300)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "isbn",
                table: "Book",
                type: "varchar(20)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<string>(
                name: "genre",
                table: "Book",
                type: "varchar(100)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "author",
                table: "Book",
                type: "varchar(300)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(300)",
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "updated_at",
                table: "Book",
                type: "timestamptz",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");
        }
    }
}
