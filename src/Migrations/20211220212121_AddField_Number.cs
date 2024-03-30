using AzureDevopsTracker.Data;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AzureDevopsTracker.Migrations
{
    public partial class AddField_Number : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Number",
                schema: DataBaseConfig.SchemaName,
                table: "ChangeLogs",
                type: "varchar(200)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Number",
                schema: DataBaseConfig.SchemaName,
                table: "ChangeLogs");
        }
    }
}
