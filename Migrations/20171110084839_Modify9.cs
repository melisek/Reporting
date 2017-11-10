using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace szakdoga.Migrations
{
    public partial class Modify9 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GUID",
                table: "User",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifyDate",
                table: "Report",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<string>(
                name: "GUID",
                table: "Report",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Rows",
                table: "Report",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "GUID",
                table: "Query",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifyDate",
                table: "Dashboards",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldDefaultValueSql: "getdate()");

            migrationBuilder.AlterColumn<string>(
                name: "GUID",
                table: "Dashboards",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddUniqueConstraint(
                name: "AlternateKey_User_GUID",
                table: "User",
                column: "GUID");

            migrationBuilder.AddUniqueConstraint(
                name: "AlternateKey_Report_GUID",
                table: "Report",
                column: "GUID");

            migrationBuilder.AddUniqueConstraint(
                name: "AlternateKey_Query_GUID",
                table: "Query",
                column: "GUID");

            migrationBuilder.AddUniqueConstraint(
                name: "AlternateKey_DashBoard_GUID",
                table: "Dashboards",
                column: "GUID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AlternateKey_User_GUID",
                table: "User");

            migrationBuilder.DropUniqueConstraint(
                name: "AlternateKey_Report_GUID",
                table: "Report");

            migrationBuilder.DropUniqueConstraint(
                name: "AlternateKey_Query_GUID",
                table: "Query");

            migrationBuilder.DropUniqueConstraint(
                name: "AlternateKey_DashBoard_GUID",
                table: "Dashboards");

            migrationBuilder.DropColumn(
                name: "Rows",
                table: "Report");

            migrationBuilder.AlterColumn<string>(
                name: "GUID",
                table: "User",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifyDate",
                table: "Report",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "GUID",
                table: "Report",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "GUID",
                table: "Query",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModifyDate",
                table: "Dashboards",
                nullable: false,
                defaultValueSql: "getdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "GUID",
                table: "Dashboards",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
