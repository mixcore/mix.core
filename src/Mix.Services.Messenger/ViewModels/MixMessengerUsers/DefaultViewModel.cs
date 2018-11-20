using Microsoft.EntityFrameworkCore.Storage;
using Mix.Domain.Data.ViewModels;
using Mix.Services.Messenger.Models.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mix.Services.Messenger.ViewModels.MixMessengerUsers
{
    public class DefaultViewModel : ViewModelBase<MixChatServiceContext, MixMessengerUser, DefaultViewModel>
    {
        #region Properties
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("connections")]
        List<MixMessengerUserDevices.DefaultViewModel> Connections { get; set; }
        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }
        [JsonProperty("status")]
        public MixChatEnums.OnlineStatus Status { get; set; }
        #endregion

        #region Contructor
        public DefaultViewModel()
        {
        }

        public DefaultViewModel(MixMessengerUser model, MixChatServiceContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }
        #endregion

        #region Override

        #region Sync
        public override void ExpandView(MixChatServiceContext _context = null, IDbContextTransaction _transaction = null)
        {
            this.Connections = MixMessengerUserDevices.DefaultViewModel.Repository.GetModelListBy(m => m.UserId == this.Id).Data;
        }
        #endregion

        #region Async
        public override async Task<bool> ExpandViewAsync(MixChatServiceContext _context = null, IDbContextTransaction _transaction = null)
        {
            this.Connections = ( await MixMessengerUserDevices.DefaultViewModel.Repository.GetModelListByAsync(m => m.UserId == this.Id)).Data;
            return Connections != null;
        }
        #endregion

        #endregion
    }
}
