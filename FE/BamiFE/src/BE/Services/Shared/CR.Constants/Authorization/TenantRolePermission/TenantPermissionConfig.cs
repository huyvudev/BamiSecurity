using System.Collections.Immutable;
using CR.Constants.Authorization.RolePermission;
using CR.Constants.Authorization.RolePermission.Constant;

namespace CR.Constants.Authorization.TenantRolePermission;

/// <summary>
/// Cấu hình thông tin các quyền
/// </summary>
public static class TenantPermissionConfig
{
    public static readonly ImmutableDictionary<string, PermissionContent> Configs = Init();

    private static ImmutableDictionary<string, PermissionContent> Init()
    {
        var configs = new Dictionary<string, PermissionContent>
        {
            { TenantPermissionKeys.WebAdmin, new(nameof(TenantPermissionKeys.WebAdmin)) },
            { TenantPermissionKeys.MenuDashboard, new(nameof(TenantPermissionKeys.MenuDashboard)) },
            #region user
            { TenantPermissionKeys.UserMenu, new(nameof(TenantPermissionKeys.UserMenu)) },
            { TenantPermissionKeys.UserBtnAdd, new(nameof(TenantPermissionKeys.UserBtnAdd), PermissionIcons.IconDefault, TenantPermissionKeys.UserMenu) },
            { TenantPermissionKeys.UserBtnUpdate, new(nameof(TenantPermissionKeys.UserBtnUpdate), PermissionIcons.IconDefault, TenantPermissionKeys.UserMenu) },
            { TenantPermissionKeys.UserBtnSetPassword, new(nameof(TenantPermissionKeys.UserBtnSetPassword), PermissionIcons.IconDefault, TenantPermissionKeys.UserMenu) },
            { TenantPermissionKeys.UserBtnView, new(nameof(TenantPermissionKeys.UserBtnView), PermissionIcons.IconDefault, TenantPermissionKeys.UserMenu) },
            { TenantPermissionKeys.UserBtnExport, new(nameof(TenantPermissionKeys.UserBtnExport), PermissionIcons.IconDefault, TenantPermissionKeys.UserMenu) },
            { TenantPermissionKeys.UserBtnActiveDeactive, new(nameof(TenantPermissionKeys.UserBtnActiveDeactive), PermissionIcons.IconDefault, TenantPermissionKeys.UserMenu) },
            { TenantPermissionKeys.UserBtnRemove, new(nameof(TenantPermissionKeys.UserBtnRemove), PermissionIcons.IconDefault, TenantPermissionKeys.UserMenu) },
            #endregion

            #region content
            { TenantPermissionKeys.QuestionPoolMenu,new(nameof(TenantPermissionKeys.QuestionPoolMenu)) },
            { TenantPermissionKeys.QuestionPoolBtnAdd,new(nameof(TenantPermissionKeys.QuestionPoolBtnAdd), PermissionIcons.IconDefault, TenantPermissionKeys.QuestionPoolMenu) },
            { TenantPermissionKeys.QuestionPoolBtnUpdate,new(nameof(TenantPermissionKeys.QuestionPoolBtnUpdate), PermissionIcons.IconDefault, TenantPermissionKeys.QuestionPoolMenu) },
            { TenantPermissionKeys.QuestionPoolBtnView,new(nameof(TenantPermissionKeys.QuestionPoolBtnView), PermissionIcons.IconDefault, TenantPermissionKeys.QuestionPoolMenu) },
            { TenantPermissionKeys.QuestionPoolBtnExport,new(nameof(TenantPermissionKeys.QuestionPoolBtnExport), PermissionIcons.IconDefault, TenantPermissionKeys.QuestionPoolMenu) },
            { TenantPermissionKeys.QuestionPoolBtnActiveDeactive,new(nameof(TenantPermissionKeys.QuestionPoolBtnActiveDeactive), PermissionIcons.IconDefault, TenantPermissionKeys.QuestionPoolMenu) },
            { TenantPermissionKeys.QuestionPoolBtnRemove,new(nameof(TenantPermissionKeys.QuestionPoolBtnRemove), PermissionIcons.IconDefault, TenantPermissionKeys.QuestionPoolMenu) },
            { TenantPermissionKeys.QuestionPoolBtnClone,new(nameof(TenantPermissionKeys.QuestionPoolBtnClone), PermissionIcons.IconDefault, TenantPermissionKeys.QuestionPoolMenu) },

            { TenantPermissionKeys.ExamMenu, new(nameof(TenantPermissionKeys.ExamMenu)) },
            { TenantPermissionKeys.ExamBtnAdd, new(nameof(TenantPermissionKeys.ExamBtnAdd), PermissionIcons.IconDefault, TenantPermissionKeys.ExamMenu) },
            { TenantPermissionKeys.ExamBtnUpdate, new(nameof(TenantPermissionKeys.ExamBtnUpdate), PermissionIcons.IconDefault, TenantPermissionKeys.ExamMenu) },
            { TenantPermissionKeys.ExamBtnView, new(nameof(TenantPermissionKeys.ExamBtnView), PermissionIcons.IconDefault, TenantPermissionKeys.ExamMenu) },
            { TenantPermissionKeys.ExamBtnExport, new(nameof(TenantPermissionKeys.ExamBtnExport), PermissionIcons.IconDefault, TenantPermissionKeys.ExamMenu) },
            { TenantPermissionKeys.ExamBtnActiveDeactive, new(nameof(TenantPermissionKeys.ExamBtnActiveDeactive), PermissionIcons.IconDefault, TenantPermissionKeys.ExamMenu) },
            { TenantPermissionKeys.ExamBtnRemove, new(nameof(TenantPermissionKeys.ExamBtnRemove), PermissionIcons.IconDefault, TenantPermissionKeys.ExamMenu) },

            { TenantPermissionKeys.TrainingMenu, new(nameof(TenantPermissionKeys.TrainingMenu)) },
            { TenantPermissionKeys.TrainingBtnAdd, new(nameof(TenantPermissionKeys.TrainingBtnAdd), PermissionIcons.IconDefault,  TenantPermissionKeys.TrainingMenu) },
            { TenantPermissionKeys.TrainingBtnUpdate, new(nameof(TenantPermissionKeys.TrainingBtnUpdate), PermissionIcons.IconDefault,  TenantPermissionKeys.TrainingMenu) },
            { TenantPermissionKeys.TrainingBtnView, new(nameof(TenantPermissionKeys.TrainingBtnView), PermissionIcons.IconDefault,  TenantPermissionKeys.TrainingMenu) },
            { TenantPermissionKeys.TrainingBtnExport, new(nameof(TenantPermissionKeys.TrainingBtnExport), PermissionIcons.IconDefault,  TenantPermissionKeys.TrainingMenu) },
            { TenantPermissionKeys.TrainingBtnActiveDeactive, new(nameof(TenantPermissionKeys.TrainingBtnActiveDeactive), PermissionIcons.IconDefault,  TenantPermissionKeys.TrainingMenu) },
            { TenantPermissionKeys.TrainingBtnRemove, new(nameof(TenantPermissionKeys.TrainingBtnRemove), PermissionIcons.IconDefault,  TenantPermissionKeys.TrainingMenu) },
            { TenantPermissionKeys.TrainingBtnApproveEnroll, new(nameof(TenantPermissionKeys.TrainingBtnApproveEnroll), PermissionIcons.IconDefault,  TenantPermissionKeys.TrainingMenu) },
            { TenantPermissionKeys.TrainingBtnCancelEnroll, new(nameof(TenantPermissionKeys.TrainingBtnCancelEnroll), PermissionIcons.IconDefault,  TenantPermissionKeys.TrainingMenu) },
            #endregion

            #region setting
            { TenantPermissionKeys.CertificateTemplateMenu, new(nameof(TenantPermissionKeys.CertificateTemplateMenu)) },
            { TenantPermissionKeys.CertificateTemplateBtnAdd, new(nameof(TenantPermissionKeys.CertificateTemplateBtnAdd), PermissionIcons.IconDefault, TenantPermissionKeys.CertificateTemplateMenu ) },
            { TenantPermissionKeys.CertificateTemplateBtnUpdate, new(nameof(TenantPermissionKeys.CertificateTemplateBtnUpdate), PermissionIcons.IconDefault, TenantPermissionKeys.CertificateTemplateMenu ) },
            { TenantPermissionKeys.CertificateTemplateBtnView, new(nameof(TenantPermissionKeys.CertificateTemplateBtnView), PermissionIcons.IconDefault, TenantPermissionKeys.CertificateTemplateMenu ) },
            { TenantPermissionKeys.CertificateTemplateBtnExport, new(nameof(TenantPermissionKeys.CertificateTemplateBtnExport), PermissionIcons.IconDefault, TenantPermissionKeys.CertificateTemplateMenu ) },
            { TenantPermissionKeys.CertificateTemplateBtnActiveDeactive, new(nameof(TenantPermissionKeys.CertificateTemplateBtnActiveDeactive), PermissionIcons.IconDefault, TenantPermissionKeys.CertificateTemplateMenu ) },
            { TenantPermissionKeys.CertificateTemplateBtnRemove, new(nameof(TenantPermissionKeys.CertificateTemplateBtnRemove), PermissionIcons.IconDefault, TenantPermissionKeys.CertificateTemplateMenu ) },

            { TenantPermissionKeys.CategoryMenu, new(nameof(TenantPermissionKeys.CategoryMenu)) },
            { TenantPermissionKeys.CategoryBtnAdd, new(nameof(TenantPermissionKeys.CategoryBtnAdd), PermissionIcons.IconDefault, TenantPermissionKeys.CategoryMenu) },
            { TenantPermissionKeys.CategoryBtnUpdate, new(nameof(TenantPermissionKeys.CategoryBtnUpdate), PermissionIcons.IconDefault, TenantPermissionKeys.CategoryMenu) },
            { TenantPermissionKeys.CategoryBtnView, new(nameof(TenantPermissionKeys.CategoryBtnView), PermissionIcons.IconDefault, TenantPermissionKeys.CategoryMenu) },
            { TenantPermissionKeys.CategoryBtnExport, new(nameof(TenantPermissionKeys.CategoryBtnExport), PermissionIcons.IconDefault, TenantPermissionKeys.CategoryMenu) },
            { TenantPermissionKeys.CategoryBtnActiveDeactive, new(nameof(TenantPermissionKeys.CategoryBtnActiveDeactive), PermissionIcons.IconDefault, TenantPermissionKeys.CategoryMenu) },
            { TenantPermissionKeys.CategoryBtnRemove, new(nameof(TenantPermissionKeys.CategoryBtnRemove), PermissionIcons.IconDefault, TenantPermissionKeys.CategoryMenu) },

            { TenantPermissionKeys.TagMenu, new(nameof(TenantPermissionKeys.TagMenu)) },
            { TenantPermissionKeys.TagBtnAdd, new(nameof(TenantPermissionKeys.TagBtnAdd), PermissionIcons.IconDefault, TenantPermissionKeys.TagMenu) },
            { TenantPermissionKeys.TagBtnUpdate, new(nameof(TenantPermissionKeys.TagBtnUpdate), PermissionIcons.IconDefault, TenantPermissionKeys.TagMenu) },
            { TenantPermissionKeys.TagBtnView, new(nameof(TenantPermissionKeys.TagBtnView), PermissionIcons.IconDefault, TenantPermissionKeys.TagMenu) },
            { TenantPermissionKeys.TagBtnExport, new(nameof(TenantPermissionKeys.TagBtnExport), PermissionIcons.IconDefault, TenantPermissionKeys.TagMenu) },
            { TenantPermissionKeys.TagBtnActiveDeactive, new(nameof(TenantPermissionKeys.TagBtnActiveDeactive), PermissionIcons.IconDefault, TenantPermissionKeys.TagMenu) },
            { TenantPermissionKeys.TagBtnRemove, new(nameof(TenantPermissionKeys.TagBtnRemove), PermissionIcons.IconDefault, TenantPermissionKeys.TagMenu) },

            { TenantPermissionKeys.SkillMenu, new(nameof(TenantPermissionKeys.SkillMenu)) },
            { TenantPermissionKeys.SkillBtnAdd, new(nameof(TenantPermissionKeys.SkillBtnAdd), PermissionIcons.IconDefault, TenantPermissionKeys.SkillMenu) },
            { TenantPermissionKeys.SkillBtnUpdate, new(nameof(TenantPermissionKeys.SkillBtnUpdate), PermissionIcons.IconDefault, TenantPermissionKeys.SkillMenu) },
            { TenantPermissionKeys.SkillBtnView, new(nameof(TenantPermissionKeys.SkillBtnView), PermissionIcons.IconDefault, TenantPermissionKeys.SkillMenu) },
            { TenantPermissionKeys.SkillBtnExport, new(nameof(TenantPermissionKeys.SkillBtnExport), PermissionIcons.IconDefault, TenantPermissionKeys.SkillMenu) },
            { TenantPermissionKeys.SkillBtnActiveDeactive, new(nameof(TenantPermissionKeys.SkillBtnActiveDeactive), PermissionIcons.IconDefault, TenantPermissionKeys.SkillMenu) },
            { TenantPermissionKeys.SkillBtnRemove, new(nameof(TenantPermissionKeys.SkillBtnRemove), PermissionIcons.IconDefault, TenantPermissionKeys.SkillMenu) },

            { TenantPermissionKeys.SkillLevelSetMenu, new(nameof(TenantPermissionKeys.SkillLevelSetMenu)) },
            { TenantPermissionKeys.SkillLevelSetBtnAdd, new(nameof(TenantPermissionKeys.SkillLevelSetBtnAdd), PermissionIcons.IconDefault, TenantPermissionKeys.SkillLevelSetMenu) },
            { TenantPermissionKeys.SkillLevelSetBtnUpdate, new(nameof(TenantPermissionKeys.SkillLevelSetBtnUpdate), PermissionIcons.IconDefault, TenantPermissionKeys.SkillLevelSetMenu) },
            { TenantPermissionKeys.SkillLevelSetBtnView, new(nameof(TenantPermissionKeys.SkillLevelSetBtnView), PermissionIcons.IconDefault, TenantPermissionKeys.SkillLevelSetMenu) },
            { TenantPermissionKeys.SkillLevelSetBtnExport, new(nameof(TenantPermissionKeys.SkillLevelSetBtnExport), PermissionIcons.IconDefault, TenantPermissionKeys.SkillLevelSetMenu) },
            { TenantPermissionKeys.SkillLevelSetBtnActiveDeactive, new(nameof(TenantPermissionKeys.SkillLevelSetBtnActiveDeactive), PermissionIcons.IconDefault, TenantPermissionKeys.SkillLevelSetMenu) },
            { TenantPermissionKeys.SkillLevelSetBtnRemove, new(nameof(TenantPermissionKeys.SkillLevelSetBtnRemove), PermissionIcons.IconDefault, TenantPermissionKeys.SkillLevelSetMenu) },

            { TenantPermissionKeys.NotiTemplateMenu, new(nameof(TenantPermissionKeys.NotiTemplateMenu)) },
            { TenantPermissionKeys.NotiTemplateBtnAdd, new(nameof(TenantPermissionKeys.NotiTemplateBtnAdd), PermissionIcons.IconDefault, TenantPermissionKeys.NotiTemplateMenu) },
            { TenantPermissionKeys.NotiTemplateBtnUpdate, new(nameof(TenantPermissionKeys.NotiTemplateBtnUpdate), PermissionIcons.IconDefault, TenantPermissionKeys.NotiTemplateMenu) },
            { TenantPermissionKeys.NotiTemplateBtnView, new(nameof(TenantPermissionKeys.NotiTemplateBtnView), PermissionIcons.IconDefault, TenantPermissionKeys.NotiTemplateMenu) },
            { TenantPermissionKeys.NotiTemplateBtnExport, new(nameof(TenantPermissionKeys.NotiTemplateBtnExport), PermissionIcons.IconDefault, TenantPermissionKeys.NotiTemplateMenu) },
            { TenantPermissionKeys.NotiTemplateBtnActiveDeactive, new(nameof(TenantPermissionKeys.NotiTemplateBtnActiveDeactive), PermissionIcons.IconDefault, TenantPermissionKeys.NotiTemplateMenu) },
            { TenantPermissionKeys.NotiTemplateBtnRemove, new(nameof(TenantPermissionKeys.NotiTemplateBtnRemove), PermissionIcons.IconDefault, TenantPermissionKeys.NotiTemplateMenu) },

            { TenantPermissionKeys.LMSConfigMenu, new(nameof(TenantPermissionKeys.LMSConfigMenu)) },
            #endregion
        };

        return configs.ToImmutableDictionary();
    }
}
