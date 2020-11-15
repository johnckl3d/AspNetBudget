using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetupAPI.Migrations
{
    public partial class addtotalamount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "totalAmount",
                table: "Budgets",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "totalAmount",
                table: "Budgets");
        }
    }
}
