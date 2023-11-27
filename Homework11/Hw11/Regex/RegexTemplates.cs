namespace Hw11.Regex;

public static class RegexTemplates
{
    public static readonly System.Text.RegularExpressions.Regex SplitDelimiter =
        new("(?<=[-+*/()])|(?=[-+*/()])");

    public static readonly System.Text.RegularExpressions.Regex NumberPattern =
        new(@"^\d+");
}