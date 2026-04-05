namespace CR.Constants.Core.MultiTenancy
{
    /// <summary>
    /// Định danh chức năng (được thiết lập ngưỡng bằng đo đếm)
    /// </summary>
    public enum Features
    {
        /// <summary>
        /// Số lượng user, đơn vị "Limit": số nguyên dương
        /// </summary>
        NumUser = 1,
        /// <summary>
        /// Số lượng khoá học, đơn vị "Limit": nguyên dương
        /// </summary>
        NumCourse = 2,
        /// <summary>
        /// Dung lượng lưu trữ, đơn vị "Limit": MB
        /// </summary>
        Storage = 3,
        /// <summary>
        /// Tuỳ chỉnh tên miền, không có đơn vị "Limit", nếu "Plan" có feature này tức là có cho tuỳ chỉnh
        /// </summary>
        EditDomain = 4,
    }
}
