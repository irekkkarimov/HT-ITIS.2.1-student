using System.Diagnostics.CodeAnalysis;
using MemoryCachedCalculator.Dto;
using MemoryCachedCalculator.Services.MyMemoryCache;

namespace MemoryCachedCalculator.Services.CachedCalculator;

[ExcludeFromCodeCoverage]
public class MathCachedCalculatorService : IMathCalculatorService
{
    private readonly IMathCalculatorService _simpleCalculator;
    private readonly IMyMemoryCache _cache;

    public MathCachedCalculatorService(IMyMemoryCache cache, IMathCalculatorService simpleCalculator)
    {
        _cache = cache;
        _simpleCalculator = simpleCalculator;
    }

    public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
    {
        // // For cache reading purposes we need to make sure that expression is not null
        if (!string.IsNullOrEmpty(expression))
        {
            // Reading cache
            var expressionTuple = _cache.Find(expression.Trim());
            if (expressionTuple.Item1)
            {
                // Getting cached expression result
                var expressionResult = expressionTuple.Item2;
                Console.WriteLine($"Result from cache: {expressionResult}");

                // Returning expression result
                return new CalculationMathExpressionResultDto(expressionResult);
            }
        }


        // Calculating expression using MathCalculatorService
        var result = await _simpleCalculator.CalculateMathExpressionAsync(expression);

        // If computation succeeded
        if (result.IsSuccess)
        {
            // Writing to cache
            _cache.Insert(expression!.Trim(), result.Result);
        }

        // Returning CalculatorMathExpressionResultDto that was received from Calculating method
        return result;
    }
}