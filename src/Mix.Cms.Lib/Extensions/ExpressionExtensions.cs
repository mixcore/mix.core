using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Mix.Cms.Lib.Extensions
{
    public static class ExpressionExtensions
    {
        public static Expression<T> Compose<T>(this Expression<T> firstExpr, Expression<T> secondExpr, Func<Expression, Expression, Expression> merge)
        {
            // replace parameters in the second lambda expression with parameters from the first
            var secondExprBody = ParameterRebinder.ReplaceParameters(secondExpr, firstExpr);

            // apply composition of lambda expression bodies to parameters from the first expression
            return Expression.Lambda<T>(merge(firstExpr.Body, secondExprBody), firstExpr.Parameters);
        }

        public static Expression<Func<T, bool>> AndAlso<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> AndAlsoNot<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second.Not(), Expression.AndAlso);
        }

        public static Expression<Func<T, bool>> AndAlsoIf<T>(this Expression<Func<T, bool>> first, bool condition, Expression<Func<T, bool>> second)
        {
            return condition
                ? first.Compose(second, Expression.AndAlso)
                : first;
        }

        public static Expression<Func<T, bool>> OrIf<T>(this Expression<Func<T, bool>> first, bool condition, Expression<Func<T, bool>> second)
        {
            return condition
                ? first.Compose(second, Expression.OrElse)
                : first;
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.OrElse);
        }

        public static Expression<Func<T, bool>> OrNot<T>(this Expression<Func<T, bool>> first, Expression<Func<T, bool>> second)
        {
            return first.Compose(second.Not(), Expression.OrElse);
        }

        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> one)
        {
            var candidateExpr = one.Parameters[0];
            var body = Expression.Not(one.Body);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }
    }

    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> _targetToSourceParamsMap;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> targetToSourceParamsMap)
        {
            _targetToSourceParamsMap = targetToSourceParamsMap ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters<T>(Expression<T> targetExpr, Expression<T> sourceExpr)
        {
            var targetToSourceParamsMap = sourceExpr.Parameters
                .Select((sourceParam, firstParamIndex) => new
                {
                    sourceParam,
                    targetParam = targetExpr.Parameters[firstParamIndex]
                })
                .ToDictionary(p => p.targetParam, p => p.sourceParam);

            return new ParameterRebinder(targetToSourceParamsMap).Visit(targetExpr.Body);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (_targetToSourceParamsMap.TryGetValue(node, out var replacement))
            {
                node = replacement;
            }

            return base.VisitParameter(node);
        }
    }
}
