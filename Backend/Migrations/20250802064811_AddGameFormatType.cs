using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddGameFormatType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Formats",
                table: "Games");

            migrationBuilder.CreateTable(
                name: "GameFormatTypes",
                columns: table => new
                {
                    GameFormatTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameFormatTypes", x => x.GameFormatTypeId);
                });

            migrationBuilder.CreateTable(
                name: "GameGameFormatType",
                columns: table => new
                {
                    GamesGameId = table.Column<int>(type: "INTEGER", nullable: false),
                    SupportedFormatsGameFormatTypeId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameGameFormatType", x => new { x.GamesGameId, x.SupportedFormatsGameFormatTypeId });
                    table.ForeignKey(
                        name: "FK_GameGameFormatType_GameFormatTypes_SupportedFormatsGameFormatTypeId",
                        column: x => x.SupportedFormatsGameFormatTypeId,
                        principalTable: "GameFormatTypes",
                        principalColumn: "GameFormatTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameGameFormatType_Games_GamesGameId",
                        column: x => x.GamesGameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameGameFormatType_SupportedFormatsGameFormatTypeId",
                table: "GameGameFormatType",
                column: "SupportedFormatsGameFormatTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameGameFormatType");

            migrationBuilder.DropTable(
                name: "GameFormatTypes");

            migrationBuilder.AddColumn<string>(
                name: "Formats",
                table: "Games",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
