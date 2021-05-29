using Microsoft.EntityFrameworkCore.Storage;
using Mix.Common.Helper;
using Mix.Lib.Entities.Cms;
using Mix.Lib.Extensions;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Mix.Lib.Helpers
{
    public static class MixDataHelper
    {

        public static JObject ParseData(
            string dataId,
            string culture,
            MixCmsContext _context = null,
            IDbContextTransaction _transaction = null)
        {
            UnitOfWorkHelper<MixCmsContext>.InitTransaction(
                    _context, _transaction,
                    out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
            var values = context.MixDatabaseDataValue.Where(
                m => m.DataId == dataId && m.Specificulture == culture
                    && !string.IsNullOrEmpty(m.MixDatabaseColumnName));
            var properties = values.Select(m => m.ToJProperty());
            var obj = new JObject(
                new JProperty("id", dataId),
                properties
            );

            if (isRoot)
            {
                transaction.Dispose();
                context.Dispose();
            }

            return obj;
        }

    }
}
