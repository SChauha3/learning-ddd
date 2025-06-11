using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningDDD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedShadowRowVersionToEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Groups");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "Connectors");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ChargeStations");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Groups",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "Connectors",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ChargeStations",
                type: "BLOB",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
