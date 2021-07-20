using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Models.Cms;
using Mix.Cms.Lib.Services;
using System;
using System.Linq.Expressions;

namespace Mix.Cms.Lib.ViewModels.MixPosts
{
    public class Expressions
    {
        public static Expression<Func<MixDatabaseDataValue, bool>> GetMetaExpression(string databaseName, string slug, string culture = null)
        {
            culture ??= MixService.GetAppSetting<string>(MixAppSettingKeywords.DefaultCulture);
            Expression<Func<MixDatabaseDataValue, bool>> valExp =
                m => m.Specificulture == culture
                    && m.Status == MixContentStatus.Published
                    && m.MixDatabaseName == databaseName
                    && m.MixDatabaseColumnName == "slug" && EF.Functions.Like(m.StringValue, slug);
            return valExp;
        }
    }
}
