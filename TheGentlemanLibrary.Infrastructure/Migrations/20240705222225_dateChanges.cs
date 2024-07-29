using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TheGentlemanLibrary.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class dateChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Century",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "ExactDate",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "Born",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "Century",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "ExactDate",
                table: "Authors");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Authors");

            migrationBuilder.AddColumn<string>(
                name: "DateRange",
                table: "Books",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DateRange",
                table: "Authors",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateRange",
                table: "Books");

            migrationBuilder.DropColumn(
                name: "DateRange",
                table: "Authors");

            migrationBuilder.AddColumn<short>(
                name: "Century",
                table: "Books",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExactDate",
                table: "Books",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Year",
                table: "Books",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Born",
                table: "Authors",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Century",
                table: "Authors",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ExactDate",
                table: "Authors",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "Year",
                table: "Authors",
                type: "smallint",
                nullable: true);
        }
    }
}
