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

        private static Func<MixAttributeSetValue, bool> GetBinaryExpressionByType(string type,
            Func<MixAttributeSetValue, bool> left,
            Func<MixAttributeSetValue, bool> right)
        {
            return type switch
            {
                "and" => m => left != null ? (left(m) && right(m)) : right(m),
                "or" => m => left != null ? (left(m) || right(m)) : right(m),
                "nand" => m => left != null ? !(left(m) && right(m)) : !right(m),
                "nor" => m => left != null ? !(left(m) || right(m)) : !right(m),
                _ => null,
            };
        }

        private static Expression<Func<MixAttributeSetValue, bool>> FuncToExpression(Func<MixAttributeSetValue, bool> f)
        {
            return x => f(x);
        }


        public static Expression<Func<MixAttributeSetValue, bool>> CreateExpression  (
          JObject jsonQuery, string name = "model")
        {

            var parameter = Expression.Parameter(typeof(MixAttributeSetValue), name);

            Func<MixAttributeSetValue, bool> root;
            try
            {
                ExpressionModel expressionModel = jsonQuery.ToObject<ExpressionModel>();
                root = GetExpression(expressionModel);

                return FuncToExpression(root);
            }
            catch(Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return null;
        }

        private static Func<MixAttributeSetValue, bool> GetExpression(
         ExpressionModel expressionModel)
        {

            Func<MixAttributeSetValue, bool> root = null;
            try
            {
                expressionModel?.Functions?.ForEach(function => {
                    Console.Write("Function " + function.Rule);
                    root = GetBinaryExpressionByType(expressionModel.ExpressionType,
                        root, GetFunction(function));
                });
              

                expressionModel?.Expressions?.ForEach(expression =>
                {
                    Console.Write("root.ToString() " + expression.ToString());
                    root = GetBinaryExpressionByType(expressionModel.ExpressionType,
                            root, GetExpression(expression));
                });
               
                return root;
            }
            catch (Exception ex)
            {
                Console.Write(ex.ToString());
            }

            return null;
        }

        private static Func<MixAttributeSetValue, bool> GetFunction(FunctionModel functionModel)
        {
            return functionModel.Rule switch
            {
                "equal" => m => m.AttributeFieldName == functionModel.FieldName
                                         && (EF.Functions.Like(m.StringValue, $"{functionModel.Value}")),
                "contain" => m => m.AttributeFieldName == functionModel.FieldName &&
                                        (EF.Functions.Like(m.StringValue, $"%{functionModel.Value}%")),
                _ => null,
            };
        }
    }
}
