using Microsoft.EntityFrameworkCore.Migrations;

namespace Group_18_Final_Project.Migrations
{
    public partial class UpdatedDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookOrders_BookOrderID",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_BookOrders_BookOrderID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BookOrderID",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookOrderID",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookOrderID",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BookOrderID",
                table: "Books");

            migrationBuilder.AddColumn<int>(
                name: "BookID",
                table: "BookOrders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderID",
                table: "BookOrders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookOrders_BookID",
                table: "BookOrders",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_BookOrders_OrderID",
                table: "BookOrders",
                column: "OrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_BookOrders_Books_BookID",
                table: "BookOrders",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookOrders_Orders_OrderID",
                table: "BookOrders",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "OrderID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookOrders_Books_BookID",
                table: "BookOrders");

            migrationBuilder.DropForeignKey(
                name: "FK_BookOrders_Orders_OrderID",
                table: "BookOrders");

            migrationBuilder.DropIndex(
                name: "IX_BookOrders_BookID",
                table: "BookOrders");

            migrationBuilder.DropIndex(
                name: "IX_BookOrders_OrderID",
                table: "BookOrders");

            migrationBuilder.DropColumn(
                name: "BookID",
                table: "BookOrders");

            migrationBuilder.DropColumn(
                name: "OrderID",
                table: "BookOrders");

            migrationBuilder.AddColumn<int>(
                name: "BookOrderID",
                table: "Orders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookOrderID",
                table: "Books",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BookOrderID",
                table: "Orders",
                column: "BookOrderID");

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookOrderID",
                table: "Books",
                column: "BookOrderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookOrders_BookOrderID",
                table: "Books",
                column: "BookOrderID",
                principalTable: "BookOrders",
                principalColumn: "BookOrderID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_BookOrders_BookOrderID",
                table: "Orders",
                column: "BookOrderID",
                principalTable: "BookOrders",
                principalColumn: "BookOrderID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
