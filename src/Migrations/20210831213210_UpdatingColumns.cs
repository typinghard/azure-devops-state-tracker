using AzureDevopsTracker.Data;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AzureDevopsTracker.Migrations
{
    public partial class UpdatingColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IterationPath",
                schema: DataBaseConfig.SchemaName,
                table: "WorkItemsChange",
                type: "varchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalWorkedTime",
                schema: DataBaseConfig.SchemaName,
                table: "WorkItemsChange",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IterationPath",
                schema: DataBaseConfig.SchemaName,
                table: "WorkItemsChange");

            migrationBuilder.DropColumn(
                name: "TotalWorkedTime",
                schema: DataBaseConfig.SchemaName,
                table: "WorkItemsChange");
        }
    }
}
