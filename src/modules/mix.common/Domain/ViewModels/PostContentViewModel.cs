using Microsoft.EntityFrameworkCore;
using Mix.RepoDb.Repositories;
using RepoDb;
using RepoDb.Enumerations;
using System.Reflection.Emit;

namespace Mix.Common.Domain.ViewModels
{
    public class PostContentViewModel
        : ExtraColumnMultilingualSEOContentViewModelBase
            <MixCmsContext, MixPostContent, int, PostContentViewModel>
    {
        #region Constructors

        public PostContentViewModel()
        {
        }

        public PostContentViewModel(MixPostContent entity, UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }

        public PostContentViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }
        #endregion

        #region Properties

        public string ClassName { get; set; }

        public string DetailUrl => $"/post/{Id}/{SeoName}";

        public JObject AdditionalData { get; set; }
        #endregion

        #region Overrides
        public override async Task ExpandView(CancellationToken cancellationToken = default)
        {
            await base.ExpandView(cancellationToken);
        }

        #endregion

        #region Public Method


        public T Property<T>(string fieldName)
        {
            return AdditionalData != null
                ? AdditionalData.Value<T>(fieldName)
                : default;
        }

        public async Task LoadAdditionalDataAsync(MixRepoDbRepository mixRepoDbRepository)
        {
            if (AdditionalData == null)
            {
                var relationships = Context.MixDatabaseRelationship.Where(m => m.SourceDatabaseName == MixDatabaseName).ToList();
                mixRepoDbRepository.InitTableName(MixDatabaseName);
                var obj = await mixRepoDbRepository.GetSingleByParentAsync(MixContentType.Post, Id);
                AdditionalData = obj != null ? ReflectionHelper.ParseObject(obj) : null;

                foreach (var item in relationships)
                {

                    mixRepoDbRepository.InitTableName(item.DestinateDatabaseName);
                    var allowsIds = Context.MixDatabaseAssociation
                            .Where(m => m.ParentDatabaseName == MixDatabaseName
                                        && m.ParentId == AdditionalData.Value<int>("id")
                                        && m.ChildDatabaseName == item.DestinateDatabaseName)
                            .Select(m => m.ChildId).ToList();
                    var queries = new List<QueryField>()
                    {
                        new QueryField("Id", Operation.In, allowsIds)
                    };
                    var data = await mixRepoDbRepository.GetListByAsync(queries);
                    var arr = new JArray();
                    foreach (var dataItem in data)
                    {
                        arr.Add(ReflectionHelper.ParseObject(dataItem));
                    }
                    AdditionalData.Add(new JProperty(item.DisplayName, JArray.FromObject(arr)));
                }

            }
        }
        #endregion

        #region Private Methods


        #endregion
    }
}
