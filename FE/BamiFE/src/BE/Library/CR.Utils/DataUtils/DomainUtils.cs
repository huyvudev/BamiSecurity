namespace CR.Utils.DataUtils;

public static class DomainUtils
{
    /// <summary>
    /// Loại bỏ chỉ còn domain
    /// </summary>
    /// <param name="url"></param>
    /// <returns></returns>
    public static string? GetDomain(this string? url)
    {
        if (string.IsNullOrEmpty(url))
        {
            return null;
        }

        return url.Trim()
            .Replace("http://", string.Empty)
            .Replace("https://", string.Empty)
            .TrimEnd('/');
    }
}
