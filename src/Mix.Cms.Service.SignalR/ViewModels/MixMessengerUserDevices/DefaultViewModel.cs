using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Messenger.Models.Data;
using Mix.Domain.Data.ViewModels;
using Newtonsoft.Json;
using System;
using Mix.Cms.Lib.Enums;
using Mix.Cms.Lib.Constants;
using Mix.Cms.Service.SignalR.Domain.Enums;

namespace Mix.Cms.Service.SignalR.ViewModels.MixMessengerUserDevices
{
    public class DefaultViewModel : ViewModelBase<MixChatServiceContext, MixMessengerUserDevice, DefaultViewModel>
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("userId")]
        public string UserId { get; set; }

        [JsonProperty("connectionId")]
        public string ConnectionId { get; set; }

        [JsonProperty("deviceId")]
        public string DeviceId { get; set; }

        [JsonProperty("status")]
        public MixDeviceStatus Status { get; set; }

        [JsonProperty("startDate")]
        public DateTime StartDate { get; set; }

        [JsonProperty("endDate")]
        public DateTime? EndDate { get; set; }

        #endregion Properties

        public DefaultViewModel()
        {
        }

        public DefaultViewModel(MixMessengerUserDevice model, MixChatServiceContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }
    }
}