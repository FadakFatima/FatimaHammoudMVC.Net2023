using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CmsShoppingCart.Migrations
{
    public partial class SaleCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OnSale",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnSale",
                table: "Products");
        }
    }
}
