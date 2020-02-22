using Mix.Cms.Messenger.Models;
using Mix.Cms.Messenger.Models.Data;
using Mix.Domain.Core.ViewModels;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Mix.Cms.Messenger.ViewModels.MixMessengerUsers
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

        #endregion Contructor

        #region Override

        #region Async

        public async Task<RepositoryResponse<bool>> Join()
        {
            using (MixChatServiceContext _context = new MixChatServiceContext())
            {
                var result = new RepositoryResponse<bool>() { IsSucceed = true };
                try
                {
                    var user = new MixMessengerUser()
                    {
                        Id = Id,
                        FacebookId = Id,
                        Avatar = Avatar,
                        CreatedDate = DateTime.UtcNow,
                        Name = Name,
                        Status = (int)MixChatEnums.OnlineStatus.Connected
                    };
                    if (_context.MixMessengerUser.Any(u => u.Id == user.Id))
                    {
                        _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                    else
                    {
                        _context.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    }
                    if (Device != null)
                    {
                        //var cnn = _context.MixMessengerUserDevice.FirstOrDefault(c => c.UserId == Device.UserId && c.DeviceId == Device.DeviceId);
                        if (_context.MixMessengerUserDevice.Any(c => c.UserId == Device.UserId && c.DeviceId == Device.DeviceId))
                        {
                            Device.ConnectionId = Device.ConnectionId;
                            Device.Status = (int)MixChatEnums.DeviceStatus.Actived;
                            Device.StartDate = DateTime.UtcNow;
                            _context.Entry(Device).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                        }
                        else
                        {
                            Device.Status = (int)MixChatEnums.DeviceStatus.Actived;
                            Device.StartDate = DateTime.UtcNow;
                            _context.Entry(Device).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                        }
                    }
                    result.IsSucceed = (await _context.SaveChangesAsync()) > 0;
                }
                catch (Exception ex)
                {
                    result.IsSucceed = false;
                    result.Exception = ex;
                }
                return result;
            }
        }

        #endregion Async

        #endregion Override
    }
}