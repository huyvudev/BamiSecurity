using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CR.Core.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBatchInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                schema: "core",
                table: "CoreBatch",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                schema: "core",
                table: "CoreBatch",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CutDate",
                schema: "core",
                table: "CoreBatch",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EngravedDate",
                schema: "core",
                table: "CoreBatch",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FinishDate",
                schema: "core",
                table: "CoreBatch",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PrintDate",
                schema: "core",
                table: "CoreBatch",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Sku",
                schema: "core",
                table: "CoreBatch",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "core",
                table: "CoreBatch",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedBy",
                schema: "core",
                table: "CoreBatch");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                schema: "core",
                table: "CoreBatch");

            migrationBuilder.DropColumn(
                name: "CutDate",
                schema: "core",
                table: "CoreBatch");

            migrationBuilder.DropColumn(
                name: "EngravedDate",
                schema: "core",
                table: "CoreBatch");

            migrationBuilder.DropColumn(
                name: "FinishDate",
                schema: "core",
                table: "CoreBatch");

            migrationBuilder.DropColumn(
                name: "PrintDate",
                schema: "core",
                table: "CoreBatch");

            migrationBuilder.DropColumn(
                name: "Sku",
                schema: "core",
                table: "CoreBatch");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "core",
                table: "CoreBatch");
        }
    }
}
