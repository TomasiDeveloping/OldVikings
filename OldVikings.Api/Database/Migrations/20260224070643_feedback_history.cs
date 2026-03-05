using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OldVikings.Api.Migrations
{
    /// <inheritdoc />
    public partial class feedback_history : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_Category",
                table: "Feedbacks");

            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_Visibility_Status_CreatedAtUtc",
                table: "Feedbacks");

            migrationBuilder.AddColumn<int>(
                name: "DiscordAttempts",
                table: "Feedbacks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DiscordLastAttemptAtUtc",
                table: "Feedbacks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DiscordPosted",
                table: "Feedbacks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "DiscordThreadId",
                table: "Feedbacks",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FeedbackStatusHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeedbackItemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OldStatus = table.Column<int>(type: "int", nullable: false),
                    NewStatus = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
                    ChangedAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DiscordUserId = table.Column<long>(type: "bigint", nullable: false),
                    DiscordUserName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackStatusHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedbackStatusHistories_Feedbacks_FeedbackItemId",
                        column: x => x.FeedbackItemId,
                        principalTable: "Feedbacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackStatusHistories_FeedbackItemId_ChangedAtUtc",
                table: "FeedbackStatusHistories",
                columns: new[] { "FeedbackItemId", "ChangedAtUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedbackStatusHistories");

            migrationBuilder.DropColumn(
                name: "DiscordAttempts",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "DiscordLastAttemptAtUtc",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "DiscordPosted",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "DiscordThreadId",
                table: "Feedbacks");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_Category",
                table: "Feedbacks",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_Visibility_Status_CreatedAtUtc",
                table: "Feedbacks",
                columns: new[] { "Visibility", "Status", "CreatedAtUtc" });
        }
    }
}
