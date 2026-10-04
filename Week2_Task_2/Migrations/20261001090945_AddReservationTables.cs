using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Week2_Task_2.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "reservations",
                columns: table => new
                {
                    id = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    customer_id = table.Column<int>(
                        type: "int",
                        nullable: false),

                    created_at = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false),

                    expires_at = table.Column<DateTime>(
                        type: "datetime2",
                        nullable: false),

                    status = table.Column<string>(
                        type: "nvarchar(max)",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_reservations",
                        x => x.id);

                    table.ForeignKey(
                        name: "FK_reservations_Customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "Customers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "reservation_items",
                columns: table => new
                {
                    id = table.Column<int>(
                        type: "int",
                        nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),

                    reservation_id = table.Column<int>(
                        type: "int",
                        nullable: false),

                    product_id = table.Column<int>(
                        type: "int",
                        nullable: false),

                    quantity = table.Column<int>(
                        type: "int",
                        nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey(
                        "PK_reservation_items",
                        x => x.id);

                    table.ForeignKey(
                        name: "FK_reservation_items_prod_product_id",
                        column: x => x.product_id,
                        principalTable: "prod",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);

                    table.ForeignKey(
                        name: "FK_reservation_items_reservations_reservation_id",
                        column: x => x.reservation_id,
                        principalTable: "reservations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_reservation_items_product_id",
                table: "reservation_items",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservation_items_reservation_id",
                table: "reservation_items",
                column: "reservation_id");

            migrationBuilder.CreateIndex(
                name: "IX_reservations_customer_id",
                table: "reservations",
                column: "customer_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reservation_items");

            migrationBuilder.DropTable(
                name: "reservations");
        }
    }
}