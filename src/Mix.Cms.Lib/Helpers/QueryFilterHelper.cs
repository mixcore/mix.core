using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Extensions;
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
                        .AddFunction(FunctionModel.Create(MixCompareOperatorKind.Equal, "ho_va_ten", "Tran Nhat Duy"))
                        .AddFunction(FunctionModel.Create(MixCompareOperatorKind.Equal, "status", "open"))
                ).AddExpression
                (
                    ExpressionModel.Create(MixLogicalOperatorKind.And)
                    .AddExpression
                    (
                        ExpressionModel.Create(MixLogicalOperatorKind.Or)
                            .AddFunction(FunctionModel.Create(MixCompareOperatorKind.Equal, "date", "2020/12/10", "2020/12/30"))
                            .AddFunction(FunctionModel.Create(MixCompareOperatorKind.Equal, "address", "Nguyen thi Minh Khai"))
                    ).AddExpression
                    (
                        ExpressionModel.Create(MixLogicalOperatorKind.And)
                            .AddFunction(FunctionModel.Create(MixCompareOperatorKind.GreaterThan, "age", "18"))
                            .AddFunction(FunctionModel.Create(MixCompareOperatorKind.Equal, "gender", "male"))
                    )
                );
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
            Expression<Func<MixAttributeSetValue, bool>> root = null;
            try
            {
                expressionModel?.Functions?.ForEach(function =>
                {
                    root = CombineExpressionByType(expressionModel.ExpressionType,
                        root, GetFunction(function));
                });


                expressionModel?.Expressions?.ForEach(expression =>
                {
                    root = CombineExpressionByType(expressionModel.ExpressionType,
                            root, GetExpression(expression));
                });

                return root;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
                return null;
            }
        }

        /// <summary>
        /// Combine Two Expressions by Type
        /// </summary>
        /// <param name="type"></param>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        private static Expression<Func<MixAttributeSetValue, bool>> CombineExpressionByType(
            MixLogicalOperatorKind type,
            Expression<Func<MixAttributeSetValue, bool>> expr1,
            Expression<Func<MixAttributeSetValue, bool>> expr2)
        {
            return type switch
            {
                MixLogicalOperatorKind.And => expr1.AndAlsoIf(expr1 != null, expr2),
                MixLogicalOperatorKind.Or => expr1.OrIf(expr1 != null, expr2),
                _ => expr2,
            };
        }
        private static Expression<Func<MixAttributeSetValue, bool>> GetFunction(FunctionModel functionModel)
        {
            return functionModel.Rule switch
            {
                MixCompareOperatorKind.Equal => m => m.AttributeFieldName == functionModel.FieldName
                && m.StringValue == functionModel.Value,

                MixCompareOperatorKind.NotEqual => m => m.AttributeFieldName == functionModel.FieldName
                && m.StringValue != functionModel.Value,

                MixCompareOperatorKind.Contain => m => m.AttributeFieldName == functionModel.FieldName
                && (EF.Functions.Like(m.StringValue, buildLikeString(functionModel.Value))),

                MixCompareOperatorKind.NotContain => m => m.AttributeFieldName == functionModel.FieldName
                && !(EF.Functions.Like(m.StringValue, buildLikeString(functionModel.Value))),

                MixCompareOperatorKind.InRange => m => m.AttributeFieldName == functionModel.FieldName
                && (string.Compare(m.StringValue, functionModel.MinValue) > 0
                && string.Compare(m.StringValue, functionModel.MaxValue) < 0),

                MixCompareOperatorKind.NotInRange => m => m.AttributeFieldName == functionModel.FieldName
                && (string.Compare(m.StringValue, functionModel.MinValue) < 0
                || string.Compare(m.StringValue, functionModel.MaxValue) > 0),

                MixCompareOperatorKind.GreaterThanOrEqual => m => m.AttributeFieldName == functionModel.FieldName
                && (string.Compare(m.StringValue, functionModel.Value) > 0
                || m.StringValue == functionModel.Value),

                MixCompareOperatorKind.GreaterThan => m => m.AttributeFieldName == functionModel.FieldName
                && string.Compare(m.StringValue, functionModel.Value) > 0,

                MixCompareOperatorKind.LessThanOrEqual => m => m.AttributeFieldName == functionModel.FieldName
                && (string.Compare(m.StringValue, functionModel.Value) < 0
                || m.StringValue == functionModel.Value),

                MixCompareOperatorKind.LessThan => m => m.AttributeFieldName == functionModel.FieldName
                && string.Compare(m.StringValue, functionModel.Value) < 0,

                _ => null,
            };
        }

        private static string buildLikeString(string stringValue)
        {
            return $"%{stringValue}%";
        }
    }
}
