using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OldVikings.Api.Migrations
{
    /// <inheritdoc />
    public partial class changeScheduleDay : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyScheduleDays_Players_LeaderPlayerId",
                table: "WeeklyScheduleDays");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyScheduleDays_Players_VipPlayerId",
                table: "WeeklyScheduleDays");

            migrationBuilder.AlterColumn<Guid>(
                name: "VipPlayerId",
                table: "WeeklyScheduleDays",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "LeaderPlayerId",
                table: "WeeklyScheduleDays",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyScheduleDays_Players_LeaderPlayerId",
                table: "WeeklyScheduleDays",
                column: "LeaderPlayerId",
                principalTable: "Players",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyScheduleDays_Players_VipPlayerId",
                table: "WeeklyScheduleDays",
                column: "VipPlayerId",
                principalTable: "Players",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyScheduleDays_Players_LeaderPlayerId",
                table: "WeeklyScheduleDays");

            migrationBuilder.DropForeignKey(
                name: "FK_WeeklyScheduleDays_Players_VipPlayerId",
                table: "WeeklyScheduleDays");

            migrationBuilder.AlterColumn<Guid>(
                name: "VipPlayerId",
                table: "WeeklyScheduleDays",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LeaderPlayerId",
                table: "WeeklyScheduleDays",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyScheduleDays_Players_LeaderPlayerId",
                table: "WeeklyScheduleDays",
                column: "LeaderPlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WeeklyScheduleDays_Players_VipPlayerId",
                table: "WeeklyScheduleDays",
                column: "VipPlayerId",
                principalTable: "Players",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
