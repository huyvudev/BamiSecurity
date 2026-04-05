using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CR.Core.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderFKSku : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoreOrderItem_CoreSku_SkuId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.AlterColumn<double>(
                name: "Width",
                schema: "core",
                table: "CoreOrderItem",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "SkuId",
                schema: "core",
                table: "CoreOrderItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "MockUpFront",
                schema: "core",
                table: "CoreOrderItem",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);

            migrationBuilder.AlterColumn<double>(
                name: "Length",
                schema: "core",
                table: "CoreOrderItem",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<string>(
                name: "MockUpFront",
                schema: "core",
                table: "CoreOrderDetail",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512);

            migrationBuilder.AddForeignKey(
                name: "FK_CoreOrderItem_CoreSku_SkuId",
                schema: "core",
                table: "CoreOrderItem",
                column: "SkuId",
                principalSchema: "core",
                principalTable: "CoreSku",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoreOrderItem_CoreSku_SkuId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.AlterColumn<double>(
                name: "Width",
                schema: "core",
                table: "CoreOrderItem",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "SkuId",
                schema: "core",
                table: "CoreOrderItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MockUpFront",
                schema: "core",
                table: "CoreOrderItem",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "Length",
                schema: "core",
                table: "CoreOrderItem",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "MockUpFront",
                schema: "core",
                table: "CoreOrderDetail",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(512)",
                oldMaxLength: 512,
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CoreOrderItem_CoreSku_SkuId",
                schema: "core",
                table: "CoreOrderItem",
                column: "SkuId",
                principalSchema: "core",
                principalTable: "CoreSku",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
