using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Shared.Extensions
{
    public static class HostEnvironmentEnvExtensions
    {
        /// <summary>
        /// Checks if the current host environment name is <see cref="Environments.Development"/>.
        /// </summary>
        /// <param name="hostEnvironment">An instance of <see cref="IHostEnvironment"/>.</param>
        /// <returns>True if the environment name is <see cref="Environments.Development"/>, otherwise false.</returns>
        public static bool IsLocal(this IHostEnvironment hostEnvironment)
        {
            if (hostEnvironment is null)
            {
                throw new NullReferenceException();
            }

            return hostEnvironment.IsEnvironment(MixEnvironments.Local);
        }
    }
}
