using Microsoft.EntityFrameworkCore.Migrations;

namespace MeetupAPI.Migrations
{
    public partial class addcostitemid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CostItems",
                newName: "name");

            migrationBuilder.AddColumn<string>(
                name: "costItemId",
                table: "CostItems",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "costItemId",
                table: "CostItems");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "CostItems",
                newName: "Name");
        }
    }
}
