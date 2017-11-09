using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace szakdoga.Migrations
{
    public partial class Modify8 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Columns",
                table: "Report",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Filter",
                table: "Report",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sort",
                table: "Report",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Columns",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "Filter",
                table: "Report");

            migrationBuilder.DropColumn(
                name: "Sort",
                table: "Report");
        }
    }
}
