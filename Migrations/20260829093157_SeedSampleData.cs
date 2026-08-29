using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JOTrain.Migrations
{
    /// <inheritdoc />
    public partial class SeedSampleData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Amman" },
                    { 2, "Zarqa" },
                    { 3, "Irbid" },
                    { 4, "Aqaba" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "FullName", "Password", "Role" },
                values: new object[,]
                {
                    { 1, "admin@jotrain.com", "System Admin", "123", 0 },
                    { 2, "staff@jotrain.com", "Station Staff", "123", 1 },
                    { 3, "hamzeh@jotrain.com", "Hamzeh Waleed Alafaghani", "123", 2 }
                });

            migrationBuilder.InsertData(
                table: "Trips",
                columns: new[] { "Id", "ArrivalStationId", "DepartureStationId", "DepartureTime", "PriceEconomy", "PriceEconomyPlus", "PriceVIP" },
                values: new object[,]
                {
                    { 1, 2, 1, new DateTime(2026, 9, 10, 8, 30, 0, 0, DateTimeKind.Unspecified), 2.00m, 3.50m, 5.00m },
                    { 2, 3, 1, new DateTime(2026, 9, 10, 10, 0, 0, 0, DateTimeKind.Unspecified), 4.00m, 6.00m, 8.00m },
                    { 3, 4, 1, new DateTime(2026, 9, 12, 7, 0, 0, 0, DateTimeKind.Unspecified), 10.00m, 15.00m, 25.00m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Trips",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: 4);
        }
    }
}
