using Hw11.ErrorMessages;
using Hw11.Exceptions;
using Hw11.Regex;

namespace Hw11.Handlers;

public static class ExpressionValidator
{
    public static bool Validate(string expression)
    {
        // Check for null or empty
        if (string.IsNullOrEmpty(expression))
            throw new InvalidSyntaxException(MathErrorMessager.EmptyString);

        // Checking for allowed characters
        if (ContainsOnlyAllowedCharacters(expression) is (false, var errorChar))
            throw new InvalidSymbolException(MathErrorMessager.UnknownCharacterMessage(errorChar));

        // Correct bracket sequence
        if (!ContainsCorrectBracketPairs(expression))
            throw new InvalidSyntaxException(MathErrorMessager.IncorrectBracketsNumber);

        // Checking if operands are numbers(double)
        if (OnlyNumbers(expression) is (false, var errorNumber))
            throw new InvalidNumberException(MathErrorMessager.NotNumberMessage(errorNumber));

        // Not starting with operation
        if (!NotStartingWithOperation(expression))
            throw new InvalidSyntaxException(MathErrorMessager.StartingWithOperation);

        // Not ending with operation
        if (!NotEndingWithExpression(expression))
            throw new InvalidSyntaxException(MathErrorMessager.EndingWithOperation);

        // Complex validation including different errors
        ComplexValidation(expression);

        // Return empty value if validation passed
        return true;
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

    private static void ComplexValidation(string expression)
    {
        var expressionSplit = RegexTemplates.SplitDelimiter.Split(expression)
            .Select(i => i.Replace(" ", ""))
            .Where(i => i != "")
            .ToArray();
        var isPreviousOperation = false;
        var isPreviousNumber = false;
        var isPreviousOpenParenthesis = false;
        var isPreviousCloseParenthesis = false;
        var previousOperation = "";

        foreach (var element in expressionSplit)
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
                        throw new InvalidSyntaxException(
                            MathErrorMessager.OperationBeforeParenthesisMessage(previousOperation));

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
                            throw new InvalidSyntaxException(
                                MathErrorMessager.InvalidOperatorAfterParenthesisMessage(element));

                        if (!isPreviousNumber && !isPreviousCloseParenthesis)
                        {
                            if (isPreviousOpenParenthesis && element == "-")
                                continue;
                            throw new InvalidSyntaxException(
                                MathErrorMessager.TwoOperationInRowMessage(previousOperation, element));
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
    }
}