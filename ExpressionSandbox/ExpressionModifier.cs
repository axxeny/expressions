using System;
using System.Linq.Expressions;

namespace ExpressionSandbox
{
    internal class ExpressionModifier : ExpressionVisitor
    {
        public Func<Expression, bool> IsNodeToReplace { get; private set; }
        public Expression Replacement { get; private set; }
        public bool Recursive { get; private set; }

        public ExpressionModifier(Func<Expression, bool> isNodeToReplace, Expression replacement, bool recursive)
        {
            IsNodeToReplace = isNodeToReplace;
            Replacement = replacement;
            Recursive = recursive;
        }

        public Expression Modify(Expression expression)
        {
            return Visit(expression);
        }

        public override Expression Visit(Expression node)
        {
            if (node == null) return null;
            if (!IsNodeToReplace(node)) return base.Visit(node);
            return Recursive 
                ? Visit(Replacement)
                : base.Visit(node);
        }
    }
}