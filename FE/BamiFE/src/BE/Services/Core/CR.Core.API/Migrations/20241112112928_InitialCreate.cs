using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CR.Core.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.EnsureSchema(
                name: "core");

            migrationBuilder.CreateTable(
                name: "OpenIddictApplications",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ClientSecret = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ConsentType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JsonWebKeySet = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Permissions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostLogoutRedirectUris = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RedirectUris = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Requirements = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Settings = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictApplications", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictScopes",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Descriptions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DisplayNames = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Resources = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictScopes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    UserType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    PermissionInWeb = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SendOtp",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    SendCount = table.Column<int>(type: "int", nullable: false),
                    LastSentDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeLimitCanVerifyOtp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SendOtp", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysVar",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrName = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    VarName = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    VarValue = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    VarDesc = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysVar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: true),
                    UserCode = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    PhoneNumber = table.Column<string>(type: "varchar(20)", unicode: false, maxLength: 20, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Hometown = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IdCode = table.Column<string>(type: "varchar(125)", unicode: false, maxLength: 125, nullable: true),
                    Password = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    Email = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    FullName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    UserType = table.Column<int>(type: "int", nullable: false, defaultValue: 2),
                    Status = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    LockedStatus = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PinCode = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    IsTempPin = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    IsPasswordTemp = table.Column<bool>(type: "bit", nullable: false),
                    IsFirstTime = table.Column<bool>(type: "bit", nullable: false),
                    OperatingSystem = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Browser = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    LastLogin = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvatarImageUri = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true),
                    S3Key = table.Column<string>(type: "nvarchar(2024)", maxLength: 2024, nullable: true),
                    OtpRequestId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    ResendOtpDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoginFailCount = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    DateTimeLoginFailCount = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SecretPasswordCode = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    SecretPasswordExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    TenantId = table.Column<int>(type: "int", nullable: true),
                    TenantDomainRegister = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TenantNameRegister = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    TenantLanguage = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: true),
                    TimeLockUser = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DeletedBy = table.Column<int>(type: "int", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictAuthorizations",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Scopes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictAuthorizations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenIddictAuthorizations_OpenIddictApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalSchema: "core",
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "RolePermission",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    PermissionKey = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermission", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RolePermission_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AuthOtp",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OtpCode = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: false),
                    ExpireTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    VerifyTime = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthOtp", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AuthOtp_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationToken",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FcmToken = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    ApnsToken = table.Column<string>(type: "varchar(128)", unicode: false, maxLength: 128, nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationToken_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRole",
                schema: "dbo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedBy = table.Column<int>(type: "int", nullable: true),
                    DeletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Deleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserRole_Role_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "dbo",
                        principalTable: "Role",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRole_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "dbo",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OpenIddictTokens",
                schema: "core",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ApplicationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AuthorizationId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    ConcurrencyToken = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Payload = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Properties = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RedemptionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReferenceId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Subject = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OpenIddictTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OpenIddictTokens_OpenIddictApplications_ApplicationId",
                        column: x => x.ApplicationId,
                        principalSchema: "core",
                        principalTable: "OpenIddictApplications",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OpenIddictTokens_OpenIddictAuthorizations_AuthorizationId",
                        column: x => x.AuthorizationId,
                        principalSchema: "core",
                        principalTable: "OpenIddictAuthorizations",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "SysVar",
                columns: new[] { "Id", "GrName", "VarDesc", "VarName", "VarValue" },
                values: new object[,]
                {
                    { 2, "EKYC", "Tuổi nhỏ nhất", "AGE_MIN", "18" },
                    { 3, "OTP", "Số giây otp hết hạn", "SECOND", "60" },
                    { 4, "AUTH_MAX_TURN", "Số lượt đăng nhập cho phép", "LOGIN_MAX_TURN", "5" },
                    { 5, "AUTH_MAX_TURN", "Số lượt nhập opt cho phép", "OTP_MAX_TURN", "5" },
                    { 6, "USER_FORGOT_PASSWORD", "Thời gian hết hạn link nhập mật khẩu mới", "USER_FORGOT_PASSWORD_LINK_EXPITE_TIME", "15" }
                });

            migrationBuilder.InsertData(
                schema: "dbo",
                table: "User",
                columns: new[] { "Id", "AvatarImageUri", "Browser", "CreatedBy", "CreatedDate", "DateOfBirth", "DateTimeLoginFailCount", "Deleted", "DeletedBy", "DeletedDate", "Email", "FullName", "Gender", "Hometown", "IdCode", "IsFirstTime", "IsPasswordTemp", "LastLogin", "LockedStatus", "ModifiedBy", "ModifiedDate", "OperatingSystem", "OtpRequestId", "Password", "PhoneNumber", "PinCode", "ResendOtpDate", "S3Key", "SecretPasswordCode", "SecretPasswordExpiryDate", "Status", "TenantDomainRegister", "TenantId", "TenantLanguage", "TenantNameRegister", "TimeLockUser", "UserCode", "UserType", "Username" },
                values: new object[,]
                {
                    { 1, null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, null, "admin", null, null, null, false, false, null, null, null, null, null, null, "YwS3Sl1LmmfNEOmYcM1vovRASKv64jGzhfeIkrvTtgQNobJJXGJrSHxG/9C9HhGn", null, null, null, null, null, null, 1, null, null, null, null, null, null, 1, "admin" },
                    { 2, null, null, null, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), false, null, null, null, "tenant1", null, null, null, false, false, null, null, null, null, null, null, "rSpr+QfOFQPoiJLjwR1VW53sEyglygDkdXxdzXe3dRe3/cCy+3AY/ikx5LgRGxrp", null, null, null, null, null, null, 1, null, 1, null, null, null, null, 2, "tenant1" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthOtp",
                schema: "dbo",
                table: "AuthOtp",
                columns: new[] { "UserId", "IsUsed" });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationToken",
                schema: "dbo",
                table: "NotificationToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictApplications_ClientId",
                schema: "core",
                table: "OpenIddictApplications",
                column: "ClientId",
                unique: true,
                filter: "[ClientId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictAuthorizations_ApplicationId_Status_Subject_Type",
                schema: "core",
                table: "OpenIddictAuthorizations",
                columns: new[] { "ApplicationId", "Status", "Subject", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictScopes_Name",
                schema: "core",
                table: "OpenIddictScopes",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_ApplicationId_Status_Subject_Type",
                schema: "core",
                table: "OpenIddictTokens",
                columns: new[] { "ApplicationId", "Status", "Subject", "Type" });

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_AuthorizationId",
                schema: "core",
                table: "OpenIddictTokens",
                column: "AuthorizationId");

            migrationBuilder.CreateIndex(
                name: "IX_OpenIddictTokens_ReferenceId",
                schema: "core",
                table: "OpenIddictTokens",
                column: "ReferenceId",
                unique: true,
                filter: "[ReferenceId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Role",
                schema: "dbo",
                table: "Role",
                columns: new[] { "Name", "PermissionInWeb", "Status", "Deleted" });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission",
                schema: "dbo",
                table: "RolePermission",
                columns: new[] { "PermissionKey", "RoleId" });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermission_RoleId",
                schema: "dbo",
                table: "RolePermission",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SendOtp",
                schema: "dbo",
                table: "SendOtp",
                column: "Username");

            migrationBuilder.CreateIndex(
                name: "IX_SysVar",
                schema: "dbo",
                table: "SysVar",
                columns: new[] { "GrName", "VarName" });

            migrationBuilder.CreateIndex(
                name: "IX_User",
                schema: "dbo",
                table: "User",
                columns: new[] { "Username", "CreatedDate", "Status", "UserType" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole",
                schema: "dbo",
                table: "UserRole",
                columns: new[] { "UserId", "RoleId", "Deleted" });

            migrationBuilder.CreateIndex(
                name: "IX_UserRole_RoleId",
                schema: "dbo",
                table: "UserRole",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthOtp",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "NotificationToken",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OpenIddictScopes",
                schema: "core");

            migrationBuilder.DropTable(
                name: "OpenIddictTokens",
                schema: "core");

            migrationBuilder.DropTable(
                name: "RolePermission",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SendOtp",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "SysVar",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "UserRole",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OpenIddictAuthorizations",
                schema: "core");

            migrationBuilder.DropTable(
                name: "Role",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "User",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "OpenIddictApplications",
                schema: "core");
        }
    }
}
