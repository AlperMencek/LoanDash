using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanDash.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddCreditScoreToBorrower : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreditScore",
                table: "Borrowers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreditScore",
                table: "Borrowers");
        }
    }
}
