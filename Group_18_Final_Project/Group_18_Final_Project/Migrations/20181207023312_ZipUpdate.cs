using Microsoft.EntityFrameworkCore.Migrations;

namespace Group_18_Final_Project.Migrations
{
    public partial class ZipUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "AspNetUsers",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(int),
                oldMaxLength: 5);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ZipCode",
                table: "AspNetUsers",
                maxLength: 5,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 5,
                oldNullable: true);
        }
    }
}
