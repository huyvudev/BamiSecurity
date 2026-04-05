using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CR.Core.API.Migrations
{
    /// <inheritdoc />
    public partial class updateOrderNamespace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "nameSpace",
                schema: "core",
                table: "CoreOrder",
                newName: "Namespace");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Namespace",
                schema: "core",
                table: "CoreOrder",
                newName: "nameSpace");
        }
    }
}
