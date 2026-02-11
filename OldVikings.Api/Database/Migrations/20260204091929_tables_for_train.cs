using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OldVikings.Api.Migrations
{
    /// <inheritdoc />
    public partial class tables_for_train : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Players",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DisplayName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Players", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WeeklySchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WeekStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklySchedules", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PoolLeaders",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolLeaders", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_PoolLeaders_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PoolVips",
                columns: table => new
                {
                    PlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PoolVips", x => x.PlayerId);
                    table.ForeignKey(
                        name: "FK_PoolVips_Players_PlayerId",
                        column: x => x.PlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WeeklyScheduleDays",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateOnly>(type: "date", nullable: false),
                    LeaderPlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VipPlayerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeeklyScheduleDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WeeklyScheduleDays_Players_LeaderPlayerId",
                        column: x => x.LeaderPlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeeklyScheduleDays_Players_VipPlayerId",
                        column: x => x.VipPlayerId,
                        principalTable: "Players",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WeeklyScheduleDays_WeeklySchedules_ScheduleId",
                        column: x => x.ScheduleId,
                        principalTable: "WeeklySchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyScheduleDays_LeaderPlayerId",
                table: "WeeklyScheduleDays",
                column: "LeaderPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyScheduleDays_ScheduleId_Date",
                table: "WeeklyScheduleDays",
                columns: new[] { "ScheduleId", "Date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WeeklyScheduleDays_VipPlayerId",
                table: "WeeklyScheduleDays",
                column: "VipPlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_WeeklySchedules_WeekStartDate",
                table: "WeeklySchedules",
                column: "WeekStartDate",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PoolLeaders");

            migrationBuilder.DropTable(
                name: "PoolVips");

            migrationBuilder.DropTable(
                name: "WeeklyScheduleDays");

            migrationBuilder.DropTable(
                name: "Players");

            migrationBuilder.DropTable(
                name: "WeeklySchedules");
        }
    }
}
