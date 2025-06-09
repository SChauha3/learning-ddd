using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LearningDDD.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Migrations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    Capacity = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChargeStations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", maxLength: 255, nullable: false),
                    GroupId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChargeStations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChargeStations_Groups_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Groups",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Connectors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    ChargeStationContextId = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxCurrent = table.Column<int>(type: "INTEGER", nullable: false),
                    ChargeStationId = table.Column<Guid>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Connectors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Connectors_ChargeStations_ChargeStationId",
                        column: x => x.ChargeStationId,
                        principalTable: "ChargeStations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChargeStations_GroupId",
                table: "ChargeStations",
                column: "GroupId");

            migrationBuilder.CreateIndex(
                name: "IX_Connectors_ChargeStationContextId_ChargeStationId",
                table: "Connectors",
                columns: new[] { "ChargeStationContextId", "ChargeStationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Connectors_ChargeStationId",
                table: "Connectors",
                column: "ChargeStationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Connectors");

            migrationBuilder.DropTable(
                name: "ChargeStations");

            migrationBuilder.DropTable(
                name: "Groups");
        }
    }
}
