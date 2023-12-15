using System.Diagnostics.CodeAnalysis;
using MemoryCachedCalculator.Dto;
using MemoryCachedCalculator.Services;
using MemoryCachedCalculator.Services.MathCalculator;
using Microsoft.AspNetCore.Mvc;

namespace MemoryCachedCalculator.Controllers;

[Route("[controller]")]
public class CalculatorController : ControllerBase
{
    private readonly IMathCalculatorService _mathCalculatorService;

    public CalculatorController(IMathCalculatorService mathCalculatorService)
    {
        _mathCalculatorService = mathCalculatorService;
    }
    
    [HttpPost]
    public async Task<ActionResult> CalculateMathExpression(string expression)
    {
        var result = await _mathCalculatorService.CalculateMathExpressionAsync(expression);
        return Ok(new { result.IsSuccess, result.ErrorMessage, result.Result });
    }
}