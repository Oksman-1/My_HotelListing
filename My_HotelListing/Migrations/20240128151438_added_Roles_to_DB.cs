using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace My_HotelListing.Migrations
{
    /// <inheritdoc />
    public partial class added_Roles_to_DB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "cded9d59-1eb1-4f37-977f-d1c42ce5cbb8", "4a31eb82-3913-4dd5-82f9-6f1485d3da64", "Administrator", "ADMINISTRATOR" },
                    { "e9c96833-0efa-4632-9240-628d871df763", "bd99b972-e8a8-4ce7-adfd-31b0a2841374", "Manager", "MANAGER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cded9d59-1eb1-4f37-977f-d1c42ce5cbb8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e9c96833-0efa-4632-9240-628d871df763");
        }
    }
}
