using AzureDevopsTracker.Data;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AzureDevopsTracker.Migrations
{
    public partial class WorkItem_AddDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                schema: DataBaseConfig.SchemaName,
                table: "WorkItems",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                schema: DataBaseConfig.SchemaName,
                table: "WorkItems");
        }
    }
}
