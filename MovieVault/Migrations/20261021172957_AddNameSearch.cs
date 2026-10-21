using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieVault.Migrations
{
    /// <inheritdoc />
    public partial class AddNameSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameSearch",
                table: "Titles",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
            migrationBuilder.Sql("UPDATE Titles SET NameSearch = lower(Name)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameSearch",
                table: "Titles");
        }
    }
}
