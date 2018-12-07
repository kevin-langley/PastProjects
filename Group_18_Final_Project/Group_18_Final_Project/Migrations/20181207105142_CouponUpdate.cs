using Microsoft.EntityFrameworkCore.Migrations;

namespace Group_18_Final_Project.Migrations
{
    public partial class CouponUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponName",
                table: "Coupons");

            migrationBuilder.AddColumn<int>(
                name: "CouponType",
                table: "Coupons",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "CouponValue",
                table: "Coupons",
                nullable: false,
                defaultValue: 0m);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponType",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "CouponValue",
                table: "Coupons");

            migrationBuilder.AddColumn<int>(
                name: "CouponName",
                table: "Coupons",
                nullable: false,
                defaultValue: 0);
        }
    }
}
