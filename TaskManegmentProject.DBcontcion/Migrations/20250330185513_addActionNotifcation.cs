using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManegmentProject.DBcontcion.Migrations
{
    /// <inheritdoc />
    public partial class addActionNotifcation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_MyTask_TaskId",
                table: "Notification");

            migrationBuilder.AlterColumn<string>(
                name: "TaskId",
                table: "Notification",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Action",
                table: "Notification",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "WorkspaceId",
                table: "Notification",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_WorkspaceId",
                table: "Notification",
                column: "WorkspaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_MyTask_TaskId",
                table: "Notification",
                column: "TaskId",
                principalTable: "MyTask",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_WorkSpaces_WorkspaceId",
                table: "Notification",
                column: "WorkspaceId",
                principalTable: "WorkSpaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_MyTask_TaskId",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_Notification_WorkSpaces_WorkspaceId",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_WorkspaceId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Notification");

            migrationBuilder.AlterColumn<string>(
                name: "TaskId",
                table: "Notification",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Action",
                table: "Notification",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_MyTask_TaskId",
                table: "Notification",
                column: "TaskId",
                principalTable: "MyTask",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
