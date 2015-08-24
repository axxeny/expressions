using System.Linq.Expressions;

namespace ExpressionSandbox
{
    internal class BinaryOperatorModifier : ExpressionVisitor
    {
        public ExpressionType OriginalOperator { get; private set; }
        public ExpressionType Replacement { get; private set; }
        public bool Recursive { get; private set; }

        public BinaryOperatorModifier(ExpressionType originalOperator, ExpressionType replacement, bool recursive)
        {
            OriginalOperator = originalOperator;
            Replacement = replacement;
            Recursive = recursive;
        }

        public Expression Modify(Expression expression)
        {
            return Visit(expression);
        }

        private Expression VisitIfRecursive(Expression expression)
        {
            return Recursive
                ? Visit(expression)
                : expression;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType != OriginalOperator)
            {
                return base.VisitBinary(node);
            }

            var left = VisitIfRecursive(node.Left);
            var right = VisitIfRecursive(node.Right);

            return Expression.MakeBinary(Replacement, left, right);
        }
    }
}