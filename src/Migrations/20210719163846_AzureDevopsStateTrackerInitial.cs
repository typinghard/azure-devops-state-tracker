using AzureDevopsTracker.Data;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

namespace AzureDevopsTracker.Migrations
{
    public partial class AzureDevopsStateTrackerInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: DataBaseConfig.SchemaName);

            migrationBuilder.CreateTable(
                name: "WorkItems",
                schema: DataBaseConfig.SchemaName,
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(200)", nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    AreaPath = table.Column<string>(type: "varchar(200)", nullable: true),
                    TeamProject = table.Column<string>(type: "varchar(200)", nullable: true),
                    IterationPath = table.Column<string>(type: "varchar(200)", nullable: true),
                    AssignedTo = table.Column<string>(type: "varchar(200)", nullable: true),
                    Type = table.Column<string>(type: "varchar(200)", nullable: true),
                    CreatedBy = table.Column<string>(type: "varchar(200)", nullable: true),
                    Title = table.Column<string>(type: "varchar(200)", nullable: true),
                    Tags = table.Column<string>(type: "varchar(200)", nullable: true),
                    Effort = table.Column<string>(type: "varchar(200)", nullable: true),
                    OriginalEstimate = table.Column<string>(type: "varchar(200)", nullable: true),
                    StoryPoints = table.Column<string>(type: "varchar(200)", nullable: true),
                    WorkItemParentId = table.Column<string>(type: "varchar(200)", nullable: true),
                    Activity = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeByStates",
                schema: DataBaseConfig.SchemaName,
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(200)", nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    WorkItemId = table.Column<string>(type: "varchar(200)", nullable: true),
                    State = table.Column<string>(type: "varchar(200)", nullable: true),
                    TotalTime = table.Column<double>(nullable: false),
                    TotalWorkedTime = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeByStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TimeByStates_WorkItems_WorkItemId",
                        column: x => x.WorkItemId,
                        principalSchema: DataBaseConfig.SchemaName,
                        principalTable: "WorkItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkItemsChange",
                schema: DataBaseConfig.SchemaName,
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(200)", nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false),
                    WorkItemId = table.Column<string>(type: "varchar(200)", nullable: true),
                    NewDate = table.Column<DateTime>(nullable: false),
                    OldDate = table.Column<DateTime>(nullable: true),
                    NewState = table.Column<string>(type: "varchar(200)", nullable: true),
                    OldState = table.Column<string>(type: "varchar(200)", nullable: true),
                    ChangedBy = table.Column<string>(type: "varchar(200)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkItemsChange", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkItemsChange_WorkItems_WorkItemId",
                        column: x => x.WorkItemId,
                        principalSchema: DataBaseConfig.SchemaName,
                        principalTable: "WorkItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TimeByStates_WorkItemId",
                schema: DataBaseConfig.SchemaName,
                table: "TimeByStates",
                column: "WorkItemId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkItemsChange_WorkItemId",
                schema: DataBaseConfig.SchemaName,
                table: "WorkItemsChange",
                column: "WorkItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TimeByStates",
                schema: DataBaseConfig.SchemaName);

            migrationBuilder.DropTable(
                name: "WorkItemsChange",
                schema: DataBaseConfig.SchemaName);

            migrationBuilder.DropTable(
                name: "WorkItems",
                schema: DataBaseConfig.SchemaName);
        }
    }
}
