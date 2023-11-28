namespace ComponentHub.Domain.Core.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Non reversible normalization of strings for comparisons.
    /// </summary>
    /// <param name="s">The string to normalize.</param>
    /// <returns>The normalized string.</returns>
    public static string NormalizeString(this string s)
    {
        return s.Trim().Replace("\n", "").Replace("\t", "").Replace("\r", "").Replace(" ", "");
    }
}