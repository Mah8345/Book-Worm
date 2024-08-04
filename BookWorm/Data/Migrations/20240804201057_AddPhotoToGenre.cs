using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWorm.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddPhotoToGenre : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ImageId",
                table: "Genres",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Genres_ImageId",
                table: "Genres",
                column: "ImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genres_ApplicationImages_ImageId",
                table: "Genres",
                column: "ImageId",
                principalTable: "ApplicationImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genres_ApplicationImages_ImageId",
                table: "Genres");

            migrationBuilder.DropIndex(
                name: "IX_Genres_ImageId",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Genres");
        }
    }
}
