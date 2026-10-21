using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieVault.Migrations
{
    /// <inheritdoc />
    public partial class AddReferences : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GenreId",
                table: "Titles",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Genres",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "TEXT", maxLength: 200, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genres", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Viewings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TitleId = table.Column<int>(type: "INTEGER", nullable: false),
                    WatchedAt = table.Column<DateOnly>(type: "TEXT", nullable: false),
                    Rating = table.Column<int>(type: "INTEGER", nullable: false),
                    IsRewatch = table.Column<bool>(type: "INTEGER", nullable: false),
                    Notes = table.Column<string>(type: "TEXT", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Viewings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Viewings_Titles_TitleId",
                        column: x => x.TitleId,
                        principalTable: "Titles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Titles_GenreId",
                table: "Titles",
                column: "GenreId");

            migrationBuilder.CreateIndex(
                name: "IX_Viewings_TitleId",
                table: "Viewings",
                column: "TitleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Titles_Genres_GenreId",
                table: "Titles",
                column: "GenreId",
                principalTable: "Genres",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Titles_Genres_GenreId",
                table: "Titles");

            migrationBuilder.DropTable(
                name: "Genres");

            migrationBuilder.DropTable(
                name: "Viewings");

            migrationBuilder.DropIndex(
                name: "IX_Titles_GenreId",
                table: "Titles");

            migrationBuilder.DropColumn(
                name: "GenreId",
                table: "Titles");
        }
    }
}
