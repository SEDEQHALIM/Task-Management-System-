using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManegmentProject.DBcontcion.Migrations
{
    /// <inheritdoc />
    public partial class editMyTaskAssignto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AssignTo",
                table: "MyTask",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_MyTask_AssignTo",
                table: "MyTask",
                column: "AssignTo");

            migrationBuilder.AddForeignKey(
                name: "FK_MyTask_AspNetUsers_AssignTo",
                table: "MyTask",
                column: "AssignTo",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MyTask_AspNetUsers_AssignTo",
                table: "MyTask");

            migrationBuilder.DropIndex(
                name: "IX_MyTask_AssignTo",
                table: "MyTask");

            migrationBuilder.DropColumn(
                name: "AssignTo",
                table: "MyTask");
        }
    }
}
