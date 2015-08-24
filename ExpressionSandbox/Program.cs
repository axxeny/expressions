using System;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionSandbox
{
    class Program
    {
        static void Main(string[] args)
        {
            Contract.Assert(args.Length == 0);
            
            var a = Expression.Parameter(typeof (double), "a");
            var b = Expression.Parameter(typeof (double), "b");
            var c = Expression.Parameter(typeof (double), "c");
            var two = Expression.Constant(2.0);
            var four = Expression.Constant(4.0);
            var minusB = Expression.Negate(b);
            var discr =
                Expression.Subtract(
                    Expression.Call(typeof(Math).GetMethod("Pow"), b, two),
                    Expression.Multiply(four, Expression.Multiply(a, c)));
            var sqrtOfDiscr = Expression.Call(typeof (Math).GetMethod("Sqrt"), discr);
            
            var rootNegative =
                Expression.Divide(
                    Expression.Subtract(minusB, sqrtOfDiscr),
                    Expression.Multiply(two, a)
                    );
            var rootPositive =
                new BinaryOperatorModifier(ExpressionType.Subtract, ExpressionType.Add, recursive: false)
                .Modify(rootNegative);

            Console.WriteLine(Expression.Lambda(rootPositive, a, b, c).ToString());
            Console.WriteLine(Expression.Lambda(rootNegative, a, b, c).ToString());

            var d = Expression.Parameter(typeof (double), "D");
            var binaryToConst =
                new ExpressionModifier(
                    e => e.NodeType == ExpressionType.Subtract 
                        && ((BinaryExpression) e).Left is MethodCallExpression
                        && ((BinaryExpression)e).Right is BinaryExpression && ((BinaryExpression)e).Right.NodeType == ExpressionType.Multiply,
                    d,
                    recursive: false);
            var rootsWithHiddenDiscr = new[] {rootPositive, rootNegative}
                .Select(e => binaryToConst.Modify(e));

            foreach (var root in rootsWithHiddenDiscr)
            {
                Console.WriteLine(Expression.Lambda(root, a, b, d).ToString());
            }
        }
    }
}
