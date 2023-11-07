using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Hw8.Calculator;
using Hw8.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Controllers;

public class CalculatorController : Controller
{
    public ActionResult<double> Calculate([FromServices] ICalculatorCaller calculatorCaller,
        string val1,
        string operation,
        string val2)
    {
        var result = calculatorCaller.Calculate(val1, operation, val2);
        if (result.Status == ResultStatus.Success)
            return Ok(result.Value);
        return BadRequest(result.Message);
    }
    
    [ExcludeFromCodeCoverage]
    public IActionResult Index()
    {
        return Content(
            "Заполните val1, operation(plus, minus, multiply, divide) и val2 здесь '/calculator/calculate?val1= &operation= &val2= '\n" +
            "и добавьте её в адресную строку.");
    }
}