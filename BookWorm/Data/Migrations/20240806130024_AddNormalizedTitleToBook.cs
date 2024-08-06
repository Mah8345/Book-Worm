using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWorm.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNormalizedTitleToBook : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedTitle",
                table: "Books",
                type: "nvarchar(450)",
                nullable: false,
                computedColumnSql: "UPPER(Title)");

            migrationBuilder.CreateIndex(
                name: "IX_Books_NormalizedTitle",
                table: "Books",
                column: "NormalizedTitle");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Books_NormalizedTitle",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "NormalizedTitle",
                table: "Books");
        }
    }
}
