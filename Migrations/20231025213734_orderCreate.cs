using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsShoppingCart.Migrations
{
    public partial class orderCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Items",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Items",
                table: "Orders");
        }
    }
}
