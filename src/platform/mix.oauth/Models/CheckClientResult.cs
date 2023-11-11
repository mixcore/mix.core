/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

namespace Mix.OAuth.Models
{
    public class CheckClientResult
    {
        public Client Client { get; set; }

        /// <summary>
        /// The clinet is found in my Clients Store
        /// </summary>
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }

        public string? ErrorDescription { get; set; }
    }
}
