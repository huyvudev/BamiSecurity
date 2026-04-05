using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CR.Core.API.Migrations
{
    /// <inheritdoc />
    public partial class updateOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoreOrderItem_CoreBrand_BrandId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_CoreOrderItem_CoreOrderDetail_OrderDetailId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropForeignKey(
                name: "FK_CoreOrderItem_CoreSku_SkuId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropIndex(
                name: "IX_CoreOrderItem_BrandId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropIndex(
                name: "IX_CoreOrderItem_SkuId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "BrandId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "DesignBack",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "DesignFront",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "DesignHood",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "DesignSleeves",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "Length",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "MockUpBack",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "MockUpFront",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "NameSpace",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "SkuId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "Width",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "core",
                table: "CorePartner",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "core",
                table: "CorePartner",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "OrderDetailId",
                schema: "core",
                table: "CoreOrderItem",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                schema: "core",
                table: "CoreOrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Length",
                schema: "core",
                table: "CoreOrderDetail",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameSpace",
                schema: "core",
                table: "CoreOrderDetail",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleDesignBack",
                schema: "core",
                table: "CoreOrderDetail",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleDesignFront",
                schema: "core",
                table: "CoreOrderDetail",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleDesignHood",
                schema: "core",
                table: "CoreOrderDetail",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SaleDesignSleeves",
                schema: "core",
                table: "CoreOrderDetail",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkuId",
                schema: "core",
                table: "CoreOrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Width",
                schema: "core",
                table: "CoreOrderDetail",
                type: "float",
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

            migrationBuilder.CreateIndex(
                name: "IX_CoreOrderDetail_SkuId",
                schema: "core",
                table: "CoreOrderDetail",
                column: "SkuId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoreOrderDetail_CoreBrand_BrandId",
                schema: "core",
                table: "CoreOrderDetail",
                column: "BrandId",
                principalSchema: "core",
                principalTable: "CoreBrand",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CoreOrderDetail_CoreSku_SkuId",
                schema: "core",
                table: "CoreOrderDetail",
                column: "SkuId",
                principalSchema: "core",
                principalTable: "CoreSku",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_CoreOrderItem_CoreOrderDetail_OrderDetailId",
                schema: "core",
                table: "CoreOrderItem",
                column: "OrderDetailId",
                principalSchema: "core",
                principalTable: "CoreOrderDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoreOrderDetail_CoreBrand_BrandId",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_CoreOrderDetail_CoreSku_SkuId",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_CoreOrderItem_CoreOrderDetail_OrderDetailId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropIndex(
                name: "IX_CoreOrderDetail_BrandId",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_CoreOrderDetail_SkuId",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "core",
                table: "CorePartner");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "core",
                table: "CorePartner");

            migrationBuilder.DropColumn(
                name: "BrandId",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.DropColumn(
                name: "Length",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.DropColumn(
                name: "NameSpace",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.DropColumn(
                name: "SaleDesignBack",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.DropColumn(
                name: "SaleDesignFront",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.DropColumn(
                name: "SaleDesignHood",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.DropColumn(
                name: "SaleDesignSleeves",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.DropColumn(
                name: "SkuId",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.DropColumn(
                name: "Width",
                schema: "core",
                table: "CoreOrderDetail");

            migrationBuilder.AlterColumn<int>(
                name: "OrderDetailId",
                schema: "core",
                table: "CoreOrderItem",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "BrandId",
                schema: "core",
                table: "CoreOrderItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesignBack",
                schema: "core",
                table: "CoreOrderItem",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesignFront",
                schema: "core",
                table: "CoreOrderItem",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesignHood",
                schema: "core",
                table: "CoreOrderItem",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DesignSleeves",
                schema: "core",
                table: "CoreOrderItem",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Length",
                schema: "core",
                table: "CoreOrderItem",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MockUpBack",
                schema: "core",
                table: "CoreOrderItem",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MockUpFront",
                schema: "core",
                table: "CoreOrderItem",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameSpace",
                schema: "core",
                table: "CoreOrderItem",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SkuId",
                schema: "core",
                table: "CoreOrderItem",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Width",
                schema: "core",
                table: "CoreOrderItem",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "/XAcud9HxWjXOR0C9PSlOzfBk5nTclEPiCSEW8/rf3SrtEx3HBNiAzWv4IaymyB5");

            migrationBuilder.CreateIndex(
                name: "IX_CoreOrderItem_BrandId",
                schema: "core",
                table: "CoreOrderItem",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_CoreOrderItem_SkuId",
                schema: "core",
                table: "CoreOrderItem",
                column: "SkuId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoreOrderItem_CoreBrand_BrandId",
                schema: "core",
                table: "CoreOrderItem",
                column: "BrandId",
                principalSchema: "core",
                principalTable: "CoreBrand",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CoreOrderItem_CoreOrderDetail_OrderDetailId",
                schema: "core",
                table: "CoreOrderItem",
                column: "OrderDetailId",
                principalSchema: "core",
                principalTable: "CoreOrderDetail",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

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
    }
}
