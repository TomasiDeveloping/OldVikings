using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OldVikings.Api.Migrations
{
    /// <inheritdoc />
    public partial class feednack_update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DiscordChannelId",
                table: "Feedbacks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DiscordMessageId",
                table: "Feedbacks",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UpdatedByDiscordName",
                table: "Feedbacks",
                type: "nvarchar(64)",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UpdatedByDiscordUserId",
                table: "Feedbacks",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_DiscordChannelId_DiscordMessageId",
                table: "Feedbacks",
                columns: new[] { "DiscordChannelId", "DiscordMessageId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Feedbacks_DiscordChannelId_DiscordMessageId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "DiscordChannelId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "DiscordMessageId",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "UpdatedByDiscordName",
                table: "Feedbacks");

            migrationBuilder.DropColumn(
                name: "UpdatedByDiscordUserId",
                table: "Feedbacks");
        }
    }
}
