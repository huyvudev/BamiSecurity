namespace CR.Constants.ErrorCodes
{
    public class CoreErrorCode : ErrorCode
    {
        protected CoreErrorCode()
            : base() { }

        //Otp
        public const int CoreSendOtpMultipleRequest = 3001;
        public const int CoreVerifyOtpNotFound = 3002;
        public const int CoreOtpIsVerified = 3003;
        public const int CoreVerifyOtpTurnEnd = 3004;
        public const int CoreVerifyOtpTimeout = 3005;
        public const int CoreOtpIsInvalid = 3006;

        public const int CoreUserForgotLinkIsExpired = 3007;
        public const int CoreUserForgotLinkIsInvalid = 3008;

        //Excel
        public const int CoreImportExcelError = 3009;
        public const int CoreImportExcelErrorWorksheetNull = 3010;

        //sku module
        public const int CoreProductMethodCodeExisted = 3011;
        public const int CoreProductMethodNotFound = 3012;
        public const int CoreSkuCodeExisted = 3013;
        public const int CoreSkuNotFound = 3014;
        public const int CoreSkuCodeError = 3015;
        public const int CoreMaterialCodeExisted = 3016;
        public const int CoreMaterialNotFound = 3017;
        public const int CoreSkuBaseNotFound = 3018;
        public const int CoreSkuBaseCodeExisted = 3019;
        public const int CoreSkuSizeNameExisted = 3020;
        public const int CoreSkuSizeNotFound = 3021;
        public const int CoreSkuSizePkgMockupNotFound = 3022;

        //brand
        public const int CoreBrandNameHasBeenUsed = 3023;
        public const int CoreBrandNotFound = 3024;

        //store
        public const int CoreStoreNameHasBeenUsed = 3025;
        public const int CoreStoreNotFound = 3026;
        public const int CoreBrandDoesNotExist = 3027;

        //PartnerType
        public const int CorePartnerTypeNameHasBeenUsed = 3028;
        public const int CorePartnerTypeNotFound = 3029;
        public const int CorePartnerTypeDoesNotExist = 3030;

        //Partner
        public const int CorePartnerNotFound = 3031;
        public const int CorePartnerNameHasBeenUsed = 3032;

        //Order
        public const int CoreOrderNotFound = 3033;
        public const int CoreOrderNumberHasBeenUsed = 3034;
        public const int CoreOrderDetailNotFound = 3035;
        public const int CoreLackOfBasicInformationForOrderDetail = 3036;
        public const int CoreOrderDetailApproved = 3037;
        public const int CoreOrderItemNotFound = 3038;
        public const int CoreOrderCantDelete = 3039;
        public const int CoreOrderDetailNotDone = 3040;
        public const int CoreOrderPushed = 3041;
        public const int CoreSkuSizeCreateFailed = 3042;
        public const int CoreSkuMockUpCreateFailed = 3043;
        public const int CoreBatchMustNotEmpty = 3044;
        public const int CoreBatchNotFound = 3045;
        public const int CoreOrderItemsDifferentSku = 3046;
        public const int CoreOrderItemInvalidOrInOtherBatch = 3047;
        public const int CoreBatchInvalid = 3048;
        public const int CoreOrderItemStatusCannotRevert = 3049;
        public const int CoreOrderItemNotInBatch = 3050;
    }
}
