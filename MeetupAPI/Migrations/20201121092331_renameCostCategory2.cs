using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetupAPI.Migrations
{
    public partial class renameCostCategory2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostItems_Budgets_costCategoryId",
                table: "CostItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Budgets_BudgetId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets");

            migrationBuilder.RenameTable(
                name: "Budgets",
                newName: "CostCategories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CostCategories",
                table: "CostCategories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CostItems_CostCategories_costCategoryId",
                table: "CostItems",
                column: "costCategoryId",
                principalTable: "CostCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_CostCategories_BudgetId",
                table: "Users",
                column: "BudgetId",
                principalTable: "CostCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CostItems_CostCategories_costCategoryId",
                table: "CostItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_CostCategories_BudgetId",
                table: "Users");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CostCategories",
                table: "CostCategories");

            migrationBuilder.RenameTable(
                name: "CostCategories",
                newName: "Budgets");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Budgets",
                table: "Budgets",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CostItems_Budgets_costCategoryId",
                table: "CostItems",
                column: "costCategoryId",
                principalTable: "Budgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Budgets_BudgetId",
                table: "Users",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
