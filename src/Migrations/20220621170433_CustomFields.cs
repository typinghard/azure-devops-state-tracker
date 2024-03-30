using AzureDevopsTracker.Data;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AzureDevopsTracker.Migrations
{
    public partial class CustomFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Lancado",
                schema: DataBaseConfig.SchemaName,
                table: "WorkItems");

            migrationBuilder.CreateTable(
                name: "CustomFields",
                schema: DataBaseConfig.SchemaName,
                columns: table => new
                {
                    WorkItemId = table.Column<string>(type: "varchar(200)", nullable: false),
                    Key = table.Column<string>(type: "varchar(1000)", nullable: false),
                    Value = table.Column<string>(type: "varchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomFields", x => new { x.WorkItemId, x.Key });
                    table.ForeignKey(
                        name: "FK_CustomFields_WorkItems_WorkItemId",
                        column: x => x.WorkItemId,
                        principalSchema: DataBaseConfig.SchemaName,
                        principalTable: "WorkItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomFields",
                schema: DataBaseConfig.SchemaName);

            migrationBuilder.AddColumn<string>(
                name: "Lancado",
                schema: DataBaseConfig.SchemaName,
                table: "WorkItems",
                type: "varchar(200)",
                nullable: true);
        }
    }
}
