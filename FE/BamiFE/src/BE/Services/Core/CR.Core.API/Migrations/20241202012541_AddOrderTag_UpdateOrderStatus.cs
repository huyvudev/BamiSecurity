using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CR.Core.API.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderTag_UpdateOrderStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "dbo",
                table: "User",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "core",
                table: "CoreOrderItem");

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

            migrationBuilder.AddColumn<string>(
                name: "Size",
                schema: "core",
                table: "CoreOrderItem",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "CoreTag",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoreOrderTag",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    TagId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreOrderTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoreOrderTag_CoreOrder_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "core",
                        principalTable: "CoreOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoreOrderTag_CoreTag_TagId",
                        column: x => x.TagId,
                        principalSchema: "core",
                        principalTable: "CoreTag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoreOrderItem_BrandId",
                schema: "core",
                table: "CoreOrderItem",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_CoreOrderTag_OrderId",
                schema: "core",
                table: "CoreOrderTag",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CoreOrderTag_TagId",
                schema: "core",
                table: "CoreOrderTag",
                column: "TagId");

            migrationBuilder.AddForeignKey(
                name: "FK_CoreOrderItem_CoreBrand_BrandId",
                schema: "core",
                table: "CoreOrderItem",
                column: "BrandId",
                principalSchema: "core",
                principalTable: "CoreBrand",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CoreOrderItem_CoreBrand_BrandId",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropTable(
                name: "CoreOrderTag",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CoreTag",
                schema: "core");

            migrationBuilder.DropIndex(
                name: "IX_CoreOrderItem_BrandId",
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
                name: "Size",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "core",
                table: "CoreOrderItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "User",
                columns: new[] { "Id", "AvatarImageUri", "Browser", "CreatedBy", "CreatedDate", "DateOfBirth", "DateTimeLoginFailCount", "Deleted", "DeletedBy", "DeletedDate", "Email", "FullName", "Gender", "Hometown", "IdCode", "IsFirstTime", "IsPasswordTemp", "LastLogin", "LockedStatus", "ModifiedBy", "ModifiedDate", "OperatingSystem", "OtpRequestId", "Password", "PhoneNumber", "PinCode", "ResendOtpDate", "S3Key", "SecretPasswordCode", "SecretPasswordExpiryDate", "Status", "TenantDomainRegister", "TenantId", "TenantLanguage", "TenantNameRegister", "TimeLockUser", "UserCode", "UserType", "Username" },
                values: new object[] { 2, null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, null, "tenant1", null, null, null, false, false, null, null, null, null, null, null, "eiRPHnMfRCC2wPE3brn4mdqn3mGEuctNic6fNtwKFdW1mu3z5TMqeiQMGqYZ4X3C", null, null, null, null, null, null, 1, null, 1, null, null, null, null, 2, "tenant1" });
        }
    }
}
