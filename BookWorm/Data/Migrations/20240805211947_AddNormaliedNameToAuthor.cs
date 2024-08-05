using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWorm.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddNormaliedNameToAuthor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedName",
                table: "Authors",
                type: "nvarchar(450)",
                nullable: false,
                computedColumnSql: "UPPER(Name)");

            migrationBuilder.CreateIndex(
                name: "IX_Authors_NormalizedName",
                table: "Authors",
                column: "NormalizedName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Authors_NormalizedName",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "NormalizedName",
                table: "Authors");
        }
    }
}
