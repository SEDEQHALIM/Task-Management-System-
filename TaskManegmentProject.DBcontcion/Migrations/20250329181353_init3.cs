using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManegmentProject.DBcontcion.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberWorkSpace_AspNetRoles_RoleID",
                table: "MemberWorkSpace");

            migrationBuilder.AlterColumn<string>(
                name: "RoleID",
                table: "MemberWorkSpace",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "MemberRole",
                table: "MemberWorkSpace",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberWorkSpace_AspNetRoles_RoleID",
                table: "MemberWorkSpace",
                column: "RoleID",
                principalTable: "AspNetRoles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MemberWorkSpace_AspNetRoles_RoleID",
                table: "MemberWorkSpace");

            migrationBuilder.DropColumn(
                name: "MemberRole",
                table: "MemberWorkSpace");

            migrationBuilder.AlterColumn<string>(
                name: "RoleID",
                table: "MemberWorkSpace",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MemberWorkSpace_AspNetRoles_RoleID",
                table: "MemberWorkSpace",
                column: "RoleID",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
