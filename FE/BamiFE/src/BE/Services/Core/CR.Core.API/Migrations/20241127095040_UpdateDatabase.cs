using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CR.Core.API.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CoreBrand",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreBrand", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoreMaterial",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreMaterial", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoreOrder",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    City = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    State = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Country = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreOrder", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CorePartnerType",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorePartnerType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoreProductionMethod",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Code = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreProductionMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoreSkuBase",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreSkuBase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CoreStore",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    BrandId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreStore", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoreStore_CoreBrand_BrandId",
                        column: x => x.BrandId,
                        principalSchema: "core",
                        principalTable: "CoreBrand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CorePartner",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    PartnerTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CorePartner", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CorePartner_CorePartnerType_PartnerTypeId",
                        column: x => x.PartnerTypeId,
                        principalSchema: "core",
                        principalTable: "CorePartnerType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CoreSku",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    IsBigSize = table.Column<bool>(type: "bit", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    NeedToReview = table.Column<bool>(type: "bit", nullable: false),
                    NeedManageMaterials = table.Column<bool>(type: "bit", nullable: false),
                    AllowPartnerMarkQc = table.Column<bool>(type: "bit", nullable: false),
                    AllowQcMultipleItems = table.Column<bool>(type: "bit", nullable: false),
                    SkuBaseId = table.Column<int>(type: "int", nullable: true),
                    MaterialId = table.Column<int>(type: "int", nullable: true),
                    ProductMethodId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreSku", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoreSku_CoreMaterial_MaterialId",
                        column: x => x.MaterialId,
                        principalSchema: "core",
                        principalTable: "CoreMaterial",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CoreSku_CoreProductionMethod_ProductMethodId",
                        column: x => x.ProductMethodId,
                        principalSchema: "core",
                        principalTable: "CoreProductionMethod",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_CoreSku_CoreSkuBase_SkuBaseId",
                        column: x => x.SkuBaseId,
                        principalSchema: "core",
                        principalTable: "CoreSkuBase",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CoreOrderItem",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Barcode = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    MockUpFront = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    MockUpBack = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    NameSpace = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ItemIndex = table.Column<int>(type: "int", nullable: false),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    SkuId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreOrderItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoreOrderItem_CoreOrder_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "core",
                        principalTable: "CoreOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoreOrderItem_CoreSku_SkuId",
                        column: x => x.SkuId,
                        principalSchema: "core",
                        principalTable: "CoreSku",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CoreSkuSize",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Width = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    Length = table.Column<double>(type: "float", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    AdditionalWeight = table.Column<double>(type: "float", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    BaseCost = table.Column<double>(type: "float", nullable: false),
                    CostInMeters = table.Column<double>(type: "float", nullable: true),
                    WeightByVolume = table.Column<double>(type: "float", nullable: true),
                    PackageDescription = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    SkuId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreSkuSize", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoreSkuSize_CoreSku_SkuId",
                        column: x => x.SkuId,
                        principalSchema: "core",
                        principalTable: "CoreSku",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CoreFilePrint",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UrlFile = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    OrderItemId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreFilePrint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoreFilePrint_CoreOrderItem_OrderItemId",
                        column: x => x.OrderItemId,
                        principalSchema: "core",
                        principalTable: "CoreOrderItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CoreSkuSizePkgMockup",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MockupUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    SkuSizeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreSkuSizePkgMockup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoreSkuSizePkgMockup_CoreSkuSize_SkuSizeId",
                        column: x => x.SkuSizeId,
                        principalSchema: "core",
                        principalTable: "CoreSkuSize",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "oOGJ39c7aT7/UzpNJSUkMSNO1+ulY+2UuKQv19BHSm1zf7ggILLvtSOtA+OJwSJ4");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "eiRPHnMfRCC2wPE3brn4mdqn3mGEuctNic6fNtwKFdW1mu3z5TMqeiQMGqYZ4X3C");

            migrationBuilder.CreateIndex(
                name: "IX_CoreFilePrint_OrderItemId",
                schema: "core",
                table: "CoreFilePrint",
                column: "OrderItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CoreOrderItem_OrderId",
                schema: "core",
                table: "CoreOrderItem",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CoreOrderItem_SkuId",
                schema: "core",
                table: "CoreOrderItem",
                column: "SkuId");

            migrationBuilder.CreateIndex(
                name: "IX_CorePartner_PartnerTypeId",
                schema: "core",
                table: "CorePartner",
                column: "PartnerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoreSku_MaterialId",
                schema: "core",
                table: "CoreSku",
                column: "MaterialId");

            migrationBuilder.CreateIndex(
                name: "IX_CoreSku_ProductMethodId",
                schema: "core",
                table: "CoreSku",
                column: "ProductMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_CoreSku_SkuBaseId",
                schema: "core",
                table: "CoreSku",
                column: "SkuBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_CoreSkuSize_SkuId",
                schema: "core",
                table: "CoreSkuSize",
                column: "SkuId");

            migrationBuilder.CreateIndex(
                name: "IX_CoreSkuSizePkgMockup_SkuSizeId",
                schema: "core",
                table: "CoreSkuSizePkgMockup",
                column: "SkuSizeId");

            migrationBuilder.CreateIndex(
                name: "IX_CoreStore_BrandId",
                schema: "core",
                table: "CoreStore",
                column: "BrandId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoreFilePrint",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CorePartner",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CoreSkuSizePkgMockup",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CoreStore",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CoreOrderItem",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CorePartnerType",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CoreSkuSize",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CoreBrand",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CoreOrder",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CoreSku",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CoreMaterial",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CoreProductionMethod",
                schema: "core");

            migrationBuilder.DropTable(
                name: "CoreSkuBase",
                schema: "core");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Password",
                value: "YwS3Sl1LmmfNEOmYcM1vovRASKv64jGzhfeIkrvTtgQNobJJXGJrSHxG/9C9HhGn");

            migrationBuilder.UpdateData(
                schema: "dbo",
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                column: "Password",
                value: "rSpr+QfOFQPoiJLjwR1VW53sEyglygDkdXxdzXe3dRe3/cCy+3AY/ikx5LgRGxrp");
        }
    }
}
