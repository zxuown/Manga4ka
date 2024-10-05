using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Manga4ka.Migrations
{
    /// <inheritdoc />
    public partial class Update : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Genre_Manga_MangaId",
                table: "Genre");

            migrationBuilder.DropIndex(
                name: "IX_Genre_MangaId",
                table: "Genre");

            migrationBuilder.DropColumn(
                name: "MangaId",
                table: "Genre");

            migrationBuilder.CreateTable(
                name: "MangaGenres",
                columns: table => new
                {
                    MangaId = table.Column<int>(type: "INTEGER", nullable: false),
                    GenreId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MangaGenres", x => new { x.MangaId, x.GenreId });
                    table.ForeignKey(
                        name: "FK_MangaGenres_Genre_GenreId",
                        column: x => x.GenreId,
                        principalTable: "Genre",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MangaGenres_Manga_MangaId",
                        column: x => x.MangaId,
                        principalTable: "Manga",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MangaGenres_GenreId",
                table: "MangaGenres",
                column: "GenreId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MangaGenres");

            migrationBuilder.AddColumn<int>(
                name: "MangaId",
                table: "Genre",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Genre_MangaId",
                table: "Genre",
                column: "MangaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Genre_Manga_MangaId",
                table: "Genre",
                column: "MangaId",
                principalTable: "Manga",
                principalColumn: "Id");
        }
    }
}
