﻿using Mix.Lib.Interfaces;
using Mix.Lib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Lib.Subscribers.Handlers.MixViewModelChangedHandlers
{
    public class MixTenantSystemViewModelHandler
    {
        public static Task MessageQueueHandler(MessageQueueModel data, IMixTenantService mixTenantService)
        {
            switch (data.Action)
            {
                case "Get":
                case "Patch":
                    break;
                case "Post":
                case "Put":
                    break;
                case "Delete":
                    return mixTenantService.Reload();
                default:
                    break;
            }
            return Task.CompletedTask;
        }
    }
}