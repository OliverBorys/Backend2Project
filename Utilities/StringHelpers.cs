using System.Text.RegularExpressions;

namespace Backend2Project.Utilities;

public static class StringHelpers
{
    private static readonly Regex SlugRegex = new("[^a-z0-9]+", RegexOptions.Compiled);

    public static string Slugify(this string s) =>
        SlugRegex.Replace(s.Trim().ToLowerInvariant(), "-").Trim('-');

    public static string ToSlug(string s) => Slugify(s);
}