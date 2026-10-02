using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Producer.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Book",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    title = table.Column<string>(type: "varchar(300)", nullable: false),
                    author = table.Column<string>(type: "varchar(300)", nullable: false),
                    isbn = table.Column<string>(type: "varchar(20)", nullable: false),
                    genre = table.Column<string>(type: "varchar(100)", nullable: false),
                    published_year = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric(10,2)", nullable: false),
                    is_available = table.Column<bool>(type: "boolean", nullable: false),
                    updated_at = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Book", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Book_isbn",
                table: "Book",
                column: "isbn",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Book_updated_at",
                table: "Book",
                column: "updated_at");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Book");
        }
    }
}
