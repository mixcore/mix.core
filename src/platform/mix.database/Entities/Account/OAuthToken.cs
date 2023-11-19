using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mix.Database.Entities.Account
{
    public class OAuthToken: Entity
    {
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
