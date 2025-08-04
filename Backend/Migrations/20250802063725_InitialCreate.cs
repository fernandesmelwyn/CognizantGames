using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    IsAdmin = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.EmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    GameId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Formats = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.GameId);
                });

            migrationBuilder.CreateTable(
                name: "LeaderboardEntries",
                columns: table => new
                {
                    LeaderboardEntryId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    Format = table.Column<int>(type: "INTEGER", nullable: false),
                    Wins = table.Column<int>(type: "INTEGER", nullable: false),
                    Losses = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaderboardEntries", x => x.LeaderboardEntryId);
                    table.ForeignKey(
                        name: "FK_LeaderboardEntries_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LeaderboardEntries_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Fixtures",
                columns: table => new
                {
                    FixtureId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    Format = table.Column<int>(type: "INTEGER", nullable: false),
                    ScheduledTime = table.Column<DateTime>(type: "TEXT", nullable: false),
                    IsCompleted = table.Column<bool>(type: "INTEGER", nullable: false),
                    IsBye = table.Column<bool>(type: "INTEGER", nullable: false),
                    WinnerRegistrationId = table.Column<int>(type: "INTEGER", nullable: true),
                    IsKnockout = table.Column<bool>(type: "INTEGER", nullable: false),
                    NumberOfGames = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fixtures", x => x.FixtureId);
                    table.ForeignKey(
                        name: "FK_Fixtures_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Registrations",
                columns: table => new
                {
                    RegistrationId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<int>(type: "INTEGER", nullable: false),
                    GameId = table.Column<int>(type: "INTEGER", nullable: false),
                    Format = table.Column<int>(type: "INTEGER", nullable: false),
                    PartnerEmployeeId = table.Column<int>(type: "INTEGER", nullable: true),
                    PreferredTimeSlot = table.Column<int>(type: "INTEGER", nullable: false),
                    IsTeamNomination = table.Column<bool>(type: "INTEGER", nullable: false),
                    FixtureId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Registrations", x => x.RegistrationId);
                    table.ForeignKey(
                        name: "FK_Registrations_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Registrations_Employees_PartnerEmployeeId",
                        column: x => x.PartnerEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "EmployeeId");
                    table.ForeignKey(
                        name: "FK_Registrations_Fixtures_FixtureId",
                        column: x => x.FixtureId,
                        principalTable: "Fixtures",
                        principalColumn: "FixtureId");
                    table.ForeignKey(
                        name: "FK_Registrations_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "GameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scores",
                columns: table => new
                {
                    ScoreId = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FixtureId = table.Column<int>(type: "INTEGER", nullable: false),
                    Team1Score = table.Column<int>(type: "INTEGER", nullable: false),
                    Team2Score = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scores", x => x.ScoreId);
                    table.ForeignKey(
                        name: "FK_Scores_Fixtures_FixtureId",
                        column: x => x.FixtureId,
                        principalTable: "Fixtures",
                        principalColumn: "FixtureId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_GameId",
                table: "Fixtures",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Fixtures_WinnerRegistrationId",
                table: "Fixtures",
                column: "WinnerRegistrationId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardEntries_EmployeeId",
                table: "LeaderboardEntries",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_LeaderboardEntries_GameId",
                table: "LeaderboardEntries",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_EmployeeId",
                table: "Registrations",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_FixtureId",
                table: "Registrations",
                column: "FixtureId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_GameId",
                table: "Registrations",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_PartnerEmployeeId",
                table: "Registrations",
                column: "PartnerEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_Scores_FixtureId",
                table: "Scores",
                column: "FixtureId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fixtures_Registrations_WinnerRegistrationId",
                table: "Fixtures",
                column: "WinnerRegistrationId",
                principalTable: "Registrations",
                principalColumn: "RegistrationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Games_GameId",
                table: "Fixtures");

            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Games_GameId",
                table: "Registrations");

            migrationBuilder.DropForeignKey(
                name: "FK_Fixtures_Registrations_WinnerRegistrationId",
                table: "Fixtures");

            migrationBuilder.DropTable(
                name: "LeaderboardEntries");

            migrationBuilder.DropTable(
                name: "Scores");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "Registrations");

            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Fixtures");
        }
    }
}
