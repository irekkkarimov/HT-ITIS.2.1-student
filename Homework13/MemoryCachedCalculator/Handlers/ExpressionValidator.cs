using System.Diagnostics.CodeAnalysis;
using MemoryCachedCalculator.ErrorMessages;
using MemoryCachedCalculator.Regex;

namespace MemoryCachedCalculator.Handlers;

[ExcludeFromCodeCoverage]
public class ExpressionValidator
{
    public static (bool, string) Validate(string expression)
    {
        // Check for null or empty
        if (string.IsNullOrEmpty(expression))
            return (false, MathErrorMessager.EmptyString);

        // Checking for allowed characters
        if (ContainsOnlyAllowedCharacters(expression) is (false, var errorChar))
            return (false, MathErrorMessager.UnknownCharacterMessage(errorChar));

        // Correct bracket sequence
        if (!ContainsCorrectBracketPairs(expression))
            return (false, MathErrorMessager.IncorrectBracketsNumber);

        // Checking if operands are numbers(double)
        if (OnlyNumbers(expression) is (false, var errorNumber))
            return (false, MathErrorMessager.NotNumberMessage(errorNumber));

        // Not starting with operation
        if (!NotStartingWithOperation(expression))
            return (false, MathErrorMessager.StartingWithOperation);

        // Not ending with operation
        if (!NotEndingWithExpression(expression))
            return (false, MathErrorMessager.EndingWithOperation);

        // Complex validation including different errors
        if (ComplexValidation(expression) is (false, var errorMessage))
            return (false, errorMessage);

        // Return empty value if validation passed
        return (true, "");
    }

    private static (bool, char) ContainsOnlyAllowedCharacters(string expression)
    {
        var characters = new[]
        {
            '(',
            ')',
            '+',
            '-',
            '*',
            '/',
            '.',
            ','
        };

        foreach (var item in
                 expression
                     .Where(i => i != ' ')
                     .Where(item => !characters.Contains(item) && !char.IsDigit(item)))
        {
            return (false, item);
        }

        return (true, '.');
    }

    private static bool ContainsCorrectBracketPairs(string expression)
    {
        var stack = new Stack<char>();

        foreach (var character in expression)
        {
            switch (character)
            {
                case '(':
                    stack.Push(character);
                    break;
                case ')' when !stack.Any():
                    return false;
                case ')':
                    stack.Pop();
                    break;
            }
        }

        return !stack.Any();
    }

    private static bool NotStartingWithOperation(string expression)
    {
        var expressionWithoutWhiteSpaces = expression.Trim();
        switch (expressionWithoutWhiteSpaces.First())
        {
            case '(':
            case ')':
                return true;
        }

        return char.IsDigit(expressionWithoutWhiteSpaces.First());
    }

    private static bool NotEndingWithExpression(string expression)
    {
        var expressionWithoutWhiteSpaces = expression.Trim();
        switch (expressionWithoutWhiteSpaces.Last())
        {
            case '(':
            case ')':
                return true;
        }

        return char.IsDigit(expressionWithoutWhiteSpaces.Last());
    }

    private static (bool, string) OnlyNumbers(string expression)
    {
        var operations = new[] { "(", ")", "+", "-", "*", "/" };

        var expressionWithoutOperators = RegexTemplates.SplitDelimiter.Split(expression)
            .Where(i => !operations.Contains(i))
            .Select(i => i.Replace(" ", ""))
            .Where(i => i != "")
            .ToArray();


        foreach (var item in expressionWithoutOperators)
        {
            if (!double.TryParse(item, out _))
                return (false, item);
        }

        return (true, "");
    }

    private static (bool, string) ComplexValidation(string expression)
    {
        var expressionSplitted = RegexTemplates.SplitDelimiter.Split(expression)
            .Select(i => i.Replace(" ", ""))
            .Where(i => i != "")
            .ToArray();
        var isPreviousOperation = false;
        var isPreviousNumber = false;
        var isPreviousOpenParenthesis = false;
        var isPreviousCloseParenthesis = false;
        var previousOperation = "";

        foreach (var element in expressionSplitted)
        {
            switch (element)
            {
                case "(":
                {
                    isPreviousOpenParenthesis = true;
                    isPreviousNumber = false;
                    isPreviousOperation = false;
                    isPreviousCloseParenthesis = false;
                    break;
                }
                case ")":
                {
                    if (isPreviousOperation)
                        return (false, MathErrorMessager.OperationBeforeParenthesisMessage(previousOperation));

                    isPreviousCloseParenthesis = true;
                    isPreviousOpenParenthesis = false;
                    isPreviousNumber = false;
                    isPreviousOperation = false;
                    break;
                }
                default:
                {
                    if (double.TryParse(element, out _))
                    {
                        isPreviousCloseParenthesis = false;
                        isPreviousNumber = true;
                        isPreviousOperation = false;
                        isPreviousOpenParenthesis = false;
                    }
                    else
                    {
                        if (isPreviousOpenParenthesis && element != "-")
                            return (false, MathErrorMessager.InvalidOperatorAfterParenthesisMessage(element));

                        if (!isPreviousNumber && !isPreviousCloseParenthesis)
                        {
                            if (isPreviousOpenParenthesis && element == "-")
                                continue;
                            return (false, MathErrorMessager.TwoOperationInRowMessage(previousOperation, element));
                        }

                        isPreviousOperation = true;
                        isPreviousNumber = false;
                        isPreviousCloseParenthesis = false;
                        isPreviousOpenParenthesis = false;
                        previousOperation = element;
                    }

                    break;
                }
            }
        }

        return (true, "");
    }
}