using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTrackerDemo.Migrations
{
    public partial class SimplifyTaskOwnership : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskOwner",
                table: "ProjectTasks");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TaskOwner",
                table: "ProjectTasks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
