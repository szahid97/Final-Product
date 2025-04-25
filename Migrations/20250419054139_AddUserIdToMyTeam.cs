using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTrackerDemo.Migrations
{
    /// <inheritdoc />
    public partial class AddUserIdToMyTeam : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OwnerUserId",
                table: "MyTeams",
                newName: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MyTeams_UserId",
                table: "MyTeams",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MyTeams_AspNetUsers_UserId",
                table: "MyTeams",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyTeams_AspNetUsers_UserId",
                table: "MyTeams");

            migrationBuilder.DropIndex(
                name: "IX_MyTeams_UserId",
                table: "MyTeams");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "MyTeams",
                newName: "OwnerUserId");
        }
    }
}
