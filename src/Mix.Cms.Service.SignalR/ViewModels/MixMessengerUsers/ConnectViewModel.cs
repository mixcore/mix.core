using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.ViewModels;
using Mix.Cms.Messenger.Models.Data;
using Mix.Cms.Service.SignalR.Models;
using Mix.Common.Helper;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Service.SignalR.ViewModels.MixMessengerUsers
{
    public class ConnectViewModel
    {
        #region Properties

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("avatar")]
        public string Avatar { get; set; }

        [JsonProperty("device")]
        public MixMessengerUserDevice Device { get; set; }

        [JsonProperty("createdDate")]
        public DateTime CreatedDate { get; set; }

        [JsonProperty("lastModified")]
        public DateTime? LastModified { get; set; }
        [JsonProperty("isJoin")]
        public bool IsJoin { get; set; }
        #endregion Properties

        #region Contructor

        public ConnectViewModel(MessengerConnection connection)
        {
            Id = connection.Id;
            Name = connection.Name;
            Avatar = connection.Avatar;
            Device = new MixMessengerUserDevice()
            {
                UserId = connection.Id,
                ConnectionId = connection.ConnectionId,
                DeviceId = connection.DeviceId,
            };
            // TODO - verify cnn before add/update connections         
        }

        private bool CheckIsJoin()
        {
            var getUser = MixMessengerUsers.DefaultViewModel.Repository.GetSingleModel(u => u.Id == Id);
            if (getUser.IsSucceed)
            {
                return getUser.Data.Status == Constants.Enums.OnlineStatus.Connected;
            }
            else
            {                
                return false;
            }
        }

        #endregion Contructor

        #region Override

        #region Async

        public async Task<RepositoryResponse<bool>> Join()
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            UnitOfWorkHelper<MixChatServiceContext>.InitTransaction(null, null, out MixChatServiceContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                result = await UpdateUser(context, transaction);

                if (result.IsSucceed)
                {
                    result = await UpdateDevice(context, transaction);
                }
                UnitOfWorkHelper<MixChatServiceContext>.HandleTransaction(result.IsSucceed, isRoot, transaction);
                return result;
            }
            catch (Exception ex)
            {
                return UnitOfWorkHelper<MixChatServiceContext>.HandleException<bool>(ex, isRoot, transaction);
            }
            finally
            {
                if (isRoot)
                {
                    //if current Context is Root
                    transaction.Dispose();
                    context.Dispose();
                }
            }            
        }

        private async Task<RepositoryResponse<bool>> UpdateUser(MixChatServiceContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            // Load User from db
            var getUser = await MixMessengerUsers.DefaultViewModel.Repository.GetSingleModelAsync(m => m.Id == Id, context, transaction);
            if (getUser.IsSucceed)
            {
                // if existed => update status = connected
                if (getUser.Data.Status == Constants.Enums.OnlineStatus.Disconnected)
                {
                    getUser.Data.Status = Constants.Enums.OnlineStatus.Connected;
                    var saveUser = await getUser.Data.SaveModelAsync(false, context, transaction);
                    ViewModelHelper.HandleResult(saveUser, ref result);
                }
            }
            else
            {
                // if not existed => add new with status  = connected
                var user = new MixMessengerUsers.DefaultViewModel()
                {
                    Id = Id,
                    FacebookId = Id,
                    Avatar = Avatar,
                    CreatedDate = DateTime.UtcNow,
                    Name = Name,
                    Status = Constants.Enums.OnlineStatus.Connected
                };
                var saveUser = await user.SaveModelAsync(false, context,transaction);
                ViewModelHelper.HandleResult(saveUser, ref result);
            }
            return result;
        }

        private async Task<RepositoryResponse<bool>> UpdateDevice(MixChatServiceContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (Device != null)
            {
                //var cnn = _context.MixMessengerUserDevice.FirstOrDefault(c => c.UserId == Device.UserId && c.DeviceId == Device.DeviceId);
                var getDevice = await MixMessengerUserDevices.DefaultViewModel.Repository.GetSingleModelAsync(c => c.UserId == Device.UserId && c.DeviceId == Device.DeviceId, context, transaction);
                if (getDevice.IsSucceed)
                {
                    getDevice.Data.Status = Constants.Enums.DeviceStatus.Actived;
                    getDevice.Data.StartDate = DateTime.UtcNow;
                    getDevice.Data.ConnectionId = Device.ConnectionId;
                    var saveDevice = await getDevice.Data.SaveModelAsync(false, context, transaction);
                    ViewModelHelper.HandleResult(saveDevice, ref result);
                }
                else
                {
                    Device.Status = (int)Constants.Enums.DeviceStatus.Actived;
                    Device.StartDate = DateTime.UtcNow;
                    var dv = new MixMessengerUserDevices.DefaultViewModel(Device, context, transaction);
                    var saveDevice = await dv.SaveModelAsync(false, context, transaction);
                    ViewModelHelper.HandleResult(saveDevice, ref result);
                }
            }
            return result;
        }

        #endregion Async

        #endregion Override
    }
}