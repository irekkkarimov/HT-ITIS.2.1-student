using Hw11.ExpressionVisitor;
using Hw11.Handlers;
using Hw11.Regex;

namespace Hw11.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<double> CalculateMathExpressionAsync(string? expression)
    {
        // Валидируем выражение и если не проходит валидацию, то возвращаем ошибку
        ExpressionValidator.Validate(expression ?? string.Empty);

        // Парсим выражение в польскую запись
        var expressionParser = new ExpressionParser();
        var expressionInPolishNotation = expressionParser.ParseToPolishNotation(
            RegexTemplates.SplitDelimiter.Split(expression!));

        // Конвертируем в единое выражение
        var expressionConverted = ExpressionTreeConverter.ConvertToExpressionTree(expressionInPolishNotation);

        // Искуственная задержка
        await Task.Delay(1000);

        // Вызываем VisitExpression у MyExpressionVisitor и ожидаем результат
        var result = await MyExpressionVisitor.VisitExpression(expressionConverted);

        // Возвращаем результат
        return result;
    }
}