using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;
using Mix.Lib.Attributes;
using Mix.Servives.Permission.Domain.Entities;

namespace Mix.Servives.Permission.Domain.ViewModels
{
    [GenerateRestApiController(Route ="api/v2/permission")]
    public class MixPermissionViewModel : ViewModelBase<PermissionDbContext, MixPermission, int, MixPermissionViewModel>
    {
        public MixPermissionViewModel()
        {
        }

        public MixPermissionViewModel(PermissionDbContext context) : base(context)
        {
        }

        public MixPermissionViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixPermissionViewModel(MixPermission entity, UnitOfWorkInfo uowInfo) : base(entity, uowInfo)
        {
        }
    }
}
