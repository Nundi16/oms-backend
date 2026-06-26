using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OMS.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderClinic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrderClinic",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClinicSpecificOrderName = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uuid", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    ModifierId = table.Column<Guid>(type: "uuid", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    SourceId = table.Column<Guid>(type: "uuid", nullable: false),
                    DependantId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderClinic", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderClinic_Clinic_DependantId",
                        column: x => x.DependantId,
                        principalTable: "Clinic",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderClinic_Order_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrderClinic_DependantId",
                table: "OrderClinic",
                column: "DependantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderClinic_SourceId_DependantId",
                table: "OrderClinic",
                columns: new[] { "SourceId", "DependantId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrderClinic");
        }
    }
}
