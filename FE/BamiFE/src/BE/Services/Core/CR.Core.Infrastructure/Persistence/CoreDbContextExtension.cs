using CR.ApplicationBase.Localization;
using CR.Constants.Common.SysVar;
using CR.Constants.Core.MultiTenancy;
using CR.Constants.Core.Users;
using CR.Core.Domain.SysVar;
using CR.Core.Domain.Users;
using CR.Utils.Security;
using Microsoft.EntityFrameworkCore;

namespace CR.Core.Infrastructure.Persistence
{
    public static class CoreDbContextExtension
    {
        public static void SeedData(this ModelBuilder modelBuilder)
        {
            #region system
            modelBuilder
                .Entity<User>()
                .HasData(
                    new User
                    {
                        Id = 1,
                        Username = "admin",
                        Password = PasswordHasher.HashPassword("123qwe"),
                        FullName = "admin",
                        UserType = UserTypeEnum.SUPER_ADMIN,
                        Status = UserStatus.ACTIVE,
                    }
                );

            modelBuilder
                .Entity<SysVar>()
                .HasData(
                    new SysVar
                    {
                        Id = 2,
                        GrName = "EKYC",
                        VarName = "AGE_MIN",
                        VarValue = "18",
                        VarDesc = "Tuổi nhỏ nhất"
                    }
                );
            modelBuilder
                .Entity<SysVar>()
                .HasData(
                    new SysVar
                    {
                        Id = 3,
                        GrName = "OTP",
                        VarName = "SECOND",
                        VarValue = "60",
                        VarDesc = "Số giây otp hết hạn"
                    }
                );
            modelBuilder
                .Entity<SysVar>()
                .HasData(
                    new SysVar
                    {
                        Id = 4,
                        GrName = "AUTH_MAX_TURN",
                        VarName = "LOGIN_MAX_TURN",
                        VarValue = "5",
                        VarDesc = "Số lượt đăng nhập cho phép"
                    }
                );
            modelBuilder
                .Entity<SysVar>()
                .HasData(
                    new SysVar
                    {
                        Id = 5,
                        GrName = "AUTH_MAX_TURN",
                        VarName = "OTP_MAX_TURN",
                        VarValue = "5",
                        VarDesc = "Số lượt nhập opt cho phép"
                    }
                );

            modelBuilder
                .Entity<SysVar>()
                .HasData(
                    new SysVar
                    {
                        Id = 6,
                        GrName = GrNames.USER_FORGOT_PASSWORD,
                        VarName = VarNames.USER_FORGOT_PASSWORD_LINK_EXPITE_TIME,
                        VarValue = "15",
                        VarDesc = "Thời gian hết hạn link nhập mật khẩu mới"
                    }
                );
            #endregion
        }
    }
}
