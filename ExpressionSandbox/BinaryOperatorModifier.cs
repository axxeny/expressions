using System.Linq.Expressions;

namespace ExpressionSandbox
{
    internal class BinaryOperatorModifier : ExpressionVisitor
    {
        // Свойства и конструктор
        public ExpressionType NodeToReplace { get; private set; }
        public ExpressionType Replacement { get; private set; }
        public bool Recursive { get; private set; }

        public BinaryOperatorModifier(ExpressionType nodeToReplace, ExpressionType replacement, bool recursive)
        {
            NodeToReplace = nodeToReplace;
            Replacement = replacement;
            Recursive = recursive;
        }

        public Expression Modify(Expression expression)
        {
            return Visit(expression);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.NodeType == NodeToReplace)
            {
                var left = Recursive
                    ? Visit(node.Left)
                    : node.Left;
                var right = Recursive
                    ? Visit(node.Right)
                    : node.Right;

                return Expression.MakeBinary(Replacement, left, right);
            }

            return base.VisitBinary(node);
        }
    }
}