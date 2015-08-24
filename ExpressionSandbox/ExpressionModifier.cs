using System;
using System.Linq.Expressions;

namespace ExpressionSandbox
{
    internal class ExpressionModifier : ExpressionVisitor
    {
        public Func<Expression, bool> IsOriginal { get; private set; }
        public Expression Replacement { get; private set; }
        public bool Recursive { get; private set; }

        public ExpressionModifier(Func<Expression, bool> isOriginal, Expression replacement, bool recursive)
        {
            IsOriginal = isOriginal;
            Replacement = replacement;
            Recursive = recursive;
        }

        public Expression Modify(Expression expression)
        {
            return Visit(expression);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            return !IsOriginal(node)
                ? base.VisitBinary(node)
                : Replacement;
        }
    }
}