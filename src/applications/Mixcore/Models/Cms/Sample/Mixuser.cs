using System;
using System.Collections.Generic;

namespace Mixcore.Models.Cms.Sample;

public partial class Mixuser
{
    public Guid Id { get; set; }

    public DateTime CreatedDateTime { get; set; }

    public bool IsActived { get; set; }

    public DateTime? LastModified { get; set; }

    public string ModifiedBy { get; set; }

    public string RegisterType { get; set; }

    public DateTime? LockoutEnd { get; set; }

    public string UserName { get; set; }

    public string NormalizedUserName { get; set; }

    public string Email { get; set; }

    public string NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string PasswordHash { get; set; }

    public string SecurityStamp { get; set; }

    public string ConcurrencyStamp { get; set; }

    public string PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public virtual ICollection<Aspnetuserclaim> AspnetuserclaimMixUserId1Navigations { get; } = new List<Aspnetuserclaim>();

    public virtual ICollection<Aspnetuserclaim> AspnetuserclaimMixUsers { get; } = new List<Aspnetuserclaim>();

    public virtual ICollection<Aspnetuserlogin> AspnetuserloginMixUserId1Navigations { get; } = new List<Aspnetuserlogin>();

    public virtual ICollection<Aspnetuserlogin> AspnetuserloginMixUsers { get; } = new List<Aspnetuserlogin>();

    public virtual ICollection<Aspnetuserrole> AspnetuserroleMixUserId1Navigations { get; } = new List<Aspnetuserrole>();

    public virtual ICollection<Aspnetuserrole> AspnetuserroleMixUsers { get; } = new List<Aspnetuserrole>();

    public virtual ICollection<Aspnetusertoken> Aspnetusertokens { get; } = new List<Aspnetusertoken>();
}
