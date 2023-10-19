using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class adminIdinTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignedAdmin",
                table: "SpeechToText",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedAdmin",
                table: "ObjectionNotification",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedAdmin",
                table: "Objection",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedAdmin",
                table: "SpeechToText");

            migrationBuilder.DropColumn(
                name: "AssignedAdmin",
                table: "ObjectionNotification");

            migrationBuilder.DropColumn(
                name: "AssignedAdmin",
                table: "Objection");
        }
    }
}
