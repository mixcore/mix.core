using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Infrastructure.Models;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Enums;
using Newtonsoft.Json.Linq;

namespace Mix.Cms.Lib.Infrastructure.Helper
{
    public class QueryFilterHelper
    {
        public void TestCreateModel()
        {
            ExpressionModel.Create("and")
                .AddExpression
                (
                    ExpressionModel.Create("and")
                        .AddFunction(FunctionModel.Create("equal", "ho_va_ten", "Tran Nhat Duy"))
                        .AddFunction(FunctionModel.Create("equal", "status", "open"))
                ).AddExpression
                (
                    ExpressionModel.Create("and")
                    .AddExpression
                    (
                        ExpressionModel.Create("or")
                            .AddFunction(FunctionModel.Create("equal", "date",  "2020/12/10", "2020/12/30"))
                            .AddFunction(FunctionModel.Create("equal", "address", "Nguyen thi Minh Khai"))
                    ).AddExpression
                    (
                        ExpressionModel.Create("and")
                            .AddFunction(FunctionModel.Create("greater", "age", "18"))
                            .AddFunction(FunctionModel.Create("equal", "gender", "male"))
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
        private static Expression<Func<MixAttributeSetValue, bool>> CombineExpressionByType(string type,
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
                "and" => expr1 != null ? Expression.Lambda<Func<MixAttributeSetValue, bool>>(Expression.AndAlso(left, right), parameter) :
                                        Expression.Lambda<Func<MixAttributeSetValue, bool>>(right, parameter),
                "or"  => expr1 != null ? Expression.Lambda<Func<MixAttributeSetValue, bool>>(Expression.OrElse(left, right), parameter) :
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
            string expressionType = expressionModel.ExpressionType;

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
            Enum.TryParse(functionModel.Rule, out Heart.Enums.MixHeartEnums.ExpressionRule rule);
            return rule switch
            {
                Heart.Enums.MixHeartEnums.ExpressionRule. Eq => m => m.AttributeFieldName == functionModel.FieldName
                 && m.StringValue == functionModel.Value,

                Heart.Enums.MixHeartEnums.ExpressionRule.Neq => m => m.AttributeFieldName == functionModel.FieldName
                && m.StringValue != functionModel.Value,

                Heart.Enums.MixHeartEnums.ExpressionRule.Ct => m => m.AttributeFieldName == functionModel.FieldName
                && (EF.Functions.Like(m.StringValue, $"{functionModel.Value}")),

                Heart.Enums.MixHeartEnums.ExpressionRule.Nct => m => m.AttributeFieldName == functionModel.FieldName
                && !(EF.Functions.Like(m.StringValue, $"{functionModel.Value}")),

                Heart.Enums.MixHeartEnums.ExpressionRule.Ra => m => m.AttributeFieldName == functionModel.FieldName
                && (string.Compare(m.StringValue, functionModel.MinValue) > 0
                && string.Compare(m.StringValue, functionModel.MaxValue) < 0),

                Heart.Enums.MixHeartEnums.ExpressionRule.Nra => m => m.AttributeFieldName == functionModel.FieldName
                && (string.Compare(m.StringValue, functionModel.MinValue) < 0
                || string.Compare(m.StringValue, functionModel.MaxValue) > 0),

                Heart.Enums.MixHeartEnums.ExpressionRule.Gte => m => m.AttributeFieldName == functionModel.FieldName
                && (string.Compare(m.StringValue, functionModel.Value) > 0
                || m.StringValue == functionModel.Value),

                Heart.Enums.MixHeartEnums.ExpressionRule.Gt => m => m.AttributeFieldName == functionModel.FieldName
                && string.Compare(m.StringValue, functionModel.Value) > 0,

                Heart.Enums.MixHeartEnums.ExpressionRule.Lte => m => m.AttributeFieldName == functionModel.FieldName
                && (string.Compare(m.StringValue, functionModel.Value) < 0
                || m.StringValue == functionModel.Value),

                Heart.Enums.MixHeartEnums.ExpressionRule.Lt => m => m.AttributeFieldName == functionModel.FieldName
                && string.Compare(m.StringValue, functionModel.Value) < 0,

                _ => null,
            };
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
