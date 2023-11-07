using System.Globalization;
using Hw8.Utils;

namespace Hw8.Calculator;

public interface ICalculatorCaller
{
    public Result<double> Calculate(string val1, string operation, string val2);

    public (bool, Operation) ValidateOperation(string operation);

    public double CalculateSwitchOperation(double arg1, Operation operation, double arg2);
}