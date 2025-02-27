using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TechBack_Identity.Migrations
{
    /// <inheritdoc />
    public partial class EmailEgnor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NormalizedEmail",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NormalizedEmail",
                table: "Users");
        }
    }
}
