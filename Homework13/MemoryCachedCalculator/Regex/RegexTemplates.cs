namespace MemoryCachedCalculator.Regex;

public static class RegexTemplates
{
    public static readonly System.Text.RegularExpressions.Regex SplitDelimiter = new System.Text.RegularExpressions.Regex("(?<=[-+*/()])|(?=[-+*/()])");
    public static readonly System.Text.RegularExpressions.Regex NumberPattern = new System.Text.RegularExpressions.Regex(@"^\d+");
}