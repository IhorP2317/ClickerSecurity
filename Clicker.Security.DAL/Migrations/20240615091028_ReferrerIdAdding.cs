using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Clicker.Security.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ReferrerIdAdding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("8e7fb884-90a2-4121-b67d-bd0874fb596b"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("a5a9aa65-74c7-42cd-b98e-f2f3cd8ef08f"));

            migrationBuilder.AddColumn<Guid>(
                name: "ReferrerId",
                table: "AspNetUsers",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("006a9175-c362-4351-945b-f798e7b0b022"), "0cfbc677-fe59-4767-b9b4-26541e7d138b", "Admin", "ADMIN" },
                    { new Guid("aff1ef69-1f30-4dba-88d7-9091a2340ae8"), "85bc7ebb-bb51-4079-9798-d5f86f795d65", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("006a9175-c362-4351-945b-f798e7b0b022"));

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: new Guid("aff1ef69-1f30-4dba-88d7-9091a2340ae8"));

            migrationBuilder.DropColumn(
                name: "ReferrerId",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("8e7fb884-90a2-4121-b67d-bd0874fb596b"), "caca1e46-405e-4b9a-bfaa-a68ca81c4577", "Admin", "ADMIN" },
                    { new Guid("a5a9aa65-74c7-42cd-b98e-f2f3cd8ef08f"), "aa02eb04-4313-40b3-9e2b-3ec5b8981d7e", "User", "USER" }
                });
        }
    }
}
