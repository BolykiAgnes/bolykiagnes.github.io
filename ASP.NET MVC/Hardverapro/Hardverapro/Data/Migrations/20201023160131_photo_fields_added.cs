using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hardverapro.Data.Migrations
{
    public partial class photo_fields_added : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Advertisements",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "PictureData",
                table: "Advertisements",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Advertisements");

            migrationBuilder.DropColumn(
                name: "PictureData",
                table: "Advertisements");
        }
    }
}
