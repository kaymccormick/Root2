using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleApp1.Migrations
{
    public partial class SyntaxFields2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SyntaxFieldInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TypeName = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Override = table.Column<bool>(nullable: false),
                    Optional = table.Column<bool>(nullable: false),
                    ClrTypeName = table.Column<string>(nullable: true),
                    ElementTypeMetadataName = table.Column<string>(nullable: true),
                    IsCollection = table.Column<bool>(nullable: false),
                    AppTypeInfoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SyntaxFieldInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SyntaxFieldInfo_AppTypeInfos_AppTypeInfoId",
                        column: x => x.AppTypeInfoId,
                        principalTable: "AppTypeInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SyntaxFieldInfo_AppTypeInfoId",
                table: "SyntaxFieldInfo",
                column: "AppTypeInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SyntaxFieldInfo");
        }
    }
}
