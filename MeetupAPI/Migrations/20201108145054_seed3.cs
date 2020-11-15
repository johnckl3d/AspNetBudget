using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetupAPI.Migrations
{
    public partial class seed3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Cost",
                table: "CostItems");

            migrationBuilder.AddColumn<double>(
                name: "Amount",
                table: "CostItems",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "CostItems");

            migrationBuilder.AddColumn<double>(
                name: "Cost",
                table: "CostItems",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
