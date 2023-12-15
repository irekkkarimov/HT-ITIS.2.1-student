using System.Diagnostics.CodeAnalysis;
using System.Text;
using MemoryCachedCalculator.Regex;

namespace MemoryCachedCalculator.Handlers;

[ExcludeFromCodeCoverage]
public class ExpressionParser
{
    private readonly Dictionary<string, int> _operationPriority = new()
    {
        { "(", 0 },
        { "+", 1 },
        { "-", 1 },
        { "*", 2 },
        { "/", 2 },
        { "~", 4 }
    };

    public string ParseToPolishNotation(string[] expressionInput)
    {
        var expression = expressionInput
            .Select(i => i.Replace(" ", ""))
            .Where(i => i != "")
            .ToArray();
        var postfix = new StringBuilder();
        var operations = new Stack<string>();
        var isPreviousOpenParenthesis = false;

        for (var i = 0; i < expression.Length; i++)
        {
            var element = expression[i];

            // Если элемент является числом то добавляем в постфиксную запись и идём в следующую итерацию
            if (RegexTemplates.NumberPattern.IsMatch(element))
            {
                postfix.Append(element);
                postfix.Append(' ');
                isPreviousOpenParenthesis = false;
                continue;
            }

            switch (element)
            {
                case "(":
                {
                    operations.Push(element);
                    isPreviousOpenParenthesis = true;
                    continue;
                }
                case ")":
                {
                    // Добавляем в постфикскную запись все операторы до открывающей скобки
                    while (operations.Peek() != "(")
                    {
                        postfix.Append(operations.Pop());
                        postfix.Append(' ');
                    }

                    // Убираем открывающую скобку
                    operations.Pop();
                    isPreviousOpenParenthesis = false;
                    continue;
                }
            }

            // Унарный "-"
            if (element == "-" && isPreviousOpenParenthesis)
                element = "~";
            isPreviousOpenParenthesis = false;

            // Добавляем в постфикс все операторы из стека с более высоким приоритетом чем текущий
            while (operations.Any() && _operationPriority[operations.Peek()] >= _operationPriority[element])
            {
                postfix.Append(operations.Pop());
                postfix.Append(' ');
            }

            // Добавляем в стек текущий оператор
            operations.Push(element);
        }

        // Вписываем оставшиеся операторы в постфикс
        foreach (var operation in operations)
        {
            postfix.Append(operation);
            postfix.Append(' ');
        }

        return postfix.ToString();
    }
}