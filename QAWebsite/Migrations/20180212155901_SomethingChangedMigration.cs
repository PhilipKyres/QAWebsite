using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace QAWebsite.Migrations
{
    public partial class SomethingChangedMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Question",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Question",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Votes",
                table: "Question",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Votes",
                table: "Question");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Question",
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 300);

            migrationBuilder.AlterColumn<string>(
                name: "AuthorId",
                table: "Question",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 450,
                oldNullable: true);
        }
    }
}
