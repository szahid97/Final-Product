using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTrackerDemo.Migrations
{
    /// <inheritdoc />
    public partial class RemoveWeightageFromProjectTasks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Weightage",
                table: "ProjectTasks");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Weightage",
                table: "ProjectTasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }
    }
}
