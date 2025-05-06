using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManegmentProject.DBcontcion.Migrations
{
    /// <inheritdoc />
    public partial class SetNullOnDeleteForNotification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_MyTask_TaskId",
                table: "Notification");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_MyTask_TaskId",
                table: "Notification",
                column: "TaskId",
                principalTable: "MyTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_MyTask_TaskId",
                table: "Notification");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_MyTask_TaskId",
                table: "Notification",
                column: "TaskId",
                principalTable: "MyTask",
                principalColumn: "Id");
        }
    }
}
