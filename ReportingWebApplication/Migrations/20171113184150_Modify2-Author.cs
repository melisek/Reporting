using Microsoft.EntityFrameworkCore.Migrations;

namespace szakdoga.Migrations
{
    public partial class Modify2Author : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Report",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AuthorId",
                table: "Dashboards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Report_AuthorId",
                table: "Report",
                column: "AuthorId");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_AuthorId",
                table: "Dashboards",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dashboards_User_AuthorId",
                table: "Dashboards",
                column: "AuthorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_User_AuthorId",
                table: "Report",
                column: "AuthorId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dashboards_User_AuthorId",
                table: "Dashboards");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_User_AuthorId",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_AuthorId",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Dashboards_AuthorId",
                table: "Dashboards");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Dashboards");
        }
    }
}