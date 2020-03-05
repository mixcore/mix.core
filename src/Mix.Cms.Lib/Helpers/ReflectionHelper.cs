using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace Mix.Cms.Lib.Helpers
{
    public class ReflectionHelper
    {
        public static Expression<Func<T, bool>> CombineExpression<T>(
            Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2, MixEnums.ExpressionMethod method, string name = "model")
        {
            var parameter = Expression.Parameter(typeof(T), name);

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);
            switch (method)
            {
                case MixEnums.ExpressionMethod.Eq:
                    break;
                case MixEnums.ExpressionMethod.Lt:
                    break;
                case MixEnums.ExpressionMethod.Gt:
                    break;
                case MixEnums.ExpressionMethod.Lte:
                    break;
                case MixEnums.ExpressionMethod.Gte:
                    break;
                case MixEnums.ExpressionMethod.And:
                    return Expression.Lambda<Func<T, bool>>(Expression.Or(left, right), parameter);
                case MixEnums.ExpressionMethod.Or:
                    return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);
                default:
                    break;
            }
            return null;
        }

        public static Type GetPropertyType(Type type, string name)
        {
            Type fieldPropertyType;
            FieldInfo fieldInfo = type.GetField(name);

            if (fieldInfo == null)
            {
                PropertyInfo propertyInfo = type.GetProperty(name);

                //if (propertyInfo == null)
                //{
                //    throw new Exception();
                //}

                fieldPropertyType = propertyInfo?.PropertyType;
            }
            else
            {
                fieldPropertyType = fieldInfo.FieldType;
            }
            return fieldPropertyType;
        }
        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                    return _newValue;
                return base.Visit(node);
            }
        }
    }
    
}
