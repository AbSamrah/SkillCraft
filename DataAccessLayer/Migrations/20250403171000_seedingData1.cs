using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class seedingData1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("5b612e20-44f5-4c27-8e3e-e2e7e7c23cc3"),
                column: "Title",
                value: "User");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6d0193c3-da6c-46ad-aa23-88169e3f9202"),
                column: "Title",
                value: "Admin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("5b612e20-44f5-4c27-8e3e-e2e7e7c23cc3"),
                column: "Title",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6d0193c3-da6c-46ad-aa23-88169e3f9202"),
                column: "Title",
                value: "User");
        }
    }
}
