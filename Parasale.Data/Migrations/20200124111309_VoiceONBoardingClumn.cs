using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class VoiceONBoardingClumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {            
            migrationBuilder.AlterColumn<bool>(
                name: "IsStartupPopUp",
                table: "VoiceOnBoardings",
                nullable: true,
                oldClrType: typeof(bool));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
               name: "IsStartupPopUp",
               table: "VoiceOnBoardings",
               nullable: false,
               oldClrType: typeof(bool),
               oldNullable: true);
        }
    }
}
