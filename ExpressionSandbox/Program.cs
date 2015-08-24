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

            var roots = QuadraticEquationExpressionBuilder.GetQuadraticEquationRoots();

            ExpressionConsoleIo.WriteExpressions(roots, "a", "b", "D");

            var discrBody = QuadraticEquationExpressionBuilder.GetDiscriminant();

            var binaryToConst =
                new ExpressionModifier(
                    e => e.NodeType == ExpressionType.Parameter && ((ParameterExpression) e).Name == "D",
                    discrBody,
                    recursive: false);
            
            var rootsWithHiddenDiscr = roots
                .Select(e => binaryToConst.Modify(e));

            ExpressionConsoleIo.WriteExpressions(rootsWithHiddenDiscr, "a", "b", "c");
        }
    }
}
