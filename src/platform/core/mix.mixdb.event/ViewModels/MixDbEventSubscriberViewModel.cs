using Mix.Database.Entities.MixDb;
using Mix.Heart.UnitOfWork;
using Mix.Heart.ViewModel;

using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Mixdb.Event.ViewModels
{
    public sealed class MixDbEventSubscriberViewModel : ViewModelBase<MixDbDbContext, MixDbEventSubscriber, int, MixDbEventSubscriberViewModel>
    {
        #region Contructors

        public MixDbEventSubscriberViewModel()
        {
        }

        public MixDbEventSubscriberViewModel(MixDbDbContext context) : base(context)
        {
        }

        public MixDbEventSubscriberViewModel(UnitOfWorkInfo unitOfWorkInfo) : base(unitOfWorkInfo)
        {
        }

        public MixDbEventSubscriberViewModel(MixDbEventSubscriber entity, UnitOfWorkInfo? uowInfo) : base(entity, uowInfo)
        {
        }

        #endregion

        #region Properties
        public int MixTenantId { get; set; }
        public string? MixDbName { get; set; }
        public string? Action { get; set; }
        public JObject? Callback { get; set; }
        #endregion
    }
}
