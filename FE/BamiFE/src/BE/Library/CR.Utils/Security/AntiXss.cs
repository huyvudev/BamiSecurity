using Ganss.Xss;

namespace CR.Utils.Security;

/// <summary>
/// anti xss
/// </summary>
public static class AntiXss
{
    /// <summary>
    /// Xử lý loại bỏ mã nguy hiểm sử dụng thư viện Ganss.Xss (https://github.com/mganss/HtmlSanitizer)
    /// </summary>
    /// <param name="userInput"></param>
    /// <returns></returns>
    public static string Sanitize(string userInput)
    {
        var sanitizer = new HtmlSanitizer();
        sanitizer.AllowedTags.Add("iframe"); //cho phép thẻ iframe

        sanitizer.AllowedSchemes.Add("mailto");
        return sanitizer.Sanitize(userInput);
    }
}
