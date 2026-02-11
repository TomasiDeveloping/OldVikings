using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OldVikings.Api.Migrations
{
    /// <inheritdoc />
    public partial class update_pool_and_vip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "BlockNextCycle",
                table: "PoolVips",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ForcePick",
                table: "PoolVips",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "BlockNextCycle",
                table: "PoolLeaders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ForcePick",
                table: "PoolLeaders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlockNextCycle",
                table: "PoolVips");

            migrationBuilder.DropColumn(
                name: "ForcePick",
                table: "PoolVips");

            migrationBuilder.DropColumn(
                name: "BlockNextCycle",
                table: "PoolLeaders");

            migrationBuilder.DropColumn(
                name: "ForcePick",
                table: "PoolLeaders");
        }
    }
}
