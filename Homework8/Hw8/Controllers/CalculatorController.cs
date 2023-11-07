using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult<double> Calculate([FromServices] ICalculator calculator,
        string val1,
        string operation,
        string val2)
    {
        var operationTuple = ValidateOperation(operation);
        var (ifArg1Parsed, ifArg2Parsed) =
            (double.TryParse(val1, out var arg1Parsed), double.TryParse(val2, out var arg2Parsed));

        if (!operationTuple.Item1)
            return BadRequest(Messages.InvalidOperationMessage);
        if (!ifArg1Parsed || !ifArg2Parsed)
            return BadRequest(Messages.InvalidNumberMessage);
        return arg2Parsed == 0 
            ? BadRequest(Messages.DivisionByZeroMessage)
            : Ok(CalculateSwitchOperation(calculator, arg1Parsed, operationTuple.Item2, arg2Parsed).ToString(CultureInfo.CurrentCulture));
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
    
    [NonAction]
    private (bool, Operation) ValidateOperation(string operation) =>
        operation switch
        {
            "Plus" => (true, Operation.Plus),
            "Minus" => (true, Operation.Minus),
            "Multiply" => (true, Operation.Multiply),
            "Divide" => (true, Operation.Divide),
            _ => (false, Operation.Invalid)
        };
    
    [NonAction]
    private double CalculateSwitchOperation(ICalculator calculator, double arg1, Operation operation, double arg2) =>
        operation switch
        {
            Operation.Plus => calculator.Plus(arg1, arg2),
            Operation.Minus => calculator.Minus(arg1, arg2),
            Operation.Multiply => calculator.Multiply(arg1, arg2),
            Operation.Divide => calculator.Divide(arg1, arg2)
        };
}