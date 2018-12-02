using Microsoft.EntityFrameworkCore.Migrations;

namespace Group_18_Final_Project.Migrations
{
    public partial class TypeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ReviewText",
                table: "Reviews",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CouponCode",
                table: "Coupons",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<decimal>(
                name: "AverageRating",
                table: "Books",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponCode",
                table: "Coupons");

            migrationBuilder.AlterColumn<string>(
                name: "ReviewText",
                table: "Reviews",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AverageRating",
                table: "Books",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
