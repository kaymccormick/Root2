using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleApp1.Migrations
{
    public partial class field : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppTypeInfoId1",
                table: "SyntaxFieldInfo",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SyntaxFieldInfo_AppTypeInfoId1",
                table: "SyntaxFieldInfo",
                column: "AppTypeInfoId1");

            migrationBuilder.AddForeignKey(
                name: "FK_SyntaxFieldInfo_AppTypeInfos_AppTypeInfoId1",
                table: "SyntaxFieldInfo",
                column: "AppTypeInfoId1",
                principalTable: "AppTypeInfos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SyntaxFieldInfo_AppTypeInfos_AppTypeInfoId1",
                table: "SyntaxFieldInfo");

            migrationBuilder.DropIndex(
                name: "IX_SyntaxFieldInfo_AppTypeInfoId1",
                table: "SyntaxFieldInfo");

            migrationBuilder.DropColumn(
                name: "AppTypeInfoId1",
                table: "SyntaxFieldInfo");
        }
    }
}
