using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace geoproject.Migrations
{
    /// <inheritdoc />
    public partial class AddHeptagonSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Points",
                columns: new[] { "Id", "CoordinateType", "Geometry", "Name" },
                values: new object[] { 5, 3, "35.0 41.0, 36.0 41.5, 36.5 42.5, 36.0 43.5, 35.0 44.0, 34.0 43.0, 34.5 42.0, 35.0 41.0", "Heptagon Example" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Points",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
