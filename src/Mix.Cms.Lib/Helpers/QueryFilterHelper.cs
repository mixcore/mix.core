using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models;
using Mix.Cms.Lib.Models.Cms;
using Newtonsoft.Json.Linq;

namespace Mix.Cms.Lib.Helpers
{
    public class QueryFilterHelper
    {
        public void TestCreateModel()
        {
            ExpressionModel.Create(MixLogicalOperatorKind.And)
                .AddExpression
                (
                    ExpressionModel.Create(MixLogicalOperatorKind.And)
                        .AddFunction(FunctionModel.Create(MixCompareOperatorKind.Eq, "ho_va_ten", "Tran Nhat Duy"))
                        .AddFunction(FunctionModel.Create(MixCompareOperatorKind.Eq, "status", "open"))
                ).AddExpression
                (
                    ExpressionModel.Create(MixLogicalOperatorKind.And)
                    .AddExpression
                    (
                        ExpressionModel.Create(MixLogicalOperatorKind.Or)
                            .AddFunction(FunctionModel.Create(MixCompareOperatorKind.Eq, "date", "2020/12/10", "2020/12/30"))
                            .AddFunction(FunctionModel.Create(MixCompareOperatorKind.Eq, "address", "Nguyen thi Minh Khai"))
                    ).AddExpression
                    (
                        ExpressionModel.Create(MixLogicalOperatorKind.And)
                            .AddFunction(FunctionModel.Create(MixCompareOperatorKind.Gt, "age", "18"))
                            .AddFunction(FunctionModel.Create(MixCompareOperatorKind.Eq, "gender", "male"))
                    )
                );
        }



        /// <summary>
        /// Combine Two Expressions by Type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private static Expression<Func<MixAttributeSetValue, bool>> CombineExpressionByType(MixLogicalOperatorKind type,
            Expression<Func<MixAttributeSetValue, bool>> expr1,
            Expression<Func<MixAttributeSetValue, bool>> expr2, ParameterExpression parameter)
        {
            Expression left = null;
            if (expr1 != null)
            {
                var leftVisitor = new ReplaceExpressionVisitor(expr1.Parameters[0], parameter);
                left = leftVisitor.Visit(expr1.Body);
            }

            var rightVisitor = new ReplaceExpressionVisitor(expr2.Parameters[0], parameter);
            var right = rightVisitor.Visit(expr2.Body);
            return type switch
            {
                MixLogicalOperatorKind.And => expr1 != null ? Expression.Lambda<Func<MixAttributeSetValue, bool>>(Expression.AndAlso(left, right), parameter) :
                                        Expression.Lambda<Func<MixAttributeSetValue, bool>>(right, parameter),
                MixLogicalOperatorKind.Or => expr1 != null ? Expression.Lambda<Func<MixAttributeSetValue, bool>>(Expression.OrElse(left, right), parameter) :
                                        Expression.Lambda<Func<MixAttributeSetValue, bool>>(right, parameter),
                _ => null,
            };
        }

        public static Expression<Func<MixAttributeSetValue, bool>> CreateExpression(
          JObject jsonQuery)
        {
            Expression<Func<MixAttributeSetValue, bool>> root = null;
            try
            {
                ExpressionModel expressionModel = jsonQuery.ToObject<ExpressionModel>();
                root = GetExpression(expressionModel);
                return root;

            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return null;
        }

        private static Expression<Func<MixAttributeSetValue, bool>> GetExpression(
         ExpressionModel expressionModel)
        {
            MixLogicalOperatorKind expressionType = expressionModel.ExpressionType;

            var parameter = Expression.Parameter(typeof(MixAttributeSetValue), "m");
            Expression<Func<MixAttributeSetValue, bool>> root = null;
            try
            {
                expressionModel?.Functions?.ForEach(function =>
                {
                    root = CombineExpressionByType(expressionType,
                        root, GetFunction(function), parameter);
                });


                expressionModel?.Expressions?.ForEach(expression =>
                {
                    root = CombineExpressionByType(expressionType,
                            root, GetExpression(expression), parameter);
                });

                return root;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return null;
        }

        private static Expression<Func<MixAttributeSetValue, bool>> GetFunction(FunctionModel functionModel)
        {
            return functionModel.Rule switch
            {
                MixCompareOperatorKind.Eq => m => m.AttributeFieldName == functionModel.FieldName
                && m.StringValue == functionModel.Value,

                MixCompareOperatorKind.Neq => m => m.AttributeFieldName == functionModel.FieldName
                && m.StringValue != functionModel.Value,

                MixCompareOperatorKind.Ct => m => m.AttributeFieldName == functionModel.FieldName
                && (EF.Functions.Like(m.StringValue, buildLikeString(functionModel.Value))),

                MixCompareOperatorKind.Nct => m => m.AttributeFieldName == functionModel.FieldName
                && !(EF.Functions.Like(m.StringValue, buildLikeString(functionModel.Value))),

                MixCompareOperatorKind.Ra => m => m.AttributeFieldName == functionModel.FieldName
                && (string.Compare(m.StringValue, functionModel.MinValue) > 0
                && string.Compare(m.StringValue, functionModel.MaxValue) < 0),

                MixCompareOperatorKind.Nra => m => m.AttributeFieldName == functionModel.FieldName
                && (string.Compare(m.StringValue, functionModel.MinValue) < 0
                || string.Compare(m.StringValue, functionModel.MaxValue) > 0),

                MixCompareOperatorKind.Gte => m => m.AttributeFieldName == functionModel.FieldName
                && (string.Compare(m.StringValue, functionModel.Value) > 0
                || m.StringValue == functionModel.Value),

                MixCompareOperatorKind.Gt => m => m.AttributeFieldName == functionModel.FieldName
                && string.Compare(m.StringValue, functionModel.Value) > 0,

                MixCompareOperatorKind.Lte => m => m.AttributeFieldName == functionModel.FieldName
                && (string.Compare(m.StringValue, functionModel.Value) < 0
                || m.StringValue == functionModel.Value),

                MixCompareOperatorKind.Lt => m => m.AttributeFieldName == functionModel.FieldName
                && string.Compare(m.StringValue, functionModel.Value) < 0,

                _ => null,
            };
        }

        private static string buildLikeString(string stringValue)
        {
            return $"%{stringValue}%";
        }
    }


    public class ReplaceExpressionVisitor : ExpressionVisitor
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
