using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CR.Core.API.Migrations
{
    /// <inheritdoc />
    public partial class AddLinkOrderDetailInOrderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderDetailId",
                schema: "core",
                table: "CoreOrderItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "core",
                table: "CoreOrderItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_CoreOrderItem_OrderDetailId",
                schema: "core",
                table: "CoreOrderItem",
                column: "OrderDetailId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoreOrderItem_CoreOrderDetail_OrderDetailId",
                schema: "core",
                table: "CoreOrderItem",
                column: "OrderDetailId",
                principalSchema: "core",
                principalTable: "CoreOrderDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoreOrderItem_CoreOrderDetail_OrderDetailId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropIndex(
                name: "IX_CoreOrderItem_OrderDetailId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "OrderDetailId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "core",
                table: "CoreOrderItem");
        }
    }
}
