using Microsoft.Extensions.DependencyInjection;
using Mix.Database.Entities.Account;
using Mix.Identity.Interfaces;
using Mix.Identity.ViewModels;
using System;
using System.Collections.Generic;

namespace Mix.Identity.Services
{
    public sealed class OAuthClientService : IOAuthClientService
    {
        private readonly IServiceProvider _serviceProvider;

        public List<OAuthClient> Clients { get; set; }
        public OAuthClientService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            LoadClients();
        }

        public List<OAuthClient> LoadClients(bool isReload = false)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var accContext = scope.ServiceProvider.GetService<MixCmsAccountContext>();
                if (isReload || Clients == null)
                {
                    Clients = [.. accContext.OAuthClient];
                }
            }

            return Clients;
        }

        public List<OAuthClient> AddClients(OAuthClientViewModel data)
        {
            Clients ??= [];

            var client = new OAuthClient
            {
                CreatedDateTime = data.CreatedDateTime,
                AllowedOrigins = data.AllowedOrigins,
                AllowedProtectedResources = data.AllowedProtectedResources,
                AllowedScopes = data.AllowedScopes,
                ApplicationType = data.ApplicationType,
                ClientUri = data.ClientUri,
                CreatedBy = data.CreatedBy,
                GrantTypes = data.GrantTypes,
                Id = data.Id,
                IsActive = data.IsActive,
                IsDeleted = data.IsDeleted,
                LastModified = data.LastModified,
                ModifiedBy = data.ModifiedBy,
                Name = data.Name,
                Priority = data.Priority,
                RedirectUris = data.RedirectUris,
                RefreshTokenLifeTime = data.RefreshTokenLifeTime,
                Secret = data.Secret,
                Status = data.Status,
                UsePkce = data.UsePkce
            };

            Clients.Add(client);
            return Clients;
        }
    }
}
