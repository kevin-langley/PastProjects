using Microsoft.EntityFrameworkCore.Migrations;

namespace Group_18_Final_Project.Migrations
{
    public partial class SomeUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 5,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ZipCode",
                table: "AspNetUsers",
                maxLength: 5,
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
