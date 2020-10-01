using System;
using System.Collections.Generic;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class Aspnetusers
    {
        public Aspnetusers()
        {
            AspnetuserclaimsApplicationUser = new HashSet<Aspnetuserclaims>();
            AspnetuserclaimsUser = new HashSet<Aspnetuserclaims>();
            AspnetuserloginsApplicationUser = new HashSet<Aspnetuserlogins>();
            AspnetuserloginsUser = new HashSet<Aspnetuserlogins>();
            AspnetuserrolesApplicationUser = new HashSet<Aspnetuserroles>();
            AspnetuserrolesUser = new HashSet<Aspnetuserroles>();
            Aspnetusertokens = new HashSet<Aspnetusertokens>();
        }

        public string Id { get; set; }
        public int AccessFailedCount { get; set; }
        public string Avatar { get; set; }
        public string ConcurrencyStamp { get; set; }
        public int CountryId { get; set; }
        public string Culture { get; set; }
        public DateTime? Dob { get; set; }
        public string Email { get; set; }
        public ulong EmailConfirmed { get; set; }
        public string FirstName { get; set; }
        public string Gender { get; set; }
        public ulong IsActived { get; set; }
        public DateTime JoinDate { get; set; }
        public DateTime LastModified { get; set; }
        public string LastName { get; set; }
        public ulong LockoutEnabled { get; set; }
        public DateTime? LockoutEnd { get; set; }
        public string ModifiedBy { get; set; }
        public string NickName { get; set; }
        public string NormalizedEmail { get; set; }
        public string NormalizedUserName { get; set; }
        public string PasswordHash { get; set; }
        public string PhoneNumber { get; set; }
        public ulong PhoneNumberConfirmed { get; set; }
        public string RegisterType { get; set; }
        public string SecurityStamp { get; set; }
        public ulong TwoFactorEnabled { get; set; }
        public string UserName { get; set; }

        public virtual ICollection<Aspnetuserclaims> AspnetuserclaimsApplicationUser { get; set; }
        public virtual ICollection<Aspnetuserclaims> AspnetuserclaimsUser { get; set; }
        public virtual ICollection<Aspnetuserlogins> AspnetuserloginsApplicationUser { get; set; }
        public virtual ICollection<Aspnetuserlogins> AspnetuserloginsUser { get; set; }
        public virtual ICollection<Aspnetuserroles> AspnetuserrolesApplicationUser { get; set; }
        public virtual ICollection<Aspnetuserroles> AspnetuserrolesUser { get; set; }
        public virtual ICollection<Aspnetusertokens> Aspnetusertokens { get; set; }
    }
}
