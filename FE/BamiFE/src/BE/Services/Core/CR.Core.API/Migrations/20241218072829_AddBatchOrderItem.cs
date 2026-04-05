using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CR.Core.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBatchOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoreOrderItem_CoreBatch_CoreBatchId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.RenameColumn(
                name: "CoreBatchId",
                schema: "core",
                table: "CoreOrderItem",
                newName: "BatchId");

            migrationBuilder.RenameIndex(
                name: "IX_CoreOrderItem_CoreBatchId",
                schema: "core",
                table: "CoreOrderItem",
                newName: "IX_CoreOrderItem_BatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoreOrderItem_CoreBatch_BatchId",
                schema: "core",
                table: "CoreOrderItem",
                column: "BatchId",
                principalSchema: "core",
                principalTable: "CoreBatch",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoreOrderItem_CoreBatch_BatchId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.RenameColumn(
                name: "BatchId",
                schema: "core",
                table: "CoreOrderItem",
                newName: "CoreBatchId");

            migrationBuilder.RenameIndex(
                name: "IX_CoreOrderItem_BatchId",
                schema: "core",
                table: "CoreOrderItem",
                newName: "IX_CoreOrderItem_CoreBatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoreOrderItem_CoreBatch_CoreBatchId",
                schema: "core",
                table: "CoreOrderItem",
                column: "CoreBatchId",
                principalSchema: "core",
                principalTable: "CoreBatch",
                principalColumn: "Id");
        }
    }
}
