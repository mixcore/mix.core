/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using Mix.OAuth.Models;

namespace Mix.OAuth.Validations.Response
{
    public class TokenRevocationValidationResponse : BaseValidationResponse
    {
        public Client Client { get; set; }
    }
}
