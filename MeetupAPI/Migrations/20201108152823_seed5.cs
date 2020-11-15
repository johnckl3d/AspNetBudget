using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetupAPI.Migrations
{
    public partial class seed5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Budgets_Users_UserId",
                table: "Budgets");

            migrationBuilder.DropIndex(
                name: "IX_Budgets_UserId",
                table: "Budgets");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Budgets");

            migrationBuilder.AddColumn<int>(
                name: "BudgetId",
                table: "Users",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_BudgetId",
                table: "Users",
                column: "BudgetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Budgets_BudgetId",
                table: "Users",
                column: "BudgetId",
                principalTable: "Budgets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Budgets_BudgetId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_BudgetId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BudgetId",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Budgets",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Budgets_UserId",
                table: "Budgets",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Budgets_Users_UserId",
                table: "Budgets",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
