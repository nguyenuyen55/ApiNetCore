using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SupperHeroAPI_Dotnet8.Migrations
{
    /// <inheritdoc />
    public partial class addIsActiveInRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isActive",
                table: "Rooms",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isActive",
                table: "Rooms");
        }
    }
}
