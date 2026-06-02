using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApplicationAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateLoanTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "RemainingBalance",
                table: "Loan",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingBalance",
                table: "Loan");
        }
    }
}
