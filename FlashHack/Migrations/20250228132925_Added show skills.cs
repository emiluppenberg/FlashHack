using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashHack.Migrations
{
    /// <inheritdoc />
    public partial class Addedshowskills : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowSkills",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowSkills",
                table: "User");
        }
    }
}
