namespace CR.Constants.Authorization.Tenant
{
    /// <summary>
    /// Các loại vai trò fix trong tenant
    /// </summary>
    public enum TenantRoleFix
    {
        /// <summary>
        /// Quản trị hệ thống
        /// </summary>
        Administrator = 0,
        /// <summary>
        /// Giảng viên, người hướng dẫn, quản lý nội dung
        /// </summary>
        Trainer = 1,
        /// <summary>
        /// Quản trị danh mục
        /// </summary>
        CatalogManager = 2,
        /// <summary>
        /// Học viên <br/>
        /// thông báo: trước khi tạo thông báo truyền ngay user id vào recipientIds
        /// </summary>
        Learner = 3
    }
}
