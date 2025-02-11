using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymDatabase.Data.Migrations
{
    /// <inheritdoc />
    public partial class membership_membership_payments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Memberships_payment_MembershipId",
                table: "Memberships_payment",
                column: "MembershipId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_payment_Memberships_MembershipId",
                table: "Memberships_payment",
                column: "MembershipId",
                principalTable: "Memberships",
                principalColumn: "MembershipId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_payment_Memberships_MembershipId",
                table: "Memberships_payment");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_payment_MembershipId",
                table: "Memberships_payment");
        }
    }
}
