using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWorm.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNormalizedNameToGenereAndPublisher : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Publishers",
                type: "nvarchar(450)",
                nullable: false,
                computedColumnSql: "UPPER(Name)");

            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Genres",
                type: "nvarchar(450)",
                nullable: false,
                computedColumnSql: "UPPER(Name)");

            migrationBuilder.CreateIndex(
                name: "IX_Publishers_NormalizedName",
                table: "Publishers",
                column: "NormalizedName");

            migrationBuilder.CreateIndex(
                name: "IX_Genres_NormalizedName",
                table: "Genres",
                column: "NormalizedName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Publishers_NormalizedName",
                table: "Publishers");

            migrationBuilder.DropIndex(
                name: "IX_Genres_NormalizedName",
                table: "Genres");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Publishers");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Genres");
        }
    }
}
