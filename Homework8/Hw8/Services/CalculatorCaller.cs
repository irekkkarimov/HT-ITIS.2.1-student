using System.Globalization;
using Hw8.Calculator;
using Hw8.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Hw8.Services;

public class CalculatorCaller : ICalculatorCaller
{
    private readonly ICalculator _calculator;
    
    public CalculatorCaller([FromServices] ICalculator calculator)
    {
        _calculator = calculator;
    }
    
    public Result<double> Calculate(string val1, string operation, string val2)
    {
        var operationTuple = ValidateOperation(operation);
        var (ifArg1Parsed, ifArg2Parsed) =
            (double.TryParse(val1, out var arg1Parsed), double.TryParse(val2, out var arg2Parsed));

        if (!operationTuple.Item1)
            return Result<double>.Failure(Messages.InvalidOperationMessage);
        if (!ifArg1Parsed || !ifArg2Parsed)
            return Result<double>.Failure(Messages.InvalidNumberMessage);
        return arg2Parsed == 0 
            ? Result<double>.Failure(Messages.DivisionByZeroMessage)
            : Result<double>.Success(CalculateSwitchOperation(arg1Parsed, operationTuple.Item2, arg2Parsed));
    }

    public (bool, Operation) ValidateOperation(string operation) =>
        operation switch
        {
            "Plus" => (true, Operation.Plus),
            "Minus" => (true, Operation.Minus),
            "Multiply" => (true, Operation.Multiply),
            "Divide" => (true, Operation.Divide),
            _ => (false, Operation.Invalid)
        };

    public double CalculateSwitchOperation(double arg1, Operation operation, double arg2) =>
        operation switch
        {
            Operation.Plus => _calculator.Plus(arg1, arg2),
            Operation.Minus => _calculator.Minus(arg1, arg2),
            Operation.Multiply => _calculator.Multiply(arg1, arg2),
            Operation.Divide => _calculator.Divide(arg1, arg2),
            _ => throw new InvalidOperationException(Messages.InvalidOperationMessage)
        };
}