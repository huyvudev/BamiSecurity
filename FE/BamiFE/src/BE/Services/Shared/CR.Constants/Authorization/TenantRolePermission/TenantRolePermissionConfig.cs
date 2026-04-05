using System.Collections.Immutable;
using CR.Constants.Authorization.Tenant;

namespace CR.Constants.Authorization.TenantRolePermission;

/// <summary>
/// Quyền cho từng role của tenant
/// </summary>
public static class TenantRolePermissionConfig
{
    public static readonly ImmutableDictionary<TenantRoleFix, HashSet<string>> Configs = Init();

    private static ImmutableDictionary<TenantRoleFix, HashSet<string>> Init()
    {
        var configs = new Dictionary<TenantRoleFix, HashSet<string>>
        {
            {
                TenantRoleFix.Administrator,
                new()
                {
                    TenantPermissionKeys.WebAdmin,
                    TenantPermissionKeys.MenuDashboard,
                    //  User
                    TenantPermissionKeys.UserMenu,
                    TenantPermissionKeys.UserBtnAdd,
                    TenantPermissionKeys.UserBtnUpdate,
                    TenantPermissionKeys.UserBtnSetPassword,
                    TenantPermissionKeys.UserBtnView,
                    TenantPermissionKeys.UserBtnExport,
                    TenantPermissionKeys.UserBtnActiveDeactive,
                    TenantPermissionKeys.UserBtnRemove,

                    // Exam
                    TenantPermissionKeys.ExamMenu,
                    TenantPermissionKeys.ExamBtnAdd,
                    TenantPermissionKeys.ExamBtnUpdate,
                    TenantPermissionKeys.ExamBtnView,
                    TenantPermissionKeys.ExamBtnExport,
                    TenantPermissionKeys.ExamBtnActiveDeactive,
                    TenantPermissionKeys.ExamBtnRemove,
                    // QuestionPool
                    TenantPermissionKeys.QuestionPoolMenu,
                    TenantPermissionKeys.QuestionPoolBtnAdd,
                    TenantPermissionKeys.QuestionPoolBtnUpdate,
                    TenantPermissionKeys.QuestionPoolBtnView,
                    TenantPermissionKeys.QuestionPoolBtnExport,
                    TenantPermissionKeys.QuestionPoolBtnActiveDeactive,
                    TenantPermissionKeys.QuestionPoolBtnRemove,
                    TenantPermissionKeys.QuestionPoolBtnClone,
                    // Training
                    TenantPermissionKeys.TrainingMenu,
                    TenantPermissionKeys.TrainingBtnAdd,
                    TenantPermissionKeys.TrainingBtnUpdate,
                    TenantPermissionKeys.TrainingBtnView,
                    TenantPermissionKeys.TrainingBtnExport,
                    TenantPermissionKeys.TrainingBtnActiveDeactive,
                    TenantPermissionKeys.TrainingBtnRemove,
                    TenantPermissionKeys.TrainingBtnApproveEnroll,
                    TenantPermissionKeys.TrainingBtnCancelEnroll,

                    // Setting
                    // CertificateTemplate
                    TenantPermissionKeys.CertificateTemplateMenu,
                    TenantPermissionKeys.CertificateTemplateBtnAdd,
                    TenantPermissionKeys.CertificateTemplateBtnUpdate,
                    TenantPermissionKeys.CertificateTemplateBtnView,
                    TenantPermissionKeys.CertificateTemplateBtnExport,
                    TenantPermissionKeys.CertificateTemplateBtnActiveDeactive,
                    TenantPermissionKeys.CertificateTemplateBtnRemove,
                    // Category
                    TenantPermissionKeys.CategoryMenu,
                    TenantPermissionKeys.CategoryBtnAdd,
                    TenantPermissionKeys.CategoryBtnUpdate,
                    TenantPermissionKeys.CategoryBtnView,
                    TenantPermissionKeys.CategoryBtnExport,
                    TenantPermissionKeys.CategoryBtnActiveDeactive,
                    TenantPermissionKeys.CategoryBtnRemove,
                    // Tag
                    TenantPermissionKeys.TagMenu,
                    TenantPermissionKeys.TagBtnAdd,
                    TenantPermissionKeys.TagBtnUpdate,
                    TenantPermissionKeys.TagBtnView,
                    TenantPermissionKeys.TagBtnExport,
                    TenantPermissionKeys.TagBtnActiveDeactive,
                    TenantPermissionKeys.TagBtnRemove,
                    // Skill
                    TenantPermissionKeys.SkillMenu,
                    TenantPermissionKeys.SkillBtnAdd,
                    TenantPermissionKeys.SkillBtnUpdate,
                    TenantPermissionKeys.SkillBtnView,
                    TenantPermissionKeys.SkillBtnExport,
                    TenantPermissionKeys.SkillBtnActiveDeactive,
                    TenantPermissionKeys.SkillBtnRemove,
                    // Skill Level Set
                    TenantPermissionKeys.SkillLevelSetMenu,
                    TenantPermissionKeys.SkillLevelSetBtnAdd,
                    TenantPermissionKeys.SkillLevelSetBtnUpdate,
                    TenantPermissionKeys.SkillLevelSetBtnView,
                    TenantPermissionKeys.SkillLevelSetBtnExport,
                    TenantPermissionKeys.SkillLevelSetBtnActiveDeactive,
                    TenantPermissionKeys.SkillLevelSetBtnRemove,
                    // NotiTemplate
                    TenantPermissionKeys.NotiTemplateMenu,
                    TenantPermissionKeys.NotiTemplateBtnAdd,
                    TenantPermissionKeys.NotiTemplateBtnUpdate,
                    TenantPermissionKeys.NotiTemplateBtnView,
                    TenantPermissionKeys.NotiTemplateBtnExport,
                    TenantPermissionKeys.NotiTemplateBtnActiveDeactive,
                    TenantPermissionKeys.NotiTemplateBtnRemove,
                    // LMSConfig
                    TenantPermissionKeys.LMSConfigMenu,
                }
            },
            {
                TenantRoleFix.Trainer,
                new()
                {
                    TenantPermissionKeys.WebAdmin,
                    TenantPermissionKeys.MenuDashboard,
                    // Exam
                    TenantPermissionKeys.ExamMenu,
                    TenantPermissionKeys.ExamBtnAdd,
                    TenantPermissionKeys.ExamBtnUpdate,
                    TenantPermissionKeys.ExamBtnView,
                    TenantPermissionKeys.ExamBtnExport,
                    TenantPermissionKeys.ExamBtnActiveDeactive,
                    TenantPermissionKeys.ExamBtnRemove,
                    // QuestionPool
                    TenantPermissionKeys.QuestionPoolMenu,
                    TenantPermissionKeys.QuestionPoolBtnAdd,
                    TenantPermissionKeys.QuestionPoolBtnUpdate,
                    TenantPermissionKeys.QuestionPoolBtnView,
                    TenantPermissionKeys.QuestionPoolBtnExport,
                    TenantPermissionKeys.QuestionPoolBtnActiveDeactive,
                    TenantPermissionKeys.QuestionPoolBtnRemove,
                    TenantPermissionKeys.QuestionPoolBtnClone,
                    // Training
                    TenantPermissionKeys.TrainingMenu,
                    TenantPermissionKeys.TrainingBtnAdd,
                    TenantPermissionKeys.TrainingBtnUpdate,
                    TenantPermissionKeys.TrainingBtnView,
                    TenantPermissionKeys.TrainingBtnExport,
                    TenantPermissionKeys.TrainingBtnActiveDeactive,
                    TenantPermissionKeys.TrainingBtnRemove,
                    TenantPermissionKeys.TrainingBtnApproveEnroll,
                    TenantPermissionKeys.TrainingBtnCancelEnroll,
                }
            },
            {
                TenantRoleFix.CatalogManager,
                new()
                {
                    TenantPermissionKeys.WebAdmin,
                    TenantPermissionKeys.MenuDashboard,
                    // Setting
                    // CertificateTemplate
                    TenantPermissionKeys.CertificateTemplateMenu,
                    TenantPermissionKeys.CertificateTemplateBtnAdd,
                    TenantPermissionKeys.CertificateTemplateBtnUpdate,
                    TenantPermissionKeys.CertificateTemplateBtnView,
                    TenantPermissionKeys.CertificateTemplateBtnExport,
                    TenantPermissionKeys.CertificateTemplateBtnActiveDeactive,
                    TenantPermissionKeys.CertificateTemplateBtnRemove,
                    // Category
                    TenantPermissionKeys.CategoryMenu,
                    TenantPermissionKeys.CategoryBtnAdd,
                    TenantPermissionKeys.CategoryBtnUpdate,
                    TenantPermissionKeys.CategoryBtnView,
                    TenantPermissionKeys.CategoryBtnExport,
                    TenantPermissionKeys.CategoryBtnActiveDeactive,
                    TenantPermissionKeys.CategoryBtnRemove,
                    // Tag
                    TenantPermissionKeys.TagMenu,
                    TenantPermissionKeys.TagBtnAdd,
                    TenantPermissionKeys.TagBtnUpdate,
                    TenantPermissionKeys.TagBtnView,
                    TenantPermissionKeys.TagBtnExport,
                    TenantPermissionKeys.TagBtnActiveDeactive,
                    TenantPermissionKeys.TagBtnRemove,
                    // Skill
                    TenantPermissionKeys.SkillMenu,
                    TenantPermissionKeys.SkillBtnAdd,
                    TenantPermissionKeys.SkillBtnUpdate,
                    TenantPermissionKeys.SkillBtnView,
                    TenantPermissionKeys.SkillBtnExport,
                    TenantPermissionKeys.SkillBtnActiveDeactive,
                    TenantPermissionKeys.SkillBtnRemove,
                    // Skill Level Set
                    TenantPermissionKeys.SkillLevelSetMenu,
                    TenantPermissionKeys.SkillLevelSetBtnAdd,
                    TenantPermissionKeys.SkillLevelSetBtnUpdate,
                    TenantPermissionKeys.SkillLevelSetBtnView,
                    TenantPermissionKeys.SkillLevelSetBtnExport,
                    TenantPermissionKeys.SkillLevelSetBtnActiveDeactive,
                    TenantPermissionKeys.SkillLevelSetBtnRemove,
                    // NotiTemplate
                    TenantPermissionKeys.NotiTemplateMenu,
                    TenantPermissionKeys.NotiTemplateBtnAdd,
                    TenantPermissionKeys.NotiTemplateBtnUpdate,
                    TenantPermissionKeys.NotiTemplateBtnView,
                    TenantPermissionKeys.NotiTemplateBtnExport,
                    TenantPermissionKeys.NotiTemplateBtnActiveDeactive,
                    TenantPermissionKeys.NotiTemplateBtnRemove,
                    // LMSConfig
                    TenantPermissionKeys.LMSConfigMenu,
                }
            },
            {
                TenantRoleFix.Learner,
                new() { }
            }
        };

        return configs.ToImmutableDictionary();
    }
}
