using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models;
using Mix.Cms.Lib.Models.Cms;
using Mix.Heart.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Linq.Expressions;

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

        public static Expression<Func<MixDatabaseDataValue, bool>> CreateExpression(
          JObject jsonQuery)
        {
            Expression<Func<MixDatabaseDataValue, bool>> root = null;
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

        private static Expression<Func<MixDatabaseDataValue, bool>> GetExpression(
         ExpressionModel expressionModel)
        {
            Expression<Func<MixDatabaseDataValue, bool>> root = null;
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
        private static Expression<Func<MixDatabaseDataValue, bool>> CombineExpressionByType(
            MixLogicalOperatorKind type,
            Expression<Func<MixDatabaseDataValue, bool>> expr1,
            Expression<Func<MixDatabaseDataValue, bool>> expr2)
        {
            return type switch
            {
                MixLogicalOperatorKind.And => expr1.AndAlsoIf(expr1 != null, expr2),
                MixLogicalOperatorKind.Or => expr1.OrIf(expr1 != null, expr2),
                _ => expr2,
            };
        }

        private static Expression<Func<MixDatabaseDataValue, bool>> GetFunction(FunctionModel functionModel)
        {
            return functionModel.Rule switch
            {
                MixCompareOperatorKind.Equal => m => m.MixDatabaseColumnName == functionModel.FieldName
                && EF.Functions.Like(m.StringValue, functionModel.Value),

                MixCompareOperatorKind.NotEqual => m => m.MixDatabaseColumnName == functionModel.FieldName
                && !EF.Functions.Like(m.StringValue, functionModel.Value),

                MixCompareOperatorKind.Contain => m => m.MixDatabaseColumnName == functionModel.FieldName
                && (EF.Functions.Like(m.StringValue, buildLikeString(functionModel.Value))),

                MixCompareOperatorKind.NotContain => m => m.MixDatabaseColumnName == functionModel.FieldName
                && !(EF.Functions.Like(m.StringValue, buildLikeString(functionModel.Value))),

                MixCompareOperatorKind.InRange => m => m.MixDatabaseColumnName == functionModel.FieldName
                && (string.Compare(m.StringValue, functionModel.MinValue) > 0
                && string.Compare(m.StringValue, functionModel.MaxValue) < 0),

                MixCompareOperatorKind.NotInRange => m => m.MixDatabaseColumnName == functionModel.FieldName
                && (string.Compare(m.StringValue, functionModel.MinValue) < 0
                || string.Compare(m.StringValue, functionModel.MaxValue) > 0),

                MixCompareOperatorKind.GreaterThanOrEqual => m => m.MixDatabaseColumnName == functionModel.FieldName
                && (string.Compare(m.StringValue, functionModel.Value) > 0
                || m.StringValue == functionModel.Value),

                MixCompareOperatorKind.GreaterThan => m => m.MixDatabaseColumnName == functionModel.FieldName
                && string.Compare(m.StringValue, functionModel.Value) > 0,

                MixCompareOperatorKind.LessThanOrEqual => m => m.MixDatabaseColumnName == functionModel.FieldName
                && (string.Compare(m.StringValue, functionModel.Value) < 0
                || m.StringValue == functionModel.Value),

                MixCompareOperatorKind.LessThan => m => m.MixDatabaseColumnName == functionModel.FieldName
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