// Licensed to the Mixcore Foundation under one or more agreements.
// The Mixcore Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Mix.Identity
{
    public class Const
    {
        /// <summary>
        /// The constant default connection
        /// </summary>
        public const string CONST_DEFAULT_CONNECTION = "AccountConnection";

        /// <summary>
        /// The constant file appsetting
        /// </summary>
        public const string CONST_FILE_APPSETTING = "appsettings.json";

        public enum ApplicationTypes
        {
            JavaScript = 0,
            NativeConfidential = 1
        };
    }
}