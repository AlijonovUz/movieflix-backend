using System.Text.RegularExpressions;

namespace MovieFlix.Application.Common.Helpers;

public static class SlugHelper
{
    public static string GenerateSlug(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
            return string.Empty;
        
        var str = text.Trim().ToLowerInvariant();
        str = Regex.Replace(str, @"\s+", "-");
        str = Regex.Replace(str, @"[^a-z0-9\-]", "");
        return str.Trim('-');
    }

    public static string ResolveSlug(string? customSlug, string fallbackName) 
        => string.IsNullOrWhiteSpace(customSlug)
            ? GenerateSlug(fallbackName)
            : GenerateSlug(customSlug);
    
}