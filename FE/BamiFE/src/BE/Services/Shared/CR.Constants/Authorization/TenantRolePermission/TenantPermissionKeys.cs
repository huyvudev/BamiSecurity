namespace CR.Constants.Authorization.TenantRolePermission;

public static class TenantPermissionKeys
{
    #region các loại permission
    private const string Menu = "tenant_menu_";
    private const string Tab = "tenant_tab_";
    private const string Table = "tenant_table_";
    private const string Button = "tenant_btn_";
    private const string Input = "tenant_input_";
    #endregion

    /// <summary>
    /// Có được phép vào trang web admin không
    /// </summary>
    public const string WebAdmin = "tenant_web_admin";

    public const string MenuDashboard = $"{Menu}dashboard";

    
    /* Tài khoản */
    public const string UserMenu = $"{Menu}user";
    public const string UserBtnAdd = $"{Button}add_user";
    public const string UserBtnUpdate = $"{Button}update_user";
    public const string UserBtnSetPassword = $"{Button}set_password_user";
    public const string UserBtnView = $"{Button}view_user";
    public const string UserBtnExport = $"{Button}export_users";
    public const string UserBtnActiveDeactive = $"{Button}active_deactive_user";
    public const string UserBtnRemove = $"{Button}remove_user";

    // Trắc Nghiệm
    public const string ExamMenu = $"{Menu}exam";
    public const string ExamBtnAdd = $"{Button}add_exam";
    public const string ExamBtnUpdate = $"{Button}update_exam";
    public const string ExamBtnView = $"{Button}view_exam";
    public const string ExamBtnExport = $"{Button}export_exams";
    public const string ExamBtnActiveDeactive = $"{Button}active_deactive_exam";
    public const string ExamBtnRemove = $"{Button}remove_exam";

    // Kho câu hỏi
    public const string QuestionPoolMenu = $"{Menu}question_pool";
    public const string QuestionPoolBtnAdd = $"{Button}add_question_pool";
    public const string QuestionPoolBtnUpdate = $"{Button}update_question_pool";
    public const string QuestionPoolBtnView = $"{Button}view_question_pool";
    public const string QuestionPoolBtnExport = $"{Button}export_question_pools";
    public const string QuestionPoolBtnActiveDeactive = $"{Button}active_deactive_question_pool";
    public const string QuestionPoolBtnRemove = $"{Button}remove_question_pool";
    public const string QuestionPoolBtnClone = $"{Button}clone_question_pool";

    // Khóa học
    public const string TrainingMenu = $"{Menu}training";
    public const string TrainingBtnAdd = $"{Button}add_training";
    public const string TrainingBtnUpdate = $"{Button}update_training";
    public const string TrainingBtnView = $"{Button}view_training";
    public const string TrainingBtnExport = $"{Button}export_trainings";
    public const string TrainingBtnActiveDeactive = $"{Button}active_deactive_training";
    public const string TrainingBtnRemove = $"{Button}remove_training";
    //
    public const string TrainingBtnApproveEnroll = $"{Button}approve_enroll_training";
    public const string TrainingBtnCancelEnroll = $"{Button}cancel_enroll_training";

    // CÀI ĐẶT
    // Mẫu chứng chỉ
    public const string CertificateTemplateMenu = $"{Menu}certificate_template";
    public const string CertificateTemplateBtnAdd = $"{Button}add_certificate_template";
    public const string CertificateTemplateBtnUpdate = $"{Button}update_certificate_template";
    public const string CertificateTemplateBtnView = $"{Button}view_certificate_template";
    public const string CertificateTemplateBtnExport = $"{Button}export_certificate_templates";
    public const string CertificateTemplateBtnActiveDeactive =$"{Button}active_deactive_certificate_template";
    public const string CertificateTemplateBtnRemove = $"{Button}remove_certificate_template";

    // Danh mục
    public const string CategoryMenu = $"{Menu}category";
    public const string CategoryBtnAdd = $"{Button}add_category";
    public const string CategoryBtnUpdate = $"{Button}update_category";
    public const string CategoryBtnView = $"{Button}view_category";
    public const string CategoryBtnExport = $"{Button}export_categories";
    public const string CategoryBtnActiveDeactive = $"{Button}active_deactive_category";
    public const string CategoryBtnRemove = $"{Button}remove_category";

    // Thẻ
    public const string TagMenu = $"{Menu}tag";
    public const string TagBtnAdd = $"{Button}add_tag";
    public const string TagBtnUpdate = $"{Button}update_tag";
    public const string TagBtnView = $"{Button}view_tag";
    public const string TagBtnExport = $"{Button}export_tags";
    public const string TagBtnActiveDeactive = $"{Button}active_deactive_tag";
    public const string TagBtnRemove = $"{Button}remove_tag";

    // Kỹ năng
    public const string SkillMenu = $"{Menu}skill";
    public const string SkillBtnAdd = $"{Button}add_skill";
    public const string SkillBtnUpdate = $"{Button}update_skill";
    public const string SkillBtnView = $"{Button}view_skill";
    public const string SkillBtnExport = $"{Button}export_skills";
    public const string SkillBtnActiveDeactive = $"{Button}active_deactive_skill";
    public const string SkillBtnRemove = $"{Button}remove_skill";

    // Thang kỹ năng
    public const string SkillLevelSetMenu = $"{Menu}skill_level_set";
    public const string SkillLevelSetBtnAdd = $"{Button}add_skill_level_set";
    public const string SkillLevelSetBtnUpdate = $"{Button}update_skill_level_set";
    public const string SkillLevelSetBtnView = $"{Button}view_skill_level_set";
    public const string SkillLevelSetBtnExport = $"{Button}export_skill_level_sets";
    public const string SkillLevelSetBtnActiveDeactive = $"{Button}active_deactive_skill_level_set";
    public const string SkillLevelSetBtnRemove = $"{Button}remove_skill_level_set";

    // Mẫu thông báo
    public const string NotiTemplateMenu = $"{Menu}noti_template";
    public const string NotiTemplateBtnAdd = $"{Button}add_noti_template";
    public const string NotiTemplateBtnUpdate = $"{Button}update_noti_template";
    public const string NotiTemplateBtnView = $"{Button}view_noti_template";
    public const string NotiTemplateBtnExport = $"{Button}export_noti_templates";
    public const string NotiTemplateBtnActiveDeactive = $"{Button}active_deactive_noti_template";
    public const string NotiTemplateBtnRemove = $"{Button}remove_noti_template";

    // Cấu hình LMS
    public const string LMSConfigMenu = $"{Menu}lms_config";
}
