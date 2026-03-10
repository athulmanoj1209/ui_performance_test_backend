using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace performance_test.Migrations
{
    /// <inheritdoc />
    public partial class addedauditorservicetypeinAuditorServices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AuditServiceType",
                table: "AuditorServices",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceType",
                table: "AuditorServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditServiceType",
                table: "AuditorServices");

            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "AuditorServices");
        }
    }
}
