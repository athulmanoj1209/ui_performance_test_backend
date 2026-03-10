using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace performance_test.Migrations
{
    /// <inheritdoc />
    public partial class removedservicetypeinAuditorservices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "AuditorServices");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ServiceType",
                table: "AuditorServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
