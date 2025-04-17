using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccessLayer.Migrations
{
    /// <inheritdoc />
    public partial class seedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "Title" },
                values: new object[,]
                {
                    { new Guid("5b612e20-44f5-4c27-8e3e-e2e7e7c23cc3"), "Admin" },
                    { new Guid("6d0193c3-da6c-46ad-aa23-88169e3f9202"), "User" },
                    { new Guid("949acd82-f23d-4d2a-970e-ee89a6e1109c"), "Editor" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("5b612e20-44f5-4c27-8e3e-e2e7e7c23cc3"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("6d0193c3-da6c-46ad-aa23-88169e3f9202"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("949acd82-f23d-4d2a-970e-ee89a6e1109c"));
        }
    }
}
