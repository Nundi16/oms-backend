using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addconnectorfksandorderclinicconfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderClinics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClinicSpecificOrderName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    ParentId = table.Column<Guid>(type: "uuid", nullable: false),
                    DependantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderClinics", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderClinics_DependantId",
                table: "OrderClinics",
                column: "DependantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderClinics_ParentId",
                table: "OrderClinics",
                column: "ParentId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderClinics_ParentId_DependantId",
                table: "OrderClinics",
                columns: new[] { "ParentId", "DependantId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderClinics");
        }
    }
}
