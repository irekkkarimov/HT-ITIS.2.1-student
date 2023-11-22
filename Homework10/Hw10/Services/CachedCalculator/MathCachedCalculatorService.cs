using Hw10.DbModels;
using Hw10.Dto;
using Hw10.Services.MathCalculator;

namespace Hw10.Services.CachedCalculator;

public class MathCachedCalculatorService : IMathCalculatorService
{
	private readonly ApplicationContext _dbContext;
	private readonly IMathCalculatorService _simpleCalculator;

	public MathCachedCalculatorService(ApplicationContext dbContext, IMathCalculatorService simpleCalculator)
	{
		_dbContext = dbContext;
		_simpleCalculator = simpleCalculator;
	}

	public async Task<CalculationMathExpressionResultDto> CalculateMathExpressionAsync(string? expression)
	{
		// // For cache reading purposes we need to make sure that expression is not null
		if (!string.IsNullOrEmpty(expression))
		{
			// Reading database cache
			var solvingExpressions = _dbContext.SolvingExpressions;
			var solvingExpressionsWhere = solvingExpressions.Where(i => i.Expression == expression.Trim());
			if (solvingExpressionsWhere.Any())
			{
				// Getting cached expression result
				var expressionResult = solvingExpressionsWhere.First().Result;
			
				// Fake delay
				await Task.Delay(500);
			
				// Returning expression result
				return new CalculationMathExpressionResultDto(expressionResult);
			}
		}
		
		
		// Calculating expression using MathCalculatorService
		var result = await _simpleCalculator.CalculateMathExpressionAsync(expression);

		// If computation succeeded
		if (result.IsSuccess)
		{
			// Declaring new SolvingExpression instance
			var solvingExpression = new SolvingExpression
			{
				SolvingExpressionId = 0,
				Expression = expression.Trim(),
				Result = result.Result
			};

			// Writing to database
			_dbContext.SolvingExpressions.Add(solvingExpression);
			await _dbContext.SaveChangesAsync();
            
		}

		// Returning CalculatorMathExpressionResultDto that was received from Calculating method
        return result;
    }
}