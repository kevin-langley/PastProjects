using Microsoft.EntityFrameworkCore.Migrations;

namespace Group_18_Final_Project.Migrations
{
    public partial class UpdatedBooksOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ShippingAdditionalPrice",
                table: "Orders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingFirstPrice",
                table: "Orders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ExtendedPrice",
                table: "BookOrders",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingAdditionalPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingFirstPrice",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ExtendedPrice",
                table: "BookOrders");
        }
    }
}
