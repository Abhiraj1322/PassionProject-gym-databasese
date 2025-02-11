using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GymDatabase.Data.Migrations
{
    /// <inheritdoc />
    public partial class Memebrship_payment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Memberships_payment",
                columns: table => new
                {
                    PaymentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MembershipId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships_payment", x => x.PaymentId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Memberships_payment");
        }
    }
}
