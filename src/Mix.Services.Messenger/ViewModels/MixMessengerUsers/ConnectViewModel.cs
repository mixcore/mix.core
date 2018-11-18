using Microsoft.EntityFrameworkCore.Storage;
using Mix.Domain.Core.ViewModels;
using Mix.Domain.Data.ViewModels;
using Mix.Services.Messenger.Models;
using Mix.Services.Messenger.Models.Data;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Services.Messenger.ViewModels.MixMessengerUsers
{
    public class ConnectViewModel : ViewModelBase<MixChatServiceContext, MixMessengerUser, ConnectViewModel>
    {
        #region Properties
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("avatar")]
        public string Avatar { get; set; }
        [JsonProperty("connections")]
        MixMessengerUserDevices.DefaultViewModel Connection { get; set; }
        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }
        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }

        #endregion

        #region Contructor
        public ConnectViewModel()
        {
        }

        public ConnectViewModel(MixMessengerUser model, MixChatServiceContext _context = null, IDbContextTransaction _transaction = null) : base(model, _context, _transaction)
        {
        }
        public ConnectViewModel(MessengerConnection connection)
        {
            Id = connection.Id;
            Name = connection.Name;
            Avatar = connection.Avatar;
            Connection = new MixMessengerUserDevices.DefaultViewModel()
            {
                ConnectionId = connection.ConnectionId,
                DeviceId = connection.DeviceId,
            };
            // TODO - verify cnn before add/update connections
        }
        #endregion

        #region Override

        #region Sync
        public override RepositoryResponse<bool> SaveSubModels(MixMessengerUser parent, MixChatServiceContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                if (result.IsSucceed)
                {
                    if (Connection != null)
                    {
                        Connection.UserId = parent.Id;
                        var cnn = _context.MixMessengerUserDevice.FirstOrDefault(c => c.UserId == Connection.UserId && c.DeviceId == Connection.DeviceId);
                        if (cnn != null)
                        {
                            cnn.ConnectionId = Connection.ConnectionId;
                            cnn.Status = (int)MixChatEnums.DeviceStatus.Actived;
                            cnn.StartDate = DateTime.UtcNow;
                            _context.Entry(cnn).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                            _context.SaveChanges();
                        }
                        else
                        {
                            Connection.Status = MixChatEnums.DeviceStatus.Actived;
                            Connection.StartDate = DateTime.UtcNow;
                            var saveResult = Connection.SaveModel(true, _context, _transaction);
                            result.IsSucceed = saveResult.IsSucceed;
                            if (!result.IsSucceed)
                            {
                                result.Errors.AddRange(saveResult.Errors);
                                result.Exception = saveResult.Exception;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
            }
            return result;
        }
        #endregion

        #region Async
        public override async Task<RepositoryResponse<bool>> SaveSubModelsAsync(MixMessengerUser parent, MixChatServiceContext _context, IDbContextTransaction _transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            try
            {
                if (result.IsSucceed)
                {
                    if (Connection != null)
                    {
                        Connection.UserId = parent.Id;
                        var cnn = _context.MixMessengerUserDevice.FirstOrDefault(c => c.UserId == Connection.UserId && c.DeviceId == Connection.DeviceId);
                        if (cnn != null)
                        {
                            cnn.ConnectionId = Connection.ConnectionId;
                            cnn.Status = (int)MixChatEnums.DeviceStatus.Actived;
                            cnn.StartDate = DateTime.UtcNow;
                            Connection = new MixMessengerUserDevices.DefaultViewModel(cnn);
                            
                        }
                        else
                        {
                            Connection.Status = MixChatEnums.DeviceStatus.Actived;
                            Connection.StartDate = DateTime.UtcNow;
                        }
                        var saveResult = await Connection.SaveModelAsync(true, _context, _transaction);
                        result.IsSucceed = saveResult.IsSucceed;
                        if (!result.IsSucceed)
                        {
                            result.Errors.AddRange(saveResult.Errors);
                            result.Exception = saveResult.Exception;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSucceed = false;
                result.Exception = ex;
            }
            return result;
        }
        #endregion

        #endregion
    }
}
