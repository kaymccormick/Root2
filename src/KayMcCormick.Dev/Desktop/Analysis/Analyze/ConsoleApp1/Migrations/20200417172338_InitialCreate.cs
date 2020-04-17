using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleApp1.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppTypeInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(nullable: true),
                    ParentInfoId = table.Column<int>(nullable: true),
                    HierarchyLevel = table.Column<int>(nullable: false),
                    ColorValue = table.Column<long>(nullable: true),
                    ElementName = table.Column<string>(nullable: true),
                    UpdatedDateTime = table.Column<DateTime>(nullable: false),
                    CreatedDateTime = table.Column<DateTime>(nullable: false),
                    Version = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppTypeInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppTypeInfos_AppTypeInfos_ParentInfoId",
                        column: x => x.ParentInfoId,
                        principalTable: "AppTypeInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppMethodInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AppTypeInfoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppMethodInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppMethodInfo_AppTypeInfos_AppTypeInfoId",
                        column: x => x.AppTypeInfoId,
                        principalTable: "AppTypeInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppParameterInfo",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsOptional = table.Column<bool>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Index = table.Column<int>(nullable: false),
                    AppMethodInfoId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppParameterInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppParameterInfo_AppMethodInfo_AppMethodInfoId",
                        column: x => x.AppMethodInfoId,
                        principalTable: "AppMethodInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppMethodInfo_AppTypeInfoId",
                table: "AppMethodInfo",
                column: "AppTypeInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AppParameterInfo_AppMethodInfoId",
                table: "AppParameterInfo",
                column: "AppMethodInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_AppTypeInfos_ParentInfoId",
                table: "AppTypeInfos",
                column: "ParentInfoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppParameterInfo");

            migrationBuilder.DropTable(
                name: "AppMethodInfo");

            migrationBuilder.DropTable(
                name: "AppTypeInfos");
        }
    }
}
