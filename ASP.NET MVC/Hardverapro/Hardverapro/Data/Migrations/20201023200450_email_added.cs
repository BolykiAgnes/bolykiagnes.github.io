using Microsoft.EntityFrameworkCore.Migrations;

namespace Hardverapro.Data.Migrations
{
    public partial class email_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Advertisements",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Advertisements");
        }
    }
}
