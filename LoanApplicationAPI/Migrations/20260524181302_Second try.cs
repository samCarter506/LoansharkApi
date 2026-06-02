using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApplicationAPI.Migrations
{
    /// <inheritdoc />
    public partial class Secondtry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loan_Applications_ApplicationId",
                table: "Loan");

            migrationBuilder.DropIndex(
                name: "IX_MonthlyExpense_ApplicationId",
                table: "MonthlyExpense");

            migrationBuilder.DropIndex(
                name: "IX_Loan_ApplicationId",
                table: "Loan");

            migrationBuilder.AlterColumn<int>(
                name: "loanTermMonths",
                table: "Loan",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "interestRate",
                table: "Loan",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Branch",
                table: "Bankings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "BankName",
                table: "Bankings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "AccountType",
                table: "Bankings",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyExpense_ApplicationId",
                table: "MonthlyExpense",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Loan_ApplicationId",
                table: "Loan",
                column: "ApplicationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_Applications_ApplicationId",
                table: "Loan",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Loan_Applications_ApplicationId",
                table: "Loan");

            migrationBuilder.DropIndex(
                name: "IX_MonthlyExpense_ApplicationId",
                table: "MonthlyExpense");

            migrationBuilder.DropIndex(
                name: "IX_Loan_ApplicationId",
                table: "Loan");

            migrationBuilder.AlterColumn<int>(
                name: "loanTermMonths",
                table: "Loan",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "interestRate",
                table: "Loan",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Branch",
                table: "Bankings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "BankName",
                table: "Bankings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AccountType",
                table: "Bankings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_MonthlyExpense_ApplicationId",
                table: "MonthlyExpense",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_Loan_ApplicationId",
                table: "Loan",
                column: "ApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Loan_Applications_ApplicationId",
                table: "Loan",
                column: "ApplicationId",
                principalTable: "Applications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
