using System.Runtime.CompilerServices;

namespace CR.Utils.DataUtils;

public static class ObjectUtils
{
    /// <summary>
    /// Gets the current method information including file path, member name, and line number.
    /// </summary>
    /// <param name="obj">The object.</param>
    /// <param name="lineNumber">The line number (automatically populated using CallerLineNumber attribute).</param>
    /// <param name="filePath">The file path (automatically populated using CallerFilePath attribute).</param>
    /// <param name="memberName">The member name (automatically populated using CallerMemberName attribute).</param>
    /// <returns>A string containing the trace information.</returns>
    public static string GetCurrentMethodInfo(
        this object obj,
        [CallerLineNumber] int lineNumber = 0,
        [CallerFilePath] string filePath = null!,
        [CallerMemberName] string memberName = null!
    )
    {
        return $"\nTrace: {filePath} {memberName} {lineNumber}\n";
    }
}
