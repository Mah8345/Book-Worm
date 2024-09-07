using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookWorm.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovedSimilarBooksTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SimilarBooks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SimilarBooks",
                columns: table => new
                {
                    BookId = table.Column<int>(type: "int", nullable: false),
                    SimilarBookId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SimilarBooks", x => new { x.BookId, x.SimilarBookId });
                    table.ForeignKey(
                        name: "FK_SimilarBooks_Books_BookId",
                        column: x => x.BookId,
                        principalTable: "Books",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SimilarBooks_Books_SimilarBookId",
                        column: x => x.SimilarBookId,
                        principalTable: "Books",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SimilarBooks_SimilarBookId",
                table: "SimilarBooks",
                column: "SimilarBookId");
        }
    }
}
