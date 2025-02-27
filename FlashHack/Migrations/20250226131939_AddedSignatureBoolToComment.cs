using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashHack.Migrations
{
    /// <inheritdoc />
    public partial class AddedSignatureBoolToComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "UseSignature",
                table: "Comment",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UseSignature",
                table: "Comment");
        }
    }
}
