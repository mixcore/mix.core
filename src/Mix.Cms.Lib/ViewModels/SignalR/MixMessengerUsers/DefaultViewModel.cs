using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.SignalR.Enums;
using Mix.Cms.Messenger.Models.Data;
using Mix.Heart.Infrastructure.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Mix.Cms.Service.ViewModels.SignalR.MixMessengerUsers
{
    public class DefaultViewModel : ViewModelBase<MixChatServiceContext, MixMessengerUser, DefaultViewModel>
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("facebookId")]
        public string FacebookId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("connections")]
        private List<Mix.Cms.Service.ViewModels.SignalR.MixMessengerUserDevices.DefaultViewModel> Connections { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        [JsonProperty("status")]
        public OnlineStatus Status { get; set; }

        #endregion Properties

        #region Contructor

        public DefaultViewModel()
        {
        }

        public DefaultViewModel(MixMessengerUser model, MixChatServiceContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }

        #endregion Contructor

        #region Override

        #region Sync

        public override void ExpandView(MixChatServiceContext _context = null, IDbContextTransaction _transaction = null)
        {
            this.Connections = Mix.Cms.Service.ViewModels.SignalR.MixMessengerUserDevices.DefaultViewModel.Repository.GetModelListBy(m => m.UserId == this.Id).Data;
        }

        #endregion Sync

        #endregion Override
    }
}