using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTrackerDemo.Migrations
{
    public partial class InitSchema : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssignedTo",
                table: "ProjectTasks",
                newName: "AssigneeId");

            migrationBuilder.AddColumn<string>(
                name: "AssignedToId",
                table: "ProjectTasks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsManager",
                table: "AspNetUsers",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_AssignedToId",
                table: "ProjectTasks",
                column: "AssignedToId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_AssigneeId",
                table: "ProjectTasks",
                column: "AssigneeId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_ManagerId",
                table: "Projects",
                column: "ManagerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_ManagerId",
                table: "Projects",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_AspNetUsers_AssignedToId",
                table: "ProjectTasks",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_AspNetUsers_AssigneeId",
                table: "ProjectTasks",
                column: "AssigneeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_ManagerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_AspNetUsers_AssignedToId",
                table: "ProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_AspNetUsers_AssigneeId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_AssignedToId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_AssigneeId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_Projects_ManagerId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "AssignedToId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "IsManager",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "AssigneeId",
                table: "ProjectTasks",
                newName: "AssignedTo");
        }
    }
}
