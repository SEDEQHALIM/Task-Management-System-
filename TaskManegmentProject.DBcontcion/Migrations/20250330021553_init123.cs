using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskManegmentProject.DBcontcion.Migrations
{
    /// <inheritdoc />
    public partial class init123 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MemberRole",
                table: "MemberWorkSpace",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Editor",
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "MemberRole",
                table: "MemberWorkSpace",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldDefaultValue: "Editor");
        }
    }
}
