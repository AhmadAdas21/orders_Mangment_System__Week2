using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Week2_Task_2.Migrations
{
    /// <inheritdoc />
    public partial class FixProductStockColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "stock",
                table: "prod",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "stock",
                table: "prod");
        }
    }
}
