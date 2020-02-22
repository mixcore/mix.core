using Microsoft.OData;
using Microsoft.OData.UriParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq.Expressions;
using System.Reflection;

namespace Mix.Cms.Lib.Helpers
{
    public class ODataHelper<TModel>
    {
        private static Expression GetPropertyExpression(Type type, string name)
        {
            var param = Expression.Parameter(type, "model");
            PropertyInfo propertyInfo = type.GetProperty(name);
            Expression fieldPropertyExpression = Expression.Property(param, propertyInfo);
            return fieldPropertyExpression;
        }

        public static object GetPropValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        private static Type GetPropertyType(Type type, string name)
        {
            Type fieldPropertyType;
            FieldInfo fieldInfo = type.GetField(name);

            if (fieldInfo == null)
            {
                PropertyInfo propertyInfo = type.GetProperty(name);

                if (propertyInfo == null)
                {
                    throw new Exception();
                }

                fieldPropertyType = propertyInfo.PropertyType;
            }
            else
            {
                fieldPropertyType = fieldInfo.FieldType;
            }
            return fieldPropertyType;
        }

        public static void ParseFilter(SingleValueNode node, ref Expression<Func<TModel, bool>> result, int kind = -1)
        {
            // Parsing a filter, e.g. /Products?$filter=Name eq 'beer'
            if (node is BinaryOperatorNode)
            {
                var bon = node as BinaryOperatorNode;
                var left = bon.Left;
                var right = bon.Right;

                if (left is ConvertNode)
                {
                    ParseFilter(((ConvertNode)left).Source, ref result);
                }

                if (left is BinaryOperatorNode)
                {
                    ParseFilter(left, ref result);
                }

                if (right is ConvertNode)
                {
                    ParseFilter(((ConvertNode)right).Source, ref result, (int)bon.OperatorKind);
                }

                if (right is BinaryOperatorNode)
                {
                    ParseFilter(right, ref result, (int)bon.OperatorKind);
                }

                if (left is SingleValuePropertyAccessNode && right is ConstantNode)
                {
                    var property = left as SingleValuePropertyAccessNode;
                    var constant = right as ConstantNode;

                    if (property != null && property.Property != null && constant != null && constant.Value != null)
                    {
                        var exp = FilterObjectSet(property, constant, bon.OperatorKind);
                        var parameter = Expression.Parameter(typeof(TModel), "model");
                        if (kind >= 0 && result != null)
                        {
                            var binaryKind = (BinaryOperatorKind)kind;
                            result = CombineExpression(result, exp, binaryKind);
                        }
                        else
                        {
                            result = exp;
                        }
                    }
                }
            }
        }

        public static Expression<Func<TModel, bool>> FilterObjectSet(SingleValuePropertyAccessNode rule, ConstantNode constant, BinaryOperatorKind kind, string name = "model")
        {
            Type type = typeof(TModel);
            var par = Expression.Parameter(type, name);

            Type fieldPropertyType;
            Expression fieldPropertyExpression;

            FieldInfo fieldInfo = type.GetField(rule.Property.Name);

            if (fieldInfo == null)
            {
                PropertyInfo propertyInfo = type.GetProperty(rule.Property.Name);

                if (propertyInfo == null)
                {
                    throw new Exception();
                }

                fieldPropertyType = propertyInfo.PropertyType;
                fieldPropertyExpression = Expression.Property(par, propertyInfo);
            }
            else
            {
                fieldPropertyType = fieldInfo.FieldType;
                fieldPropertyExpression = Expression.Field(par, fieldInfo);
            }
            object data2 = null;
            if (fieldPropertyType.IsGenericType && fieldPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                System.ComponentModel.TypeConverter conv = System.ComponentModel.TypeDescriptor.GetConverter(fieldPropertyType);
                data2 = conv.ConvertFrom(constant.LiteralText);
                //data2 = Convert.ChangeType(constant.LiteralText, Nullable.GetUnderlyingType());
            }
            else
            {
                data2 = Convert.ChangeType(constant.LiteralText, fieldPropertyType);
            }

            if (fieldPropertyType == typeof(string))
            {
                data2 = data2.ToString().Replace("'", "");
            }
            BinaryExpression eq = null;
            switch (kind)
            {
                case BinaryOperatorKind.Or:
                    eq = Expression.Or(fieldPropertyExpression,
                                     Expression.Constant(data2, fieldPropertyType));
                    break;

                case BinaryOperatorKind.And:
                    eq = Expression.And(fieldPropertyExpression,
                                      Expression.Constant(data2, fieldPropertyType));
                    break;

                case BinaryOperatorKind.Equal:
                    eq = Expression.Equal(fieldPropertyExpression,
                                      Expression.Constant(data2, fieldPropertyType));
                    break;

                case BinaryOperatorKind.NotEqual:
                    eq = Expression.NotEqual(fieldPropertyExpression,
                                      Expression.Constant(data2, fieldPropertyType));
                    break;

                case BinaryOperatorKind.GreaterThan:
                    eq = Expression.GreaterThan(fieldPropertyExpression,
                                      Expression.Constant(data2));
                    break;

                case BinaryOperatorKind.GreaterThanOrEqual:
                    eq = Expression.GreaterThanOrEqual(fieldPropertyExpression,
                                      Expression.Constant(data2, fieldPropertyType));
                    break;

                case BinaryOperatorKind.LessThan:
                    eq = Expression.LessThan(fieldPropertyExpression,
                                      Expression.Constant(data2, fieldPropertyType));
                    break;

                case BinaryOperatorKind.LessThanOrEqual:
                    eq = Expression.LessThanOrEqual(fieldPropertyExpression,
                                      Expression.Constant(data2, fieldPropertyType));
                    break;

                case BinaryOperatorKind.Add:
                    eq = Expression.Add(fieldPropertyExpression,
                                      Expression.Constant(data2, fieldPropertyType));
                    break;

                case BinaryOperatorKind.Subtract:
                    eq = Expression.Subtract(fieldPropertyExpression,
                                      Expression.Constant(data2, fieldPropertyType));
                    break;

                case BinaryOperatorKind.Multiply:
                    eq = Expression.Multiply(fieldPropertyExpression,
                                      Expression.Constant(data2, fieldPropertyType));
                    break;

                case BinaryOperatorKind.Divide:
                    eq = Expression.Divide(fieldPropertyExpression,
                                      Expression.Constant(data2, fieldPropertyType));
                    break;

                case BinaryOperatorKind.Modulo:
                    eq = Expression.Modulo(fieldPropertyExpression,
                                      Expression.Constant(data2, fieldPropertyType));
                    break;

                case BinaryOperatorKind.Has:
                    break;
            };
            return Expression.Lambda<Func<TModel, bool>>(eq, par);
        }

