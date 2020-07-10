using Microsoft.EntityFrameworkCore.Migrations;

namespace KhatmaBackEnd.Migrations
{
    public partial class AddLastDistributedPageColumnToSettingTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastDistributedPage",
                table: "Settings",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastDistributedPage",
                table: "Settings");
        }
    }
}
