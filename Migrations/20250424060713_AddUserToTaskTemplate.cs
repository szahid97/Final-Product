using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTrackerDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddUserToTaskTemplate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "TaskTemplates",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_TaskTemplates_UserId",
                table: "TaskTemplates",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskTemplates_AspNetUsers_UserId",
                table: "TaskTemplates",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskTemplates_AspNetUsers_UserId",
                table: "TaskTemplates");

            migrationBuilder.DropIndex(
                name: "IX_TaskTemplates_UserId",
                table: "TaskTemplates");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "TaskTemplates");
        }
    }
}
