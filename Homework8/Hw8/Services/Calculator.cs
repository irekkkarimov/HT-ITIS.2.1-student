using Hw8.Calculator;

namespace Hw8.Services;

public class Calculator : ICalculator
{
    public double Plus(double val1, double val2)
    {
        return val1 + val2;
    }

    public double Minus(double val1, double val2)
    {
        return val1 - val2;
    }

    public double Multiply(double val1, double val2)
    {
        return val1 * val2;
    }

    public double Divide(double firstValue, double secondValue)
    {
        if (secondValue == 0)
            throw new InvalidOperationException("Can't divide by zero!");
        return firstValue / secondValue;
    }
}