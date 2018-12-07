using Microsoft.EntityFrameworkCore.Migrations;

namespace Group_18_Final_Project.Migrations
{
    public partial class ModelsUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Books_BookReorders_BookReorderID",
                table: "Books");

            migrationBuilder.DropForeignKey(
                name: "FK_Reorders_BookReorders_BookReorderID",
                table: "Reorders");

            migrationBuilder.DropIndex(
                name: "IX_Reorders_BookReorderID",
                table: "Reorders");

            migrationBuilder.DropIndex(
                name: "IX_Books_BookReorderID",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "BookReorderID",
                table: "Reorders");

            migrationBuilder.DropColumn(
                name: "BookReorderID",
                table: "Books");

            migrationBuilder.AddColumn<bool>(
                name: "IsPending",
                table: "Reorders",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "CouponActive",
                table: "Coupons",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "ReorderQuantity",
                table: "BookReorders",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookID",
                table: "BookReorders",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "BookReorders",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ReorderID",
                table: "BookReorders",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BookReorders_BookID",
                table: "BookReorders",
                column: "BookID");

            migrationBuilder.CreateIndex(
                name: "IX_BookReorders_ReorderID",
                table: "BookReorders",
                column: "ReorderID");

            migrationBuilder.AddForeignKey(
                name: "FK_BookReorders_Books_BookID",
                table: "BookReorders",
                column: "BookID",
                principalTable: "Books",
                principalColumn: "BookID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BookReorders_Reorders_ReorderID",
                table: "BookReorders",
                column: "ReorderID",
                principalTable: "Reorders",
                principalColumn: "ReorderID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BookReorders_Books_BookID",
                table: "BookReorders");

            migrationBuilder.DropForeignKey(
                name: "FK_BookReorders_Reorders_ReorderID",
                table: "BookReorders");

            migrationBuilder.DropIndex(
                name: "IX_BookReorders_BookID",
                table: "BookReorders");

            migrationBuilder.DropIndex(
                name: "IX_BookReorders_ReorderID",
                table: "BookReorders");

            migrationBuilder.DropColumn(
                name: "IsPending",
                table: "Reorders");

            migrationBuilder.DropColumn(
                name: "CouponActive",
                table: "Coupons");

            migrationBuilder.DropColumn(
                name: "BookID",
                table: "BookReorders");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "BookReorders");

            migrationBuilder.DropColumn(
                name: "ReorderID",
                table: "BookReorders");

            migrationBuilder.AddColumn<int>(
                name: "BookReorderID",
                table: "Reorders",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BookReorderID",
                table: "Books",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReorderQuantity",
                table: "BookReorders",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateIndex(
                name: "IX_Reorders_BookReorderID",
                table: "Reorders",
                column: "BookReorderID");

            migrationBuilder.CreateIndex(
                name: "IX_Books_BookReorderID",
                table: "Books",
                column: "BookReorderID");

            migrationBuilder.AddForeignKey(
                name: "FK_Books_BookReorders_BookReorderID",
                table: "Books",
                column: "BookReorderID",
                principalTable: "BookReorders",
                principalColumn: "BookReorderID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reorders_BookReorders_BookReorderID",
                table: "Reorders",
                column: "BookReorderID",
                principalTable: "BookReorders",
                principalColumn: "BookReorderID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
