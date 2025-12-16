using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DAL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCarSpecs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DrivetrainType",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "EngineCapacity",
                table: "Cars",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "ExteriorColor",
                table: "Cars",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Horsepower",
                table: "Cars",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "InteriorColor",
                table: "Cars",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrivetrainType",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "EngineCapacity",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "ExteriorColor",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "Horsepower",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "InteriorColor",
                table: "Cars");
        }
    }
}
