namespace CR.Constants.ErrorCodes
{
    public class PayErrorCode : ErrorCode
    {
        protected PayErrorCode()
            : base() { }

        //public const int <Tên Error> = <giá trị>;
        public const int PayPaymentNotFound = 6000;
        public const int PayPaymentWrongStatus = 6001;
        public const int PayPaymentExisted = 6002;
        public const int PayWalletNotFound = 6003;
        public const int PayWalletExisted = 6004;
        public const int PayWalletAmountMoneyNotEnough = 6005;
    }
}
