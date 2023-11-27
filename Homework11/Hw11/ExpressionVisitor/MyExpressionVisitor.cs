using System.Diagnostics.CodeAnalysis;
using Hw11.ErrorMessages;

namespace Hw11.ExpressionVisitor;

using System.Linq.Expressions;

[ExcludeFromCodeCoverage]
public static class MyExpressionVisitor
{
    public static async Task<double> VisitExpression(Expression expression)
    {
        double result = await VisitNode((dynamic)expression);
        return result;
    }

    private static Task<double> VisitNode(ConstantExpression node)
    {
        var value = (double)node.Value!;
        return Task.Run(() => value);
    }

    private static async Task<double> VisitNode(UnaryExpression node)
    {
        var result = Expression.Lambda<Func<double>>(node).Compile().Invoke();

        return await Task.Run(() => result);
    }

    private static async Task<double> VisitNode(BinaryExpression node)
    {
        var values = await CompileBinaryAsync(node.Left, node.Right);
        var expression = GetExpressionByMathOperationType(node.NodeType, values);
        var result = Expression.Lambda<Func<double>>(expression).Compile().Invoke();

        return result;
    }

    private static async Task<double[]> CompileBinaryAsync(Expression left, Expression right)
    {
        var first = VisitExpression(left);
        var second = VisitExpression(right);

        return await Task.WhenAll(first, second);
    }

    private static Expression GetExpressionByMathOperationType(ExpressionType expressionType, double[] expressionValues)
    {
        return expressionType switch
        {
            ExpressionType.Negate =>
                Expression.Negate(Expression.Constant(expressionValues[0])),
            ExpressionType.Add =>
                Expression.Add(Expression.Constant(expressionValues[0]), Expression.Constant(expressionValues[1])),
            ExpressionType.Subtract =>
                Expression.Subtract(Expression.Constant(expressionValues[0]), Expression.Constant(expressionValues[1])),
            ExpressionType.Multiply =>
                Expression.Multiply(Expression.Constant(expressionValues[0]), Expression.Constant(expressionValues[1])),
            _ => expressionValues[1] == 0
                ? throw new ArgumentException(MathErrorMessager.DivisionByZero)
                : Expression.Divide(Expression.Constant(expressionValues[0]), Expression.Constant(expressionValues[1]))
        };
    }
}