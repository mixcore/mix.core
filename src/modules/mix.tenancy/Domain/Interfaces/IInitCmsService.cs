﻿using Mix.Auth.Models;
using Mix.Tenancy.Domain.Dtos;

namespace Mix.Tenancy.Domain.Interfaces
{
    public interface IInitCmsService
    {
        public Task InitDbContext(InitCmsDto model);

        public Task InitTenantAsync(InitCmsDto model);

        public Task<TokenResponseModel> InitAccountAsync(RegisterRequestModel model);
    }
}
