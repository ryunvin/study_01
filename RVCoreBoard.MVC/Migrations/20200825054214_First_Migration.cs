using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace RVCoreBoard.MVC.Migrations
{
    public partial class First_Migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UNo = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Id = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Birth = table.Column<DateTime>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Tel = table.Column<string>(nullable: false),
                    Address = table.Column<string>(nullable: false),
                    DetailAddress = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UNo);
                });

            migrationBuilder.CreateTable(
                name: "Boards",
                columns: table => new
                {
                    BNo = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(nullable: false),
                    Content = table.Column<string>(nullable: false),
                    Reg_Date = table.Column<DateTime>(nullable: false),
                    Cnt_Read = table.Column<int>(nullable: false),
                    UNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Boards", x => x.BNo);
                    table.ForeignKey(
                        name: "FK_Boards_Users_UNo",
                        column: x => x.UNo,
                        principalTable: "Users",
                        principalColumn: "UNo",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CNo = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Content = table.Column<string>(nullable: false),
                    Reg_Date = table.Column<DateTime>(nullable: false),
                    UNo = table.Column<int>(nullable: false),
                    BNo = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CNo);
                    table.ForeignKey(
                        name: "FK_Comments_Boards_BNo",
                        column: x => x.BNo,
                        principalTable: "Boards",
                        principalColumn: "BNo",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UNo",
                        column: x => x.UNo,
                        principalTable: "Users",
                        principalColumn: "UNo",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Boards_UNo",
                table: "Boards",
                column: "UNo");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_BNo",
                table: "Comments",
                column: "BNo");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UNo",
                table: "Comments",
                column: "UNo");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Boards");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
