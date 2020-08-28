using Microsoft.EntityFrameworkCore.Migrations;

namespace RVCoreBoard.MVC.Migrations
{
    public partial class migration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DataBNo",
                table: "Boards",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Boards_DataBNo",
                table: "Boards",
                column: "DataBNo");

            migrationBuilder.AddForeignKey(
                name: "FK_Boards_Boards_DataBNo",
                table: "Boards",
                column: "DataBNo",
                principalTable: "Boards",
                principalColumn: "BNo",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Boards_Boards_DataBNo",
                table: "Boards");

            migrationBuilder.DropIndex(
                name: "IX_Boards_DataBNo",
                table: "Boards");

            migrationBuilder.DropColumn(
                name: "DataBNo",
                table: "Boards");
        }
    }
}
