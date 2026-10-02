using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Producer.Migrations
{
    /// <inheritdoc />
    public partial class Surprise : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "SyncCursor",
                type: "integer",
                nullable: false,
                oldClrType: typeof(byte),
                oldType: "smallint");

            migrationBuilder.AddColumn<int>(
                name: "ProcessedAtSameTimestamp",
                table: "SyncCursor",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProcessedAtSameTimestamp",
                table: "SyncCursor");

            migrationBuilder.AlterColumn<byte>(
                name: "Id",
                table: "SyncCursor",
                type: "smallint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
