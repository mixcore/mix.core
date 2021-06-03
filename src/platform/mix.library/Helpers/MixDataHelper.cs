namespace Mix.Lib.Helpers
{
    public class MixDataHelper
    {

        //public static MixDatabaseDataViewModel LoadAdditionalData(string parentId, string culture, string databaseName, MixCmsContext _context, IDbContextTransaction _transaction)
        //{
        //    return _context.MixDatabaseDataAssociation.Where(
        //        a => a.ParentId == parentId && a.Specificulture == culture && a.MixDatabaseName == databaseName)
        //        .Join(_context.MixDatabaseData, a => a.DataId, d => d.Id, (a, d) => new { a, d })
        //        .Select(ad => new MixDatabaseDataViewModel(ad.d, _context, _transaction)).First();
        //}

        //public static JObject ParseData(
        //    string dataId,
        //    string culture,
        //    MixCmsContext _context = null,
        //    IDbContextTransaction _transaction = null)
        //{
        //    UnitOfWorkHelper<MixCmsContext>.InitTransaction(
        //            _context, _transaction,
        //            out MixCmsContext context, out IDbContextTransaction transaction, out bool isRoot);
        //    var values = context.MixDatabaseDataValue.Where(
        //        m => m.DataId == dataId && m.Specificulture == culture
        //            && !string.IsNullOrEmpty(m.MixDatabaseColumnName));
        //    var properties = values.Select(m => m.ToJProperty());
        //    var obj = new JObject(
        //        new JProperty("id", dataId),
        //        properties
        //    );

        //    if (isRoot)
        //    {
        //        transaction.Dispose();
        //        context.Dispose();
        //    }

        //    return obj;
        //}

    }
}
