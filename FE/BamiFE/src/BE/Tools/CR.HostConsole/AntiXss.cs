
using Ganss.Xss;

namespace CR.HostConsole;

public class AntiXss
{
    public static void Test()
    {
        string userInput = """
            <h1>Hello World</h1>
            <script>alert('XSS')</script>
            <img src='https://www.google.com'/>
            <iframe src='https://www.google.com'></iframe>
            <iframe src="javascript:alert('Hello from iframe');"></iframe>
            """;
        var sanitizer = new HtmlSanitizer();
        sanitizer.AllowedTags.Add("iframe");
        string sanitizedHtml = sanitizer.Sanitize(userInput);
        Console.WriteLine(sanitizedHtml);
    }
}
