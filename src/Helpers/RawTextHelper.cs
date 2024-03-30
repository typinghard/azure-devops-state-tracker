using System;
using System.Text.RegularExpressions;

namespace AzureDevopsTracker.Helpers
{
    internal static class RawTextHelper
    {
        public static string HtmlToRawText(this string source)
        {
            source = source.Replace("</div>", "\n");
            source = source.Replace("<li>", "\n• ");
            source = Regex.Replace(source, "<.*?>|&.*?;", string.Empty, RegexOptions.Compiled, TimeSpan.FromMilliseconds(250));
            source = source.Trim().EndsWith(".") ? source[0..^1] : source.Trim();

            return source;
        }
    }
}
