using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTrackerDemo.Migrations
{
    /// <inheritdoc />
    public partial class InitialIdentitySetup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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
                name: "IX_ProjectTasks_AssigneeId",
                table: "ProjectTasks");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "ProjectTeamMember",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AssignedUserId",
                table: "ProjectTasks",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "ManagerId",
                table: "Projects",
                type: "TEXT",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTeamMember_ApplicationUserId",
                table: "ProjectTeamMember",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_AssignedUserId",
                table: "ProjectTasks",
                column: "AssignedUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_AspNetUsers_ManagerId",
                table: "Projects",
                column: "ManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_AspNetUsers_AssignedToId",
                table: "ProjectTasks",
                column: "AssignedToId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_AspNetUsers_AssignedUserId",
                table: "ProjectTasks",
                column: "AssignedUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTeamMember_AspNetUsers_ApplicationUserId",
                table: "ProjectTeamMember",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Projects_AspNetUsers_ManagerId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_AspNetUsers_AssignedToId",
                table: "ProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_AspNetUsers_AssignedUserId",
                table: "ProjectTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTeamMember_AspNetUsers_ApplicationUserId",
                table: "ProjectTeamMember");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTeamMember_ApplicationUserId",
                table: "ProjectTeamMember");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_AssignedUserId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "ProjectTeamMember");

            migrationBuilder.DropColumn(
                name: "AssignedUserId",
                table: "ProjectTasks");

            migrationBuilder.AlterColumn<string>(
                name: "ManagerId",
                table: "Projects",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_AssigneeId",
                table: "ProjectTasks",
                column: "AssigneeId");

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
    }
}
