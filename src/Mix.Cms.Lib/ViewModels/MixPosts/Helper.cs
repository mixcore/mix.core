using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.Models.Cms;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.Repository;
using Mix.Domain.Data.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection;

namespace Mix.Cms.Lib.ViewModels.MixPosts
{
    public class Helper
    {
        /// <summary>
        /// Gets the modelist by meta.
        /// </summary>
        /// <typeparam name="TView">The type of the view.</typeparam>
        /// <param name="culture">The culture.</param>
        /// <param name="metaName">Name of the meta. Ex: sys_tag / sys_category</param>
        /// <param name="metaValue">The meta value.</param>
        /// <param name="orderByPropertyName">Name of the order by property.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="pageIndex">Index of the page.</param>
        /// <param name="_context">The context.</param>
        /// <param name="_transaction">The transaction.</param>
        /// <returns></returns>
        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetModelistByMeta<TView>(
            string metaName, string metaValue, string culture
            , string orderByPropertyName, int direction, int? pageSize, int? pageIndex
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var result = new RepositoryResponse<PaginationModel<TView>>()
                {
                    IsSucceed = true,
                    Data = new PaginationModel<TView>()
                    {
                        PageIndex = pageIndex.HasValue ? pageIndex.Value : 0,
                        PageSize = pageSize
                    }
                };
                var tasks = new List<Task<RepositoryResponse<TView>>>();
                // Get Tag
                var getVal = await MixAttributeSetValues.ReadViewModel.Repository.GetSingleModelAsync(m => m.AttributeSetName == metaName && m.StringValue == metaValue
                , context, transaction);
                if (getVal.IsSucceed)
                {
                    var getRelatedData = await MixRelatedAttributeDatas.ReadViewModel.Repository.GetModelListByAsync(
                        m => m.Specificulture == culture && m.DataId == getVal.Data.DataId
                        && m.ParentType == (int)MixEnums.MixAttributeSetDataType.Post
                        , orderByPropertyName, direction, pageIndex, pageSize
                        , _context: context, _transaction: transaction
                        );
                    if (getRelatedData.IsSucceed)
                    {
                        foreach (var item in getRelatedData.Data.Items)
                        {
                            if (int.TryParse(item.ParentId, out int postId))
                            {
                                var getData = await DefaultRepository<MixCmsContext, MixPost, TView>.Instance.GetSingleModelAsync(
                                m => m.Specificulture == item.Specificulture && m.Id == postId
                                    , context, transaction);
                                if (getData.IsSucceed)
                                {
                                    result.Data.Items.Add(getData.Data);
                                }
                            }
                        }
                        result.Data.TotalItems = getRelatedData.Data.TotalItems;
                        result.Data.TotalPage = getRelatedData.Data.TotalPage;
                    }
                    //var query = context.MixRelatedAttributeData.Where(m=> m.Specificulture == culture
                    //    && m.Id == getVal.Data.DataId && m.ParentId == parentId && m.ParentType == (int) MixEnums.MixAttributeSetDataType.Post)
                    //    .Select(m => m.ParentId).Distinct().ToList();
                }
                Expression<Func<MixAttributeSetValue, bool>> valPredicate = m => m.Specificulture == culture && m.AttributeSetName == metaName && m.StringValue == metaValue;

                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<TView>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    context.Database.CloseConnection(); transaction.Dispose(); context.Dispose();
                }
            }
        }
        
        public static async Task<RepositoryResponse<PaginationModel<TView>>> GetModelistByAddictionalField<TView>(
            string fieldName, object fieldValue, string culture, MixEnums.MixDataType dataType
            , MixEnums.CompareType filterType = MixEnums.CompareType.Eq
            , string orderByPropertyName=null, int direction = 0, int? pageSize = null, int? pageIndex = null
            , MixCmsContext _context = null, IDbContextTransaction _transaction = null)
            where TView : ViewModelBase<MixCmsContext, MixPost, TView>
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(_context, _transaction, out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                var result = new RepositoryResponse<PaginationModel<TView>>()
                {
                    IsSucceed = true,
                    Data = new PaginationModel<TView>()
                    {
                        PageIndex = pageIndex.HasValue ? pageIndex.Value : 0,
                        PageSize = pageSize
                    }
                };
                // Get Value Predicate By Type
                Expression<Func<MixAttributeSetValue, bool>> valPredicate = m => m.AttributeSetName == MixConstants.AttributeSetName.ADDITIONAL_FIELD_POST
                   && m.AttributeFieldName == fieldName;

                var pre = GetValuePredicate(fieldValue.ToString(), filterType, dataType);
                if (pre!=null)
                {
                    valPredicate = Mix.Heart.Helpers.ReflectionHelper.CombineExpression(valPredicate, pre, Heart.Enums.MixEnums.ExpressionMethod.And);
                }

                var query = context.MixAttributeSetValue.Where(valPredicate).Select(m => m.DataId).Distinct();
                var dataIds = query.ToList();
                var relatedQuery = context.MixRelatedAttributeData.Where(
                         m => m.ParentType == (int)MixEnums.MixAttributeSetDataType.Post && m.Specificulture == culture 
                            && dataIds.Any(d => d == m.DataId));
                var postIds = relatedQuery.Select(m=> int.Parse(m.ParentId)).Distinct().AsEnumerable().ToList();
                result = await DefaultRepository<MixCmsContext, MixPost, TView>.Instance.GetModelListByAsync(
                    m => m.Specificulture == culture && postIds.Any(p => p == m.Id)
                    , orderByPropertyName, direction
                    , pageSize ?? 100, pageIndex ?? 0
                    , null, null
                    , context, transaction);
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixCmsContext>.HandleException<PaginationModel<TView>>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    context.Database.CloseConnection(); transaction.Dispose(); context.Dispose();
                }
            }
        }

        private static Expression<Func<MixAttributeSetValue, bool>> GetValuePredicate(string fieldValue
            , MixEnums.CompareType filterType, MixEnums.MixDataType dataType)
        {
            Expression<Func<MixAttributeSetValue, bool>> valPredicate = null;
            switch (dataType)
            {
                case MixEnums.MixDataType.Date:
                case MixEnums.MixDataType.Time:
                    if (DateTime.TryParse(fieldValue, out DateTime dtValue))
                    {
                        valPredicate = FilterObjectSet<MixAttributeSetValue, DateTime>("DateTimeValue", dtValue, filterType);
                    }
                    break;
                case MixEnums.MixDataType.Double:
                    if (double.TryParse(fieldValue, out double dbValue))
                    {
                        valPredicate = FilterObjectSet<MixAttributeSetValue, double>("DoubleValue", dbValue, filterType);
                    }
                    break;
                case MixEnums.MixDataType.Boolean:
                    if (bool.TryParse(fieldValue, out bool boolValue))
                    {
                        valPredicate = FilterObjectSet<MixAttributeSetValue, bool>("BooleanValue", boolValue, filterType);
                    }
                    break;
                case MixEnums.MixDataType.Number:
                    if (int.TryParse(fieldValue, out int intValue))
                    {
                        valPredicate = FilterObjectSet<MixAttributeSetValue, int>("IntegerValue", intValue, filterType);
                    }
                    break;
                case MixEnums.MixDataType.Reference:
                    break;
                case MixEnums.MixDataType.Duration:
                case MixEnums.MixDataType.Custom:
                case MixEnums.MixDataType.DateTime:
                case MixEnums.MixDataType.PhoneNumber:
                case MixEnums.MixDataType.Text:
                case MixEnums.MixDataType.Html:
                case MixEnums.MixDataType.MultilineText:
                case MixEnums.MixDataType.EmailAddress:
                case MixEnums.MixDataType.Password:
                case MixEnums.MixDataType.Url:
                case MixEnums.MixDataType.ImageUrl:
                case MixEnums.MixDataType.CreditCard:
                case MixEnums.MixDataType.PostalCode:
                case MixEnums.MixDataType.Upload:
                case MixEnums.MixDataType.Color:
                case MixEnums.MixDataType.Icon:
                case MixEnums.MixDataType.VideoYoutube:
                case MixEnums.MixDataType.TuiEditor:
                case MixEnums.MixDataType.QRCode:
                default:
                    valPredicate = FilterObjectSet<MixAttributeSetValue, string>("StringValue", fieldValue, filterType);
                    break;
            }
            
            return valPredicate;
        }

        public static Expression<Func<TModel, bool>> FilterObjectSet<TModel, T>(string propName, T data2, MixEnums.CompareType filterType)
        {
            Type type = typeof(TModel);
            var par = Expression.Parameter(type, "model");

            Type fieldPropertyType;
            Expression fieldPropertyExpression;

            FieldInfo fieldInfo = type.GetField(propName);

            if (fieldInfo == null)
            {
                PropertyInfo propertyInfo = type.GetProperty(propName);

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
           
            BinaryExpression eq = null;
            switch (filterType)
            {
                case MixEnums.CompareType.Eq:
                    eq = Expression.Equal(fieldPropertyExpression,
                                     Expression.Constant(data2, fieldPropertyType));
                    break;
                case MixEnums.CompareType.Lt:
                    eq = Expression.LessThan(fieldPropertyExpression,
                                     Expression.Constant(data2, fieldPropertyType));
                    break;
                case MixEnums.CompareType.Gt:
                    eq = Expression.GreaterThan(fieldPropertyExpression,
                                     Expression.Constant(data2, fieldPropertyType));
                    break;
                case MixEnums.CompareType.Lte:
                    eq = Expression.LessThanOrEqual(fieldPropertyExpression,
                                     Expression.Constant(data2, fieldPropertyType));
                    break;
                case MixEnums.CompareType.Gte:
                    eq = Expression.GreaterThanOrEqual(fieldPropertyExpression,
                                     Expression.Constant(data2, fieldPropertyType));
                    break;
                case MixEnums.CompareType.In:
                    var method = typeof(string).GetMethod("Contains");
                    var call = Expression.Call(par, method, Expression.Constant(data2, typeof(string)));
                    return Expression.Lambda<Func<TModel, bool>>(call, par);
                default:
                    break;
            }           
            return Expression.Lambda<Func<TModel, bool>>(eq, par);
        }
    }
}