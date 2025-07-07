using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace geoproject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCoordinateMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Points",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Geometry = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false, comment: "Coordinate values based on CoordinateType (Point: 'x y', Line: 'x1 y1, x2 y2', Polygon: 'x1 y1, x2 y2, ...')"),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CoordinateType = table.Column<int>(type: "integer", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Points", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Points",
                columns: new[] { "Id", "CoordinateType", "Geometry", "Name" },
                values: new object[,]
                {
                    { 1, 1, "28.9784 41.0082", "Istanbul" },
                    { 2, 1, "32.8597 39.9334", "Ankara" },
                    { 3, 2, "28.9784 41.0082, 32.8597 39.9334", "Istanbul-Ankara Route" },
                    { 4, 3, "28.5 40.5, 29.5 40.5, 29.5 41.5, 28.5 41.5, 28.5 40.5", "Istanbul Region" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Points_CoordinateType",
                table: "Points",
                column: "CoordinateType");

            migrationBuilder.CreateIndex(
                name: "IX_Points_Geometry",
                table: "Points",
                column: "Geometry");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Points");
        }
    }
}
