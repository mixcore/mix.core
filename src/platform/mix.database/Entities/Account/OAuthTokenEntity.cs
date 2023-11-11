/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mix.Database.Entities.Account
{
    public class OAuthTokenEntity
    {
        [Key]
        public long Id { get; set; }
        public string Token { get; set; }
        public string ClientId { get; set; }

        /// <summary>
        /// This is a user Id
        /// </summary>
        public string SubjectId { get; set; }

        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }

        /// <summary>
        /// I will use snakflowId here
        /// <see cref="https://github.com/Shoogn/SnowflakeId"/> for more details
        /// </summary>
        public string ReferenceId { get; set; }

        public string TokenType { get; set; }
        public string TokenTypeHint { get; set; }
        public string Status { get; set; }

        /// <summary>
        /// A flag to indicate if the token is revoked
        /// </summary>
        public bool Revoked { get; set; }
    }
}
