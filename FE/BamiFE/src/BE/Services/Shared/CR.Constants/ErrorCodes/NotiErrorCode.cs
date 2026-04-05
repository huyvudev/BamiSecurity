namespace CR.Constants.ErrorCodes;

public class NotiErrorCode : ErrorCode
{
    protected NotiErrorCode()
        : base() { }

    //public const int <Tên Error> = <giá trị>;
    public const int NotiTemplateConfigEventKeyExists = 7001;
    public const int NotiTemplateConfigNotFound = 7002;
    public const int NotiTemplateEventKeyExists = 7003;
    public const int NotiTemplateNotFound = 7004;
    public const int NotiTemplateNotFoundFindByEventKey = 7005;
    public const int NotiSmtpConfigNotFoundForSendMail = 7006;
    public const int NotiSmtpConfigNotFound = 7007;
    public const int NotiTemplateNotFoundForSendMail = 7008;
    public const int NotificationNotFound = 7009;
}
