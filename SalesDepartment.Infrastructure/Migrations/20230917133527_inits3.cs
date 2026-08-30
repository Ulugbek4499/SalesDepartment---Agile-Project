using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesDepartment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class inits3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal precision");

            migrationBuilder.AlterColumn<decimal>(
                name: "Area",
                table: "Homes",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal precision");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmountOfContract",
                table: "Contracts",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal precision");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Payments",
                type: "decimal precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "Area",
                table: "Homes",
                type: "decimal precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<decimal>(
                name: "TotalAmountOfContract",
                table: "Contracts",
                type: "decimal precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
