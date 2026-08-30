using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesDepartment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class inits5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountsInWords",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PaymentDay",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "ContractEndDate",
                table: "Contracts",
                newName: "PaymentStartDate");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountOfBalance",
                table: "Contracts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "InAdvancePaymentOfContract",
                table: "Contracts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountOfBalance",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "InAdvancePaymentOfContract",
                table: "Contracts");

            migrationBuilder.RenameColumn(
                name: "PaymentStartDate",
                table: "Contracts",
                newName: "ContractEndDate");

            migrationBuilder.AddColumn<string>(
                name: "AmountsInWords",
                table: "Payments",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PaymentDay",
                table: "Contracts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
