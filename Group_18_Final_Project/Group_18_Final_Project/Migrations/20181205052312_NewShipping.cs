using Microsoft.EntityFrameworkCore.Migrations;

namespace Group_18_Final_Project.Migrations
{
    public partial class NewShipping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShippingAdditionalPrice",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "ShippingFirstPrice",
                table: "Orders",
                newName: "TotalShippingPrice");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalShippingPrice",
                table: "Orders",
                newName: "ShippingFirstPrice");

            migrationBuilder.AddColumn<decimal>(
                name: "ShippingAdditionalPrice",
                table: "Orders",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
