using JetBrains.Annotations;

namespace System.Linq.Expressions
{
    public static class ExpressionExtensions
    {
        public static string GetMemberName<TEntity, TMember>([NotNull] this Expression<Func<TEntity, TMember>> memberExpression)
            => GetMemberInfo(memberExpression)?.Name!;

        public static MemberInfo GetMemberInfo<TEntity, TMember>([NotNull] this Expression<Func<TEntity, TMember>> expression)
        {
            if (expression.NodeType != ExpressionType.Lambda)
                throw new ArgumentException(null, nameof(expression));

            var lambda = (LambdaExpression)expression;

            var memberExpression = ExtractMemberExpression(lambda.Body);
            if (memberExpression == null)
                throw new ArgumentException(null, nameof(expression));

            return memberExpression.Member;
        }

        private static MemberExpression? ExtractMemberExpression(Expression expression)
        {
            if (expression.NodeType == ExpressionType.MemberAccess)
            {
                return (MemberExpression)expression;
            }

            if (expression.NodeType == ExpressionType.Convert)
            {
                var operand = ((UnaryExpression)expression).Operand;
                return ExtractMemberExpression(operand);
            }

            return null;
        }
    }
}