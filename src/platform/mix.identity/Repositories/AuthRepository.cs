// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mix.Database.Entities.Account;
using System;
using System.Collections.Generic;

namespace Mix.Identity.Repositories
{
    public class AuthRepository : UserManager<MixUser>
    {
        public AuthRepository(IUserStore<MixUser> store, IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<MixUser> passwordHasher, IEnumerable<IUserValidator<MixUser>> userValidators,
            IEnumerable<IPasswordValidator<MixUser>> passwordValidators, ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<MixUser>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}