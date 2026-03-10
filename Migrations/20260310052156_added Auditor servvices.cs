using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace performance_test.Migrations
{
    /// <inheritdoc />
    public partial class addedAuditorservvices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditorServices",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditorServices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AuditorProfileAuditorServices",
                columns: table => new
                {
                    AuditorProfilesId = table.Column<int>(type: "int", nullable: false),
                    AuditorServicesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditorProfileAuditorServices", x => new { x.AuditorProfilesId, x.AuditorServicesId });
                    table.ForeignKey(
                        name: "FK_AuditorProfileAuditorServices_AuditorProfiles_AuditorProfilesId",
                        column: x => x.AuditorProfilesId,
                        principalTable: "AuditorProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AuditorProfileAuditorServices_AuditorServices_AuditorServicesId",
                        column: x => x.AuditorServicesId,
                        principalTable: "AuditorServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditorProfileAuditorServices_AuditorServicesId",
                table: "AuditorProfileAuditorServices",
                column: "AuditorServicesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditorProfileAuditorServices");

            migrationBuilder.DropTable(
                name: "AuditorServices");
        }
    }
}
