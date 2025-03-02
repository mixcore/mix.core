using Mix.Constant.Enums;
using Mix.Database.Entities.MixDb;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Newtonsoft.Json.Linq;

namespace Mix.Mixdb.ViewModels
{
    public class MixUserDataViewModel : ViewModelBase<MixDbDbContext, MixUserData, int, MixUserDataViewModel>
    {
        #region Properties

        public Guid ParentId { get; set; }
        public MixDatabaseParentType ParentType { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Fullname { get; set; }
        public string? Avatar { get; set; }
        public string? Gender { get; set; }
        public int TenantId { get; set; }

        public JArray Endpoints { get; set; } = new();
        #endregion
        #region Contructors
        public MixUserDataViewModel()
        {
        }

        public MixUserDataViewModel(MixDbDbContext context) : base(context)
        {
        }

        public MixUserDataViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixUserDataViewModel(MixUserData entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }
        #endregion
        #region Overrides

        #endregion

        #region Methods


        #endregion

        #region Methods



        #endregion
    }
}
