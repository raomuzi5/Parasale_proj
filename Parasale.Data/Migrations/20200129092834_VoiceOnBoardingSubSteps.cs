using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Parasale.Data.Migrations
{
    public partial class VoiceOnBoardingSubSteps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "VoiceOnBoardingSubSteps",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Step = table.Column<int>(nullable: false),
                    SubStep = table.Column<int>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    Message = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Element = table.Column<string>(nullable: true),
                    Position = table.Column<string>(nullable: true),
                    IsManager = table.Column<bool>(nullable: true),
                    IsAdmin = table.Column<bool>(nullable: true),
                    IsUser = table.Column<bool>(nullable: true),
                    IsNextExist = table.Column<bool>(nullable: true),
                    IsPreviousExist = table.Column<bool>(nullable: true),
                    VoiceOnBoardingId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VoiceOnBoardingSubSteps", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VoiceOnBoardingSubSteps_VoiceOnBoardings_VoiceOnBoardingId",
                        column: x => x.VoiceOnBoardingId,
                        principalTable: "VoiceOnBoardings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VoiceOnBoardingSubSteps_VoiceOnBoardingId",
                table: "VoiceOnBoardingSubSteps",
                column: "VoiceOnBoardingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VoiceOnBoardingSubSteps");
        }
    }
}
