using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OldVikings.Api.Migrations
{
    /// <inheritdoc />
    public partial class change_player : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Approved",
                table: "Players",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Approved",
                table: "Players");
        }
    }
}
