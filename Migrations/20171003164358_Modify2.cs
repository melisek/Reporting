using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace szakdoga.Migrations
{
    public partial class Modify2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiporDashboardRel_Riport_RiportId",
                table: "RiporDashboardRel");

            migrationBuilder.DropForeignKey(
                name: "FK_RiportUserRel_Riport_RiportId",
                table: "RiportUserRel");

            migrationBuilder.DropIndex(
                name: "IX_RiportUserRel_RiportId",
                table: "RiportUserRel");

            migrationBuilder.DropIndex(
                name: "IX_RiporDashboardRel_RiportId",
                table: "RiporDashboardRel");

            migrationBuilder.DropColumn(
                name: "RiportId",
                table: "RiportUserRel");

            migrationBuilder.DropColumn(
                name: "RiportId",
                table: "RiporDashboardRel");

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "RiportUserRel",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReportId",
                table: "RiporDashboardRel",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RiportUserRel_ReportId",
                table: "RiportUserRel",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_RiporDashboardRel_ReportId",
                table: "RiporDashboardRel",
                column: "ReportId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiporDashboardRel_Riport_ReportId",
                table: "RiporDashboardRel",
                column: "ReportId",
                principalTable: "Riport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiportUserRel_Riport_ReportId",
                table: "RiportUserRel",
                column: "ReportId",
                principalTable: "Riport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RiporDashboardRel_Riport_ReportId",
                table: "RiporDashboardRel");

            migrationBuilder.DropForeignKey(
                name: "FK_RiportUserRel_Riport_ReportId",
                table: "RiportUserRel");

            migrationBuilder.DropIndex(
                name: "IX_RiportUserRel_ReportId",
                table: "RiportUserRel");

            migrationBuilder.DropIndex(
                name: "IX_RiporDashboardRel_ReportId",
                table: "RiporDashboardRel");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "RiportUserRel");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "RiporDashboardRel");

            migrationBuilder.AddColumn<int>(
                name: "RiportId",
                table: "RiportUserRel",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RiportId",
                table: "RiporDashboardRel",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RiportUserRel_RiportId",
                table: "RiportUserRel",
                column: "RiportId");

            migrationBuilder.CreateIndex(
                name: "IX_RiporDashboardRel_RiportId",
                table: "RiporDashboardRel",
                column: "RiportId");

            migrationBuilder.AddForeignKey(
                name: "FK_RiporDashboardRel_Riport_RiportId",
                table: "RiporDashboardRel",
                column: "RiportId",
                principalTable: "Riport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RiportUserRel_Riport_RiportId",
                table: "RiportUserRel",
                column: "RiportId",
                principalTable: "Riport",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
