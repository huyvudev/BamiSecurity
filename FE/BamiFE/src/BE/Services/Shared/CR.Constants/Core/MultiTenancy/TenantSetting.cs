using System.Collections.Immutable;

namespace CR.Constants.Core.MultiTenancy;

public static class TenantSetting
{
    public const string DefaultRegDateTimeFormat = "yyyy/MM/dd HH:mm:ss";
    public const string DefaultRegTimeZone = "UTC";

    public const string CouRatingVisibility = "courseRatingVisibility";

    public const string PagNumCourse = "pagingNumCourse";
    public const string PagNumMyCourse = "pagingNumMyCourse";
    public const string PagNumCategory = "pagingNumCategory";
    public const string PagNumTag = "pagingNumTag";

    public const string ApproveAuto = "approveAuto";
    public const string ApproveAdmin = "approveAdmin";
    public const string ApproveWebhook = "approveWebhook";

    public const string RegTimeZone = "regTimeZone";
    public const string RegLanguage = "regLanguage";
    public const string RegDateTimeFormat = "regDateTimeFormat";
    public const string RegDateFormat = "regDateFormat";
    public const string RegTimeFormat = "regTimeFormat";
    public const string RegFirstDayOfWeek = "regFirstDayOfWeek";

    public static IImmutableDictionary<string, string?> DefaultSettings =>
        new Dictionary<string, string?>()
        {
            { CouRatingVisibility, "true" },
            { PagNumCourse, DefaultPaging.NumCourse.ToString() },
            { PagNumMyCourse, DefaultPaging.NumMyCourse.ToString() },
            { PagNumCategory, DefaultPaging.NumCategory.ToString() },
            { PagNumTag,  DefaultPaging.NumTag.ToString() },
            { ApproveAuto, "true" },
            { ApproveAdmin, "false" },
            { ApproveWebhook, "false" },
            { RegTimeZone, null },
            { RegLanguage, "en" },
            { RegDateTimeFormat, DefaultRegDateTimeFormat },
            { RegDateFormat, "yyyy/MM/dd" },
            { RegTimeFormat, "HH:mm:ss" },
            { RegFirstDayOfWeek, "0" }
        }.ToImmutableDictionary();
}
