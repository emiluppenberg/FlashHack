using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashHack.Migrations
{
    /// <inheritdoc />
    public partial class addfavoritesandauthoredpoststouser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "UserPostFavorites",
                columns: table => new
                {
                    PostId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPostFavorites", x => new { x.PostId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserPostFavorites_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserPostFavorites_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserPostFavorites_UserId",
                table: "UserPostFavorites",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserPostFavorites");
        }
    }
}
