using Microsoft.EntityFrameworkCore.Migrations;

namespace SolarCoffee.Data.Migrations
{
    public partial class Orders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrderItems_SlSalesOrders_SalesOrderId",
                table: "SalesOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SlSalesOrders_Customers_CustomerId",
                table: "SlSalesOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SlSalesOrders",
                table: "SlSalesOrders");

            migrationBuilder.RenameTable(
                name: "SlSalesOrders",
                newName: "SalesOrders");

            migrationBuilder.RenameIndex(
                name: "IX_SlSalesOrders_CustomerId",
                table: "SalesOrders",
                newName: "IX_SalesOrders_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SalesOrders",
                table: "SalesOrders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrderItems_SalesOrders_SalesOrderId",
                table: "SalesOrderItems",
                column: "SalesOrderId",
                principalTable: "SalesOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrders_Customers_CustomerId",
                table: "SalesOrders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrderItems_SalesOrders_SalesOrderId",
                table: "SalesOrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_SalesOrders_Customers_CustomerId",
                table: "SalesOrders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SalesOrders",
                table: "SalesOrders");

            migrationBuilder.RenameTable(
                name: "SalesOrders",
                newName: "SlSalesOrders");

            migrationBuilder.RenameIndex(
                name: "IX_SalesOrders_CustomerId",
                table: "SlSalesOrders",
                newName: "IX_SlSalesOrders_CustomerId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SlSalesOrders",
                table: "SlSalesOrders",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SalesOrderItems_SlSalesOrders_SalesOrderId",
                table: "SalesOrderItems",
                column: "SalesOrderId",
                principalTable: "SlSalesOrders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SlSalesOrders_Customers_CustomerId",
                table: "SlSalesOrders",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
