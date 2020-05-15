using Microsoft.EntityFrameworkCore.Migrations;

namespace ConsoleAnalysis.Migrations
{
    public partial class LogInvocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LogInvocation",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceLocation = table.Column<string>(nullable: true),
                    FollowingCode = table.Column<string>(nullable: true),
                    PrecedingCode = table.Column<string>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    LoggerType = table.Column<string>(nullable: true),
                    MethodDisplayName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogInvocation", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LogInvocation");
        }
    }
}
