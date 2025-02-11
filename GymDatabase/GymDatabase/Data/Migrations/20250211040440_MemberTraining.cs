using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymDatabase.Data.Migrations
{
    /// <inheritdoc />
    public partial class MemberTraining : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MemberTrainings",
                columns: table => new
                {
                    MemberTrainingId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MembershipId = table.Column<int>(type: "int", nullable: false),
                    TrainerId = table.Column<int>(type: "int", nullable: false),
                    TrainingType = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MemberTrainings", x => x.MemberTrainingId);
                    table.ForeignKey(
                        name: "FK_MemberTrainings_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Memberships",
                        principalColumn: "MembershipId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MemberTrainings_Trainees_TrainerId",
                        column: x => x.TrainerId,
                        principalTable: "Trainees",
                        principalColumn: "TrainerId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MemberTrainings_MembershipId",
                table: "MemberTrainings",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_MemberTrainings_TrainerId",
                table: "MemberTrainings",
                column: "TrainerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MemberTrainings");
        }
    }
}
