using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesDepartment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class inits2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NumberOfMonths",
                table: "Contracts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PaymentDay",
                table: "Contracts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NumberOfMonths",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "PaymentDay",
                table: "Contracts");
        }
    }
}
