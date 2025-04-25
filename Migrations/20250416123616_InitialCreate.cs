using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskTrackerDemo.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProjectTasks_ProjectTasks_ProjectTaskId",
                table: "ProjectTasks");

            migrationBuilder.DropIndex(
                name: "IX_ProjectTasks_ProjectTaskId",
                table: "ProjectTasks");

            migrationBuilder.DropColumn(
                name: "ProjectTaskId",
                table: "ProjectTasks");

            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "ProjectTasks",
                type: "TEXT",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "TEXT");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "EndDate",
                table: "ProjectTasks",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "TEXT",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProjectTaskId",
                table: "ProjectTasks",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProjectTasks_ProjectTaskId",
                table: "ProjectTasks",
                column: "ProjectTaskId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProjectTasks_ProjectTasks_ProjectTaskId",
                table: "ProjectTasks",
                column: "ProjectTaskId",
                principalTable: "ProjectTasks",
                principalColumn: "Id");
        }
    }
}
