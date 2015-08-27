using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Linq.Expressions;

namespace ExpressionSandbox
{
    class Program
    {
        private static void Main(string[] args)
        {
            Contract.Assert(args.Length == 0);

            var roots = QuadraticEquationExpressionBuilder.GetQuadraticEquationRootsAtRuntime();

            ExpressionConsoleIo.WriteExpressions(roots, "a", "b", "D");

            var discrBody = QuadraticEquationExpressionBuilder.GetDiscriminant();

            var binaryToConst =
                new ExpressionModifier(
                    e => e.NodeType == ExpressionType.Parameter && ((ParameterExpression) e).Name == "D",
                    discrBody,
                    recursive: false);

            var rootsWithDiscr = roots
                .Select(e => binaryToConst.Modify(e)).ToArray();
            ExpressionConsoleIo.WriteExpressions(rootsWithDiscr, "a", "b", "c");

            var calculationResults = ExpressionUser.CompileAndRun(rootsWithDiscr.Select(e => (Expression<Func<double, double, double, double>>)e), 1, 7, -3);
            foreach (var result in calculationResults)
            {
                Console.WriteLine(result);
            }
        }
    }
}
