using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Clicker.Security.DAL.Migrations
{
    /// <inheritdoc />
    public partial class RoleSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("8e7fb884-90a2-4121-b67d-bd0874fb596b"), "caca1e46-405e-4b9a-bfaa-a68ca81c4577", "Admin", "ADMIN" },
                    { new Guid("a5a9aa65-74c7-42cd-b98e-f2f3cd8ef08f"), "aa02eb04-4313-40b3-9e2b-3ec5b8981d7e", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8e7fb884-90a2-4121-b67d-bd0874fb596b"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a5a9aa65-74c7-42cd-b98e-f2f3cd8ef08f"));
        }
    }
}
