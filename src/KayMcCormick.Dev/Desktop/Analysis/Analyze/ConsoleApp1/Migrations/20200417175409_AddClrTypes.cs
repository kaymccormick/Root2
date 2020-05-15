using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleAnalysis.Migrations
{
    public partial class AddClrTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AppClrTypeId",
                table: "AppTypeInfos",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "AppAssembly",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppAssembly", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppClrType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FullName = table.Column<string>(nullable: true),
                    AssemblyQualifiedName = table.Column<string>(nullable: true),
                    AssemblyId = table.Column<int>(nullable: true),
                    BaseTypeId = table.Column<int>(nullable: true),
                    IsAbstract = table.Column<bool>(nullable: false),
                    IsClass = table.Column<bool>(nullable: false),
                    IsConstructedGenericType = table.Column<bool>(nullable: false),
                    IsGenericTypeDefinition = table.Column<bool>(nullable: false),
                    IsGenericType = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppClrType", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppClrType_AppAssembly_AssemblyId",
                        column: x => x.AssemblyId,
                        principalTable: "AppAssembly",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppClrType_AppClrType_BaseTypeId",
                        column: x => x.BaseTypeId,
                        principalTable: "AppClrType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppTypeInfos_AppClrTypeId",
                table: "AppTypeInfos",
                column: "AppClrTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_AppClrType_AssemblyId",
                table: "AppClrType",
                column: "AssemblyId");

            migrationBuilder.CreateIndex(
                name: "IX_AppClrType_BaseTypeId",
                table: "AppClrType",
                column: "BaseTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AppTypeInfos_AppClrType_AppClrTypeId",
                table: "AppTypeInfos",
                column: "AppClrTypeId",
                principalTable: "AppClrType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AppTypeInfos_AppClrType_AppClrTypeId",
                table: "AppTypeInfos");

            migrationBuilder.DropTable(
                name: "AppClrType");

            migrationBuilder.DropTable(
                name: "AppAssembly");

            migrationBuilder.DropIndex(
                name: "IX_AppTypeInfos_AppClrTypeId",
                table: "AppTypeInfos");

            migrationBuilder.DropColumn(
                name: "AppClrTypeId",
                table: "AppTypeInfos");
        }
    }
}
