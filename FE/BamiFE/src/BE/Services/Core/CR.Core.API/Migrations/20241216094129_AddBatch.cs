using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CR.Core.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBatch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CoreBatchId",
                schema: "core",
                table: "CoreOrderItem",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CoreBatch",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatorName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    PartnerId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreBatch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoreBatch_CorePartner_PartnerId",
                        column: x => x.PartnerId,
                        principalSchema: "core",
                        principalTable: "CorePartner",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoreOrderItem_CoreBatchId",
                schema: "core",
                table: "CoreOrderItem",
                column: "CoreBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_CoreBatch_PartnerId",
                schema: "core",
                table: "CoreBatch",
                column: "PartnerId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoreOrderItem_CoreBatch_CoreBatchId",
                schema: "core",
                table: "CoreOrderItem",
                column: "CoreBatchId",
                principalSchema: "core",
                principalTable: "CoreBatch",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoreOrderItem_CoreBatch_CoreBatchId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropTable(
                name: "CoreBatch",
                schema: "core");

            migrationBuilder.DropIndex(
                name: "IX_CoreOrderItem_CoreBatchId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "CoreBatchId",
                schema: "core",
                table: "CoreOrderItem");
        }
    }
}
