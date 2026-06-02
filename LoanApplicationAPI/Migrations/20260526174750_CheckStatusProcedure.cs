using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LoanApplicationAPI.Migrations
{
    /// <inheritdoc />
    public partial class CheckStatusProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StatusDtos",
                columns: table => new
                {
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StatusDtos");
        }
    }
}
