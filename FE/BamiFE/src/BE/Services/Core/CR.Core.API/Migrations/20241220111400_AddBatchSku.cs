using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CR.Core.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBatchSku : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sku",
                schema: "core",
                table: "CoreBatch");

            migrationBuilder.AddColumn<int>(
                name: "SkuId",
                schema: "core",
                table: "CoreBatch",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "7X6EZYlKhm2DLmESJMDduOuGG70SEbVoGagdchMKAr3TnaumY+f+ws7NClVCpeg7");

            migrationBuilder.CreateIndex(
                name: "IX_CoreBatch_SkuId",
                schema: "core",
                table: "CoreBatch",
                column: "SkuId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoreBatch_CoreSku_SkuId",
                schema: "core",
                table: "CoreBatch",
                column: "SkuId",
                principalSchema: "core",
                principalTable: "CoreSku",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoreBatch_CoreSku_SkuId",
                schema: "core",
                table: "CoreBatch");

            migrationBuilder.DropIndex(
                name: "IX_CoreBatch_SkuId",
                schema: "core",
                table: "CoreBatch");

            migrationBuilder.DropColumn(
                name: "SkuId",
                schema: "core",
                table: "CoreBatch");

            migrationBuilder.AddColumn<string>(
                name: "Sku",
                schema: "core",
                table: "CoreBatch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "D+DNCVhfCSZjGcdAoRGQiWmJ1C79hkIZEV0Hw1Ye1S0uHbcZLzJ+EhzZkrl9fAwj");
        }
    }
}
