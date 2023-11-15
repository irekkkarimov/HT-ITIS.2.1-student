using System.Linq.Expressions;
using Hw9.Dto;
using Hw9.ExpressionVisitor;
using Hw9.Handlers;
using Hw9.Regex;

namespace Hw9.Services.MathCalculator;

public class MathCalculatorService : IMathCalculatorService
{
    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        try
        {
            // Валидируем выражение и если не проходит валидацию, то возвращаем ошибку
            var expressionValidated = ExpressionValidator.Validate(expression);
            if (!expressionValidated.Item1)
                return new CalculationMathExpressionResultDto(expressionValidated.Item2);

            // Парсим выражение в польскую запись
            var expressionParser = new ExpressionParser();
            var expressionInPolishNotation = expressionParser.ParseToPolishNotation(
                RegexTemplates.SplitDelimiter.Split(expression));
            // return new CalculationMathExpressionResultDto(expressionInPolishNotation);
            
            // Конвертируем в единое выражение
            var expressionConverted = ExpressionTreeConverter.ConvertToExpressionTree(expressionInPolishNotation);
            
            // Компилируем выражение которое возвращает MyExpressionVisitor и вызываем
            var result = Expression.Lambda<Func<double>>(
                await MyExpressionVisitor.VisitExpression(expressionConverted)).Compile().Invoke();
            
            // Возвращаем результат
            return new CalculationMathExpressionResultDto(result);
        }
        catch (Exception e)
        {
            // Ловим ошибку и возвращаем (кастылиии, но нужно как-то ловить DivisionByZero)
            return new CalculationMathExpressionResultDto(e.Message);
        }
    }
}