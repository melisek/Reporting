using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace szakdoga.Migrations
{
    public partial class Modify7 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Report",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Query",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Dashboards",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Query");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Dashboards");
        }
    }
}