        public static Expression<Func<T, bool>> CombineExpression<T>(Expression<Func<T, bool>> expr1, Expression<Func<T, bool>> expr2, BinaryOperatorKind kind, string name = "model")
        {
            var parameter = Expression.Parameter(typeof(T), name);

            var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
            var left = leftVisitor.Visit(expr1.Body);

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);

            switch (kind)
            {
                case BinaryOperatorKind.Or:
                    return Expression.Lambda<Func<T, bool>>(Expression.Or(left, right), parameter);

                case BinaryOperatorKind.And:
                    return Expression.Lambda<Func<T, bool>>(Expression.AndAlso(left, right), parameter);

                case BinaryOperatorKind.Equal:
                    break;

                case BinaryOperatorKind.NotEqual:
                    break;

                case BinaryOperatorKind.GreaterThan:
                    break;

                case BinaryOperatorKind.GreaterThanOrEqual:
                    break;

                case BinaryOperatorKind.LessThan:
                    break;

                case BinaryOperatorKind.LessThanOrEqual:
                    break;

                case BinaryOperatorKind.Add:
                    break;

                case BinaryOperatorKind.Subtract:
                    break;

                case BinaryOperatorKind.Multiply:
                    break;

                case BinaryOperatorKind.Divide:
                    break;

                case BinaryOperatorKind.Modulo:
                    break;

                case BinaryOperatorKind.Has:
                    break;

                default:
                    break;
            }
            return null;
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

        public static void TryNodeValue(SingleValueNode node, IDictionary<string, object> values)
        {
            if (node is BinaryOperatorNode)
            {
                var bon = (BinaryOperatorNode)node;
                var left = bon.Left;
                var right = bon.Right;

                if (left is ConvertNode)
                {
                    TryNodeValue(((ConvertNode)left).Source, values);
                }

                if (left is BinaryOperatorNode)
                {
                    TryNodeValue(left, values);
                }

                if (right is ConvertNode)
                {
                    TryNodeValue(((ConvertNode)right).Source, values);
                }

                if (right is BinaryOperatorNode)
                {
                    TryNodeValue(right, values);
                }

                if (left is SingleValuePropertyAccessNode && right is ConstantNode)
                {
                    var p = (SingleValuePropertyAccessNode)left;

                    if (bon.OperatorKind == BinaryOperatorKind.Equal)
                    {
                        var value = ((ConstantNode)right).Value;
                        values.Add(p.Property.Name, value);
                    }
                }
            }
        }
    }

    public class InMemoryMessage : IODataRequestMessage, IODataResponseMessage, IContainerProvider, IDisposable
    {
        private readonly Dictionary<string, string> headers;

        public InMemoryMessage()
        {
            headers = new Dictionary<string, string>();
        }

        public IEnumerable<KeyValuePair<string, string>> Headers {
            get { return this.headers; }
        }

        public int StatusCode { get; set; }

        public Uri Url { get; set; }

        public string Method { get; set; }

        public Stream Stream { get; set; }

        public IServiceProvider Container { get; set; }

        public string GetHeader(string headerName)
        {
            string headerValue;
            return this.headers.TryGetValue(headerName, out headerValue) ? headerValue : null;
        }

        public void SetHeader(string headerName, string headerValue)
        {
            headers[headerName] = headerValue;
        }

        public Stream GetStream()
        {
            return this.Stream;
        }

        public Action DisposeAction { get; set; }

        void IDisposable.Dispose()
        {
            if (this.DisposeAction != null)
            {
                this.DisposeAction();
            }
        }

        Stream IODataResponseMessage.GetStream()
        {
            return this.Stream;
        }
    }
}