using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieVault.Migrations
{
    /// <inheritdoc />
    public partial class UniqueTitleNameYear : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Titles_Name_ReleaseYear",
                table: "Titles",
                columns: new[] { "Name", "ReleaseYear" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Titles_Name_ReleaseYear",
                table: "Titles");
        }
    }
}
