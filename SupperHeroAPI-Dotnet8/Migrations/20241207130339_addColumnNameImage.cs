using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupperHeroAPI_Dotnet8.Migrations
{
    /// <inheritdoc />
    public partial class addColumnNameImage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameImage",
                table: "images",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NameImage",
                table: "images");
        }
    }
}
