using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CR.Core.API.Migrations
{
    /// <inheritdoc />
    public partial class updateOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoreOrderDetail_CoreBrand_BrandId",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_CoreOrderDetail_BrandId",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.DropColumn(
                name: "BrandId",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.DropColumn(
                name: "NameSpace",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.AddColumn<string>(
                name: "Address2",
                schema: "core",
                table: "CoreOrder",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                schema: "core",
                table: "CoreOrder",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "core",
                table: "CoreOrder",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                schema: "core",
                table: "CoreOrder",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tax",
                schema: "core",
                table: "CoreOrder",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "nameSpace",
                schema: "core",
                table: "CoreOrder",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "1Oj5EzobXAbLL+Ho+BuvmEPq9yXxtFeuRkeuUpz2mlCE/t+jry6omrv+Rf/eCIuH");

            migrationBuilder.CreateIndex(
                name: "IX_CoreOrder_BrandId",
                schema: "core",
                table: "CoreOrder",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoreOrder_CoreBrand_BrandId",
                schema: "core",
                table: "CoreOrder",
                column: "BrandId",
                principalSchema: "core",
                principalTable: "CoreBrand",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoreOrder_CoreBrand_BrandId",
                schema: "core",
                table: "CoreOrder");

            migrationBuilder.DropIndex(
                name: "IX_CoreOrder_BrandId",
                schema: "core",
                table: "CoreOrder");

            migrationBuilder.DropColumn(
                name: "Address2",
                schema: "core",
                table: "CoreOrder");

            migrationBuilder.DropColumn(
                name: "BrandId",
                schema: "core",
                table: "CoreOrder");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "core",
                table: "CoreOrder");

            migrationBuilder.DropColumn(
                name: "Phone",
                schema: "core",
                table: "CoreOrder");

            migrationBuilder.DropColumn(
                name: "Tax",
                schema: "core",
                table: "CoreOrder");

            migrationBuilder.DropColumn(
                name: "nameSpace",
                schema: "core",
                table: "CoreOrder");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                schema: "core",
                table: "CoreOrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameSpace",
                schema: "core",
                table: "CoreOrderDetail",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "u4wdbwEet7an5TgzCNm6sXYqHE64QDkEa3hLo9uKp5yLKYcODiydRJeeSTiTkYzS");

            migrationBuilder.CreateIndex(
                name: "IX_CoreOrderDetail_BrandId",
                schema: "core",
                table: "CoreOrderDetail",
                column: "BrandId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoreOrderDetail_CoreBrand_BrandId",
                schema: "core",
                table: "CoreOrderDetail",
                column: "BrandId",
                principalSchema: "core",
                principalTable: "CoreBrand",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
