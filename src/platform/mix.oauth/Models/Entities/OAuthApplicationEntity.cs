/*
                        GNU GENERAL PUBLIC LICENSE
                          Version 3, 29 June 2007
 Copyright (C) 2022 Mohammed Ahmed Hussien babiker Free Software Foundation, Inc. <https://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.
 */

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mix.OAuth.Models.Entities
{
    [Table("OAuthApplications", Schema = "OAuth")]
    public class OAuthApplicationEntity
    {
        [Key]
        public int ApplicationId { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        //  public string[] AllowedScopes { get; set; }
        public string ClientUri { get; set; }
        public string RedirectUris { get; set; }
        public string ClientName { get; set; }
        public bool IsActive { get; set; }
    }
}
