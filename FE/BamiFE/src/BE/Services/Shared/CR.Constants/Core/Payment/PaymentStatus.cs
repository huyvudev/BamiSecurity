namespace CR.Constants.Core.Payment
{
    /// <summary>
    /// Trạng thái của thanh toán
    /// </summary>
    public static class PaymentStatus
    {
        /// <summary>
        /// Khởi tạo
        /// </summary>
        public const int INIT = 1;
        /// <summary>
        /// Đã thanh toán
        /// </summary>
        public const int PAY_SUCCESS = 2;
        /// <summary>
        /// Hủy thanh toán
        /// </summary>
        public const int PAY_CANCEL = 3;
    }
}
