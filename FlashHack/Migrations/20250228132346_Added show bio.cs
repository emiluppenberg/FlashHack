﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlashHack.Migrations
{
    /// <inheritdoc />
    public partial class Addedshowbio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ShowBio",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShowBio",
                table: "User");
        }
    }
}
