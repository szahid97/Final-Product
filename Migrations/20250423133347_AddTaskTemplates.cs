using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTrackerDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskTemplates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyTeams_AspNetUsers_TeamMemberId",
                table: "MyTeams");

            migrationBuilder.CreateTable(
                name: "TaskTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    TemplateName = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TaskTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateTasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    TaskTemplateId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateTasks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TemplateTasks_TaskTemplates_TaskTemplateId",
                        column: x => x.TaskTemplateId,
                        principalTable: "TaskTemplates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TemplateTasks_TaskTemplateId",
                table: "TemplateTasks",
                column: "TaskTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_MyTeams_AspNetUsers_TeamMemberId",
                table: "MyTeams",
                column: "TeamMemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyTeams_AspNetUsers_TeamMemberId",
                table: "MyTeams");

            migrationBuilder.DropTable(
                name: "TemplateTasks");

            migrationBuilder.DropTable(
                name: "TaskTemplates");

            migrationBuilder.AddForeignKey(
                name: "FK_MyTeams_AspNetUsers_TeamMemberId",
                table: "MyTeams",
                column: "TeamMemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
