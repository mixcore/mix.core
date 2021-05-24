using Microsoft.EntityFrameworkCore.Storage;
using Mix.Cms.Lib.SignalR.Enums;
using Mix.Cms.Lib.SignalR.Models;
using Mix.Cms.Messenger.Models.Data;
using Mix.Common.Helper;
using Mix.Heart.Infrastructure.ViewModels;
using Mix.Heart.Models;
using Newtonsoft.Json;
using System;

namespace Mix.Cms.Service.ViewModels.SignalR.MixMessengerUsers
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
                return getUser.Data.Status == OnlineStatus.Connected;
            }
            else
            {
                return false;
            }
        }

        #endregion Contructor

        #region Override

        #region Async

        // Cannot use asyn method for signalr hub
        public RepositoryResponse<bool> Join(MixChatServiceContext _context = null, IDbContextTransaction _transaction = null)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            UnitOfWorkHelper<MixChatServiceContext>.InitTransaction(_context, _transaction, out MixChatServiceContext context, out IDbContextTransaction transaction, out bool isRoot);
            try
            {
                result = UpdateUser(context, transaction);

                if (result.IsSucceed)
                {
                    result = UpdateDevice(context, transaction);
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
                    UnitOfWorkHelper<MixChatServiceContext>.CloseDbContext(ref context, ref transaction);
                }
            }
        }

        private RepositoryResponse<bool> UpdateUser(MixChatServiceContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            // Load User from db
            var getUser = MixMessengerUsers.DefaultViewModel.Repository.GetSingleModel(m => m.Id == Id, context, transaction);
            if (getUser.IsSucceed)
            {
                // if existed => update status = connected
                if (getUser.Data.Status == OnlineStatus.Disconnected)
                {
                    getUser.Data.Status = OnlineStatus.Connected;
                    var saveUser = getUser.Data.SaveModel(false, context, transaction);
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
                    Status = OnlineStatus.Connected
                };
                var saveUser = user.SaveModel(false, context, transaction);
                ViewModelHelper.HandleResult(saveUser, ref result);
            }
            return result;
        }

        private RepositoryResponse<bool> UpdateDevice(MixChatServiceContext context, IDbContextTransaction transaction)
        {
            var result = new RepositoryResponse<bool>() { IsSucceed = true };
            if (Device != null)
            {
                var getDevice = MixMessengerUserDevices.DefaultViewModel.Repository.GetSingleModel(c => c.UserId == Device.UserId && c.DeviceId == Device.DeviceId, context, transaction);
                if (getDevice.IsSucceed)
                {
                    getDevice.Data.Status = DeviceStatus.Actived;
                    getDevice.Data.StartDate = DateTime.UtcNow;
                    getDevice.Data.ConnectionId = Device.ConnectionId;
                    var saveDevice = getDevice.Data.SaveModel(false, context, transaction);
                    ViewModelHelper.HandleResult(saveDevice, ref result);
                }
                else
                {
                    Device.Status = (int)DeviceStatus.Actived;
                    Device.StartDate = DateTime.UtcNow;
                    var dv = new MixMessengerUserDevices.DefaultViewModel(Device, context, transaction);
                    var saveDevice = dv.SaveModel(false, context, transaction);
                    ViewModelHelper.HandleResult(saveDevice, ref result);
                }
            }
            return result;
        }

        #endregion Async

        #endregion Override
    }
}