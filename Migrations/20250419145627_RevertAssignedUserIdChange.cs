using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTrackerDemo.Migrations
{
    /// <inheritdoc />
    public partial class RevertAssignedUserIdChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_AspNetUsers_AssignedUserId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_AssignedUserId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "ProjectTasks");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_AssigneeId",
                table: "ProjectTasks",
                column: "AssigneeId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_AspNetUsers_AssigneeId",
                table: "ProjectTasks",
                column: "AssigneeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_AspNetUsers_AssigneeId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_AssigneeId",
                table: "ProjectTasks");

            migrationBuilder.AddColumn<string>(
                name: "AssignedUserId",
                table: "ProjectTasks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_AssignedUserId",
                table: "ProjectTasks",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_AspNetUsers_AssignedUserId",
                table: "ProjectTasks",
                column: "AssignedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
