using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CR.Core.API.Migrations
{
    /// <inheritdoc />
    public partial class AddOrderDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Barcode",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "Size",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.DropColumn(
                name: "Title",
                schema: "core",
                table: "CoreOrderItem");

            migrationBuilder.AddColumn<string>(
                name: "BackgroundJobId",
                schema: "core",
                table: "CoreOrder",
                type: "varchar(512)",
                unicode: false,
                maxLength: 512,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "CoreOrderDetail",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Title = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Size = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    SellerSku = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Color = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    MockUpFront = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    MockUpBack = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    DesignFront = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    DesignBack = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    DesignSleeves = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    DesignHood = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoreOrderDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoreOrderDetail_CoreOrder_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "core",
                        principalTable: "CoreOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoreOrderDetail_OrderId",
                schema: "core",
                table: "CoreOrderDetail",
                column: "OrderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoreOrderDetail",
                schema: "core");

            migrationBuilder.DropColumn(
                name: "BackgroundJobId",
                schema: "core",
                table: "CoreOrder");

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                schema: "core",
                table: "CoreOrderItem",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                schema: "core",
                table: "CoreOrderItem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                schema: "core",
                table: "CoreOrderItem",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                schema: "core",
                table: "CoreOrderItem",
                type: "nvarchar(512)",
                maxLength: 512,
                nullable: false,
                defaultValue: "");
        }
    }
}
