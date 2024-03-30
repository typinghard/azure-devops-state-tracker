using AzureDevopsTracker.Data;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace AzureDevopsTracker.Migrations
{
    public partial class Adding_ChangeLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChangeLogs",
                schema: DataBaseConfig.SchemaName,
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(200)", nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    Response = table.Column<string>(type: "varchar(max)", nullable: true),
                    Revision = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeLogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChangeLogItems",
                schema: DataBaseConfig.SchemaName,
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(200)", nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    WorkItemId = table.Column<string>(type: "varchar(200)", nullable: true),
                    Title = table.Column<string>(type: "varchar(200)", nullable: true),
                    Description = table.Column<string>(type: "varchar(max)", nullable: true),
                    WorkItemType = table.Column<string>(type: "varchar(200)", nullable: true),
                    ChangeLogId = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChangeLogItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChangeLogItems_ChangeLogs_ChangeLogId",
                        column: x => x.ChangeLogId,
                        principalSchema: DataBaseConfig.SchemaName,
                        principalTable: "ChangeLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ChangeLogItems_WorkItems_WorkItemId",
                        column: x => x.WorkItemId,
                        principalSchema: DataBaseConfig.SchemaName,
                        principalTable: "WorkItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogItems_ChangeLogId",
                schema: DataBaseConfig.SchemaName,
                table: "ChangeLogItems",
                column: "ChangeLogId");

            migrationBuilder.CreateIndex(
                name: "IX_ChangeLogItems_WorkItemId",
                schema: DataBaseConfig.SchemaName,
                table: "ChangeLogItems",
                column: "WorkItemId",
                unique: true,
                filter: "[WorkItemId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ChangeLogItems",
                schema: DataBaseConfig.SchemaName);

            migrationBuilder.DropTable(
                name: "ChangeLogs",
                schema: DataBaseConfig.SchemaName);
        }
    }
}
