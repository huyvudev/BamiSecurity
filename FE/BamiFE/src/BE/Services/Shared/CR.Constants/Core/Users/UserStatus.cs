namespace CR.Constants.Core.Users
{
    public static class UserStatus
    {
        /// <summary>
        /// Hoạt động
        /// </summary>
        public const int ACTIVE = 1;

        /// <summary>
        /// Đang khóa
        /// </summary>
        public const int DEACTIVE = 2;

        /// <summary>
        /// Xóa tài khoản (Xóa trên App)
        /// </summary>
        public const int LOCK = 3;

        /// <summary>
        /// Tạm: Đăng ký tài khoản chưa OTP
        /// </summary>
        public const int TEMP = 4;

        /// <summary>
        /// Tạm OTP : Đăng ký tài khoản đã xác thực OTP
        /// </summary>
        [Obsolete("Bỏ chỉ có 4 thôi")]
        public const int TEMP_OTP = 5;

        public static readonly int[] USER_NOT_VALID = [LOCK, TEMP, TEMP_OTP];
    }
}
