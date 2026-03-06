using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace performance_test.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuditNodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentId = table.Column<int>(type: "int", nullable: true),
                    NodeKind = table.Column<int>(type: "int", nullable: false),
                    SortOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditNodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditNodes_AuditNodes_ParentId",
                        column: x => x.ParentId,
                        principalTable: "AuditNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AuditNodeData",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AuditNodeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditNodeData", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditNodeData_AuditNodes_AuditNodeId",
                        column: x => x.AuditNodeId,
                        principalTable: "AuditNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuditorProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpId = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Dept = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Contact = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Image = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    AuditNodeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuditorProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuditorProfiles_AuditNodes_AuditNodeId",
                        column: x => x.AuditNodeId,
                        principalTable: "AuditNodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuditNodeData_AuditNodeId",
                table: "AuditNodeData",
                column: "AuditNodeId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AuditNodes_ParentId",
                table: "AuditNodes",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_AuditorProfiles_AuditNodeId",
                table: "AuditorProfiles",
                column: "AuditNodeId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuditNodeData");

            migrationBuilder.DropTable(
                name: "AuditorProfiles");

            migrationBuilder.DropTable(
                name: "AuditNodes");
        }
    }
}
