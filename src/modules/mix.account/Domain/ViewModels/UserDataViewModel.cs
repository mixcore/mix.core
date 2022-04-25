using Newtonsoft.Json.Linq;

namespace Mix.Account.Domain.ViewModels
{
    public class UserDataViewModel
        : HaveParentContentViewModelBase<MixCmsContext, MixDataContent, Guid, UserDataViewModel>
    {
        #region Contructors

        public UserDataViewModel()
        {
        }

        public UserDataViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public UserDataViewModel(MixDataContent entity,
            UnitOfWorkInfo uowInfo = null) : base(entity, uowInfo)
        {
        }


        #endregion

        #region Properties
        public int MixDatabaseId { get; set; }
        public string MixDatabaseName { get; set; }
        public JObject Data { get; set; }

        public Guid? GuidParentId { get; set; }
        public int? IntParentId { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        #endregion

        #region Overrides

        public override async Task ExpandView()
        {
            if (Data == null)
            {

                using var colRepo = MixDatabaseColumnViewModel.GetRepository(UowInfo);
                using var valRepo = MixDataContentValueViewModel.GetRepository(UowInfo);

                var Columns = await colRepo.GetListAsync(m => m.MixDatabaseName == MixDatabaseName);
                var Values = await valRepo.GetListAsync(m => m.ParentId == Id);

                Data = MixDataHelper.ParseData(Id, UowInfo);

                await Data.LoadAllReferenceDataAsync(Id, MixDatabaseName, UowInfo);
            }
        }
        #endregion
    }
}
