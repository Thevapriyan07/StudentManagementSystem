using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class markstablemodify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GradeId",
                table: "Marks",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "GradeName",
                table: "Grades",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.CreateIndex(
                name: "IX_Marks_GradeId",
                table: "Marks",
                column: "GradeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Marks_Grades_GradeId",
                table: "Marks",
                column: "GradeId",
                principalTable: "Grades",
                principalColumn: "GradeId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Marks_Grades_GradeId",
                table: "Marks");

            migrationBuilder.DropIndex(
                name: "IX_Marks_GradeId",
                table: "Marks");

            migrationBuilder.DropColumn(
                name: "GradeId",
                table: "Marks");

            migrationBuilder.AlterColumn<string>(
                name: "GradeName",
                table: "Grades",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
