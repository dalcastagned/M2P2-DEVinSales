using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DevInSales.Core.Data.Migrations
{
    public partial class UpdateSeedUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "a799d129-c11d-419a-9412-30576f5ead2d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "f9114dd4-6d0e-49be-aaac-79da1af7a120");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "9255abb0-1242-4114-9dd1-188448981c6e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthDate", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { new DateTime(2022, 6, 19, 8, 37, 15, 835, DateTimeKind.Local).AddTicks(7986), "5789bd5a-5a0f-4dc6-96ae-78b583c6e9dd", "AQAAAAEAACcQAAAAEEgCV06KBfThNIjM0nhN2wPpM0ziE1yI4lCmXfj2A8Sk51HruUdCw/cfuxerKjLNkw==", "YYGXBMRWXF6A3J5PEYA3EVNXG6Y4YBTC" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "827e930d-1246-4179-9f08-1454c72d8105");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "576126b1-4585-4c87-b12e-5a6be341d4f2");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 3,
                column: "ConcurrencyStamp",
                value: "7a021da7-b844-4276-9e6f-367b1fe00720");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "BirthDate", "ConcurrencyStamp", "PasswordHash", "SecurityStamp" },
                values: new object[] { new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "712db9ab-f081-4621-ae94-bef8d5228298", "AQAAAAEAACcQAAAAEILicgVcoZdOLqUrW4aCUvvJMWCDl+d7Y8AIwuPyaH4A2hicRG6Q1C/oSEAqyOMGjA==", null });
        }
    }
}
