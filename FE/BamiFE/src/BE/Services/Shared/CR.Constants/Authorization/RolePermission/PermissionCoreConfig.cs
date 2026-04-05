using CR.Constants.Authorization.RolePermission;
using CR.Constants.Authorization.RolePermission.Constant;
using CR.Constants.RolePermission.Constant;

namespace CR.Constants.RolePermission
{
    public static partial class PermissionConfig
    {
        public static readonly Dictionary<string, PermissionContent> CoreConfigs =
            new()
            {
                #region Tổng quan
                {
                    PermissionKeys.CoreMenuDashboard,
                    new(nameof(PermissionKeys.CoreMenuDashboard), PermissionIcons.IconDefault)
                },
                #endregion
                #region Website
                {
                    PermissionKeys.CoreMenuUser,
                    new(nameof(PermissionKeys.CoreMenuUser), PermissionIcons.IconDefault)
                },
                {
                    PermissionKeys.CoreMenuRole,
                    new(
                        nameof(PermissionKeys.CoreMenuRole),
                        PermissionIcons.IconDefault,
                        PermissionKeys.CoreMenuUser
                    )
                },
                {
                    PermissionKeys.CoreButtonAddRole,
                    new(
                        nameof(PermissionKeys.CoreButtonAddRole),
                        PermissionIcons.IconDefault,
                        PermissionKeys.CoreMenuRole
                    )
                },
                {
                    PermissionKeys.CoreButtonUpdateRole,
                    new(
                        nameof(PermissionKeys.CoreButtonUpdateRole),
                        PermissionIcons.IconDefault,
                        PermissionKeys.CoreMenuRole
                    )
                },
                {
                    PermissionKeys.CoreButtonActiveDeactiveRole,
                    new(
                        nameof(PermissionKeys.CoreButtonActiveDeactiveRole),
                        PermissionIcons.IconDefault,
                        PermissionKeys.CoreMenuRole
                    )
                },
                {
                    PermissionKeys.CoreButtonDeleteRole,
                    new(
                        nameof(PermissionKeys.CoreButtonDeleteRole),
                        PermissionIcons.IconDefault,
                        PermissionKeys.CoreMenuRole
                    )
                },
                //Quản lý tài khoản
                {
                    PermissionKeys.CoreMenuAccount,
                    new(
                        nameof(PermissionKeys.CoreMenuAccount),
                        PermissionIcons.IconDefault,
                        PermissionKeys.CoreMenuUser
                    )
                },
                {
                    PermissionKeys.CoreButtonAddAccount,
                    new(
                        nameof(PermissionKeys.CoreButtonAddAccount),
                        PermissionIcons.IconDefault,
                        PermissionKeys.CoreMenuAccount
                    )
                },
                {
                    PermissionKeys.CoreButtonUpdateAccount,
                    new(
                        nameof(PermissionKeys.CoreButtonUpdateAccount),
                        PermissionIcons.IconDefault,
                        PermissionKeys.CoreMenuAccount
                    )
                },
                {
                    PermissionKeys.CoreButtonSetPasswordAccount,
                    new(
                        nameof(PermissionKeys.CoreButtonSetPasswordAccount),
                        PermissionIcons.IconDefault,
                        PermissionKeys.CoreMenuAccount
                    )
                },
                {
                    PermissionKeys.CoreButtonActiveDeactiveAccount,
                    new(
                        nameof(PermissionKeys.CoreButtonActiveDeactiveAccount),
                        PermissionIcons.IconDefault,
                        PermissionKeys.CoreMenuAccount
                    )
                },
                {
                    PermissionKeys.CoreButtonDeleteAccount,
                    new(
                        nameof(PermissionKeys.CoreButtonDeleteAccount),
                        PermissionIcons.IconDefault,
                        PermissionKeys.CoreMenuAccount
                    )
                },
                #endregion
            };
    }
}
