using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetupAPI.Migrations
{
    public partial class renameCostCategory3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostItems_CostCategories_costCategoryId",
                table: "CostItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CostItems_CostCategories_costCategoryId",
                table: "CostItems",
                column: "costCategoryId",
                principalTable: "CostCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostItems_CostCategories_costCategoryId",
                table: "CostItems");

            migrationBuilder.AddForeignKey(
                name: "FK_CostItems_CostCategories_costCategoryId",
                table: "CostItems",
                column: "costCategoryId",
                principalTable: "CostCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
