using Microsoft.EntityFrameworkCore.Migrations;

namespace szakdoga.Migrations
{
    public partial class Modify2Modifier : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LastModifierId",
                table: "Report",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "LastModifierId",
                table: "Dashboards",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Report_LastModifierId",
                table: "Report",
                column: "LastModifierId");

            migrationBuilder.CreateIndex(
                name: "IX_Dashboards_LastModifierId",
                table: "Dashboards",
                column: "LastModifierId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dashboards_User_LastModifierId",
                table: "Dashboards",
                column: "LastModifierId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Report_User_LastModifierId",
                table: "Report",
                column: "LastModifierId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dashboards_User_LastModifierId",
                table: "Dashboards");

            migrationBuilder.DropForeignKey(
                name: "FK_Report_User_LastModifierId",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Report_LastModifierId",
                table: "Report");

            migrationBuilder.DropIndex(
                name: "IX_Dashboards_LastModifierId",
                table: "Dashboards");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "Dashboards");
        }
    }
}