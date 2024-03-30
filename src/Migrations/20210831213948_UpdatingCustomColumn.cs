using AzureDevopsTracker.Data;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AzureDevopsTracker.Migrations
{
    public partial class UpdatingCustomColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Lancado",
                schema: DataBaseConfig.SchemaName,
                table: "WorkItems",
                type: "varchar(200)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lancado",
                schema: DataBaseConfig.SchemaName,
                table: "WorkItems");
        }
    }
}
