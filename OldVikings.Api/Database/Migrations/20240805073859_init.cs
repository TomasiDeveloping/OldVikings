using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OldVikings.Api.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Greetings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServerNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    AllianceName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PlayerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Greetings", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Greetings");
        }
    }
}
