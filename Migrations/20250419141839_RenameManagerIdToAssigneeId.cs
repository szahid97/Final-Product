using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTrackerDemo.Migrations
{
    /// <inheritdoc />
    public partial class RenameManagerIdToAssigneeId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_ManagerId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "IsSubTask",
                table: "ProjectTasks");

            migrationBuilder.RenameColumn(
                name: "ManagerId",
                table: "Projects",
                newName: "AssigneeId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_ManagerId",
                table: "Projects",
                newName: "IX_Projects_AssigneeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_AssigneeId",
                table: "Projects",
                column: "AssigneeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_AssigneeId",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "AssigneeId",
                table: "Projects",
                newName: "ManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_Projects_AssigneeId",
                table: "Projects",
                newName: "IX_Projects_ManagerId");

            migrationBuilder.AddColumn<bool>(
                name: "IsSubTask",
                table: "ProjectTasks",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_ManagerId",
                table: "Projects",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
