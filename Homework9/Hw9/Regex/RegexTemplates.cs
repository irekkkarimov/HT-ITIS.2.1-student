namespace Hw9.Regex;
using System.Text.RegularExpressions;

public static class RegexTemplates
{
    public static readonly Regex SplitDelimiter = new Regex("(?<=[-+*/()])|(?=[-+*/()])");
    public static readonly Regex NumberPattern = new Regex(@"^\d+");
}