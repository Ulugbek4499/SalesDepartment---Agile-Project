using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SalesDepartment.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class inits7 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountOfBalance",
                table: "Contracts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "AmountOfBalance",
                table: "Contracts",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
