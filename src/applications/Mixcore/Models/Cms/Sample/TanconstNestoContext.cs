using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Mixcore.Models.Cms.Sample;

public partial class TanconstNestoContext : DbContext
{
    public TanconstNestoContext()
    {
    }

    public TanconstNestoContext(DbContextOptions<TanconstNestoContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Aspnetroleclaim> Aspnetroleclaims { get; set; }

    public virtual DbSet<Aspnetuserclaim> Aspnetuserclaims { get; set; }

    public virtual DbSet<Aspnetuserlogin> Aspnetuserlogins { get; set; }

    public virtual DbSet<Aspnetuserrole> Aspnetuserroles { get; set; }

    public virtual DbSet<Aspnetusertoken> Aspnetusertokens { get; set; }

    public virtual DbSet<Client> Clients { get; set; }

    public virtual DbSet<Contactaddress> Contactaddresses { get; set; }

    public virtual DbSet<Efmigrationshistory> Efmigrationshistories { get; set; }

    public virtual DbSet<Metadata> Metadata { get; set; }

    public virtual DbSet<Metadatacontent> Metadatacontents { get; set; }

    public virtual DbSet<Metadatacontentassociation> Metadatacontentassociations { get; set; }

    public virtual DbSet<Mixapplication> Mixapplications { get; set; }

    public virtual DbSet<Mixcache> Mixcaches { get; set; }

    public virtual DbSet<Mixconfiguration> Mixconfigurations { get; set; }

    public virtual DbSet<Mixconfigurationcontent> Mixconfigurationcontents { get; set; }

    public virtual DbSet<Mixcontributor> Mixcontributors { get; set; }

    public virtual DbSet<Mixcorecmshelper> Mixcorecmshelpers { get; set; }

    public virtual DbSet<Mixculture> Mixcultures { get; set; }

    public virtual DbSet<Mixdatabase> Mixdatabases { get; set; }

    public virtual DbSet<Mixdatabaseassociation> Mixdatabaseassociations { get; set; }

    public virtual DbSet<Mixdatabasecolumn> Mixdatabasecolumns { get; set; }

    public virtual DbSet<Mixdatabasecontext> Mixdatabasecontexts { get; set; }

    public virtual DbSet<Mixdatabasecontextdatabaseassociation> Mixdatabasecontextdatabaseassociations { get; set; }

    public virtual DbSet<Mixdatabaserelationship> Mixdatabaserelationships { get; set; }

    public virtual DbSet<Mixdatacontent> Mixdatacontents { get; set; }

    public virtual DbSet<Mixdatacontentassociation> Mixdatacontentassociations { get; set; }

    public virtual DbSet<Mixdatacontentvalue> Mixdatacontentvalues { get; set; }

    public virtual DbSet<Mixdatum> Mixdata { get; set; }

    public virtual DbSet<Mixdiscussion> Mixdiscussions { get; set; }

    public virtual DbSet<Mixdomain> Mixdomains { get; set; }

    public virtual DbSet<Mixlanguage> Mixlanguages { get; set; }

    public virtual DbSet<Mixlanguagecontent> Mixlanguagecontents { get; set; }

    public virtual DbSet<Mixmedium> Mixmedia { get; set; }

    public virtual DbSet<Mixmodule> Mixmodules { get; set; }

    public virtual DbSet<Mixmodulecontent> Mixmodulecontents { get; set; }

    public virtual DbSet<Mixmoduledatum> Mixmoduledata { get; set; }

    public virtual DbSet<Mixmodulepostassociation> Mixmodulepostassociations { get; set; }

    public virtual DbSet<Mixpage> Mixpages { get; set; }

    public virtual DbSet<Mixpagecontent> Mixpagecontents { get; set; }

    public virtual DbSet<Mixpagemoduleassociation> Mixpagemoduleassociations { get; set; }

    public virtual DbSet<Mixpagepostassociation> Mixpagepostassociations { get; set; }

    public virtual DbSet<Mixpost> Mixposts { get; set; }

    public virtual DbSet<Mixpostcontent> Mixpostcontents { get; set; }

    public virtual DbSet<Mixpostpostassociation> Mixpostpostassociations { get; set; }

    public virtual DbSet<Mixrole> Mixroles { get; set; }

    public virtual DbSet<Mixtenant> Mixtenants { get; set; }

    public virtual DbSet<Mixtheme> Mixthemes { get; set; }

    public virtual DbSet<Mixurlalias> Mixurlaliases { get; set; }

    public virtual DbSet<Mixuser> Mixusers { get; set; }

    public virtual DbSet<Mixusertenant> Mixusertenants { get; set; }

    public virtual DbSet<Mixviewtemplate> Mixviewtemplates { get; set; }

    public virtual DbSet<Onepaytransactionrequest> Onepaytransactionrequests { get; set; }

    public virtual DbSet<Onepaytransactionresponse> Onepaytransactionresponses { get; set; }

    public virtual DbSet<Orderdetail> Orderdetails { get; set; }

    public virtual DbSet<Orderitem> Orderitems { get; set; }

    public virtual DbSet<Productdetail> Productdetails { get; set; }

    public virtual DbSet<QrtzBlobTrigger> QrtzBlobTriggers { get; set; }

    public virtual DbSet<QrtzCalendar> QrtzCalendars { get; set; }

    public virtual DbSet<QrtzCronTrigger> QrtzCronTriggers { get; set; }

    public virtual DbSet<QrtzFiredTrigger> QrtzFiredTriggers { get; set; }

    public virtual DbSet<QrtzJobDetail> QrtzJobDetails { get; set; }

    public virtual DbSet<QrtzLock> QrtzLocks { get; set; }

    public virtual DbSet<QrtzPausedTriggerGrp> QrtzPausedTriggerGrps { get; set; }

    public virtual DbSet<QrtzSchedulerState> QrtzSchedulerStates { get; set; }

    public virtual DbSet<QrtzSimpleTrigger> QrtzSimpleTriggers { get; set; }

    public virtual DbSet<QrtzSimpropTrigger> QrtzSimpropTriggers { get; set; }

    public virtual DbSet<QrtzTrigger> QrtzTriggers { get; set; }

    public virtual DbSet<Refreshtoken> Refreshtokens { get; set; }

    public virtual DbSet<Shoppingcart> Shoppingcarts { get; set; }

    public virtual DbSet<Syscategory> Syscategories { get; set; }

    public virtual DbSet<Sysmedium> Sysmedia { get; set; }

    public virtual DbSet<Sysmenuitem> Sysmenuitems { get; set; }

    public virtual DbSet<Sysnavigation> Sysnavigations { get; set; }

    public virtual DbSet<Syspermission> Syspermissions { get; set; }

    public virtual DbSet<Syspermissionendpoint> Syspermissionendpoints { get; set; }

    public virtual DbSet<Systag> Systags { get; set; }

    public virtual DbSet<Sysuserdatum> Sysuserdata { get; set; }

    public virtual DbSet<Sysuserpermission> Sysuserpermissions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=tanconst_nesto;user=root;password=1234qwe@", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.31-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Aspnetroleclaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("aspnetroleclaims");

            entity.HasIndex(e => e.MixRoleId, "IX_AspNetRoleClaims_MixRoleId");

            entity.HasIndex(e => e.RoleId, "IX_AspNetRoleClaims_RoleId");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ClaimType)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ClaimValue)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.MixRoleId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("uuid_to_bin(uuid())")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");

            entity.HasOne(d => d.MixRole).WithMany(p => p.Aspnetroleclaims)
                .HasForeignKey(d => d.MixRoleId)
                .HasConstraintName("FK_AspNetRoleClaims_MixRoles_MixRoleId");
        });

        modelBuilder.Entity<Aspnetuserclaim>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("aspnetuserclaims");

            entity.HasIndex(e => e.MixUserId, "IX_AspNetUserClaims_MixUserId");

            entity.HasIndex(e => e.MixUserId1, "IX_AspNetUserClaims_MixUserId1");

            entity.HasIndex(e => e.UserId, "IX_AspNetUserClaims_UserId");

            entity.Property(e => e.ClaimType)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ClaimValue)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.MixUserId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MixUserId1)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.UserId)
                .HasDefaultValueSql("uuid_to_bin(uuid())")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");

            entity.HasOne(d => d.MixUser).WithMany(p => p.AspnetuserclaimMixUsers)
                .HasForeignKey(d => d.MixUserId)
                .HasConstraintName("FK_AspNetUserClaims_MixUsers_MixUserId");

            entity.HasOne(d => d.MixUserId1Navigation).WithMany(p => p.AspnetuserclaimMixUserId1Navigations)
                .HasForeignKey(d => d.MixUserId1)
                .HasConstraintName("FK_AspNetUserClaims_MixUsers_MixUserId1");
        });

        modelBuilder.Entity<Aspnetuserlogin>(entity =>
        {
            entity.HasKey(e => new { e.LoginProvider, e.ProviderKey })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("aspnetuserlogins");

            entity.HasIndex(e => e.MixUserId, "IX_AspNetUserLogins_MixUserId");

            entity.HasIndex(e => e.MixUserId1, "IX_AspNetUserLogins_MixUserId1");

            entity.HasIndex(e => e.UserId, "IX_AspNetUserLogins_UserId");

            entity.Property(e => e.LoginProvider)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ProviderKey)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.MixUserId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MixUserId1)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ProviderDisplayName)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UserId)
                .HasDefaultValueSql("uuid_to_bin(uuid())")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");

            entity.HasOne(d => d.MixUser).WithMany(p => p.AspnetuserloginMixUsers)
                .HasForeignKey(d => d.MixUserId)
                .HasConstraintName("FK_AspNetUserLogins_MixUsers_MixUserId");

            entity.HasOne(d => d.MixUserId1Navigation).WithMany(p => p.AspnetuserloginMixUserId1Navigations)
                .HasForeignKey(d => d.MixUserId1)
                .HasConstraintName("FK_AspNetUserLogins_MixUsers_MixUserId1");
        });

        modelBuilder.Entity<Aspnetuserrole>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.RoleId, e.MixTenantId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("aspnetuserroles");

            entity.HasIndex(e => e.MixRoleId, "IX_AspNetUserRoles_MixRoleId");

            entity.HasIndex(e => e.MixUserId, "IX_AspNetUserRoles_MixUserId");

            entity.HasIndex(e => e.MixUserId1, "IX_AspNetUserRoles_MixUserId1");

            entity.HasIndex(e => e.RoleId, "IX_AspNetUserRoles_RoleId");

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("uuid_to_bin(uuid())")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.RoleId)
                .HasDefaultValueSql("uuid_to_bin(uuid())")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MixRoleId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MixUserId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MixUserId1)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");

            entity.HasOne(d => d.MixRole).WithMany(p => p.Aspnetuserroles)
                .HasForeignKey(d => d.MixRoleId)
                .HasConstraintName("FK_AspNetUserRoles_MixRoles_MixRoleId");

            entity.HasOne(d => d.MixUser).WithMany(p => p.AspnetuserroleMixUsers)
                .HasForeignKey(d => d.MixUserId)
                .HasConstraintName("FK_AspNetUserRoles_MixUsers_MixUserId");

            entity.HasOne(d => d.MixUserId1Navigation).WithMany(p => p.AspnetuserroleMixUserId1Navigations)
                .HasForeignKey(d => d.MixUserId1)
                .HasConstraintName("FK_AspNetUserRoles_MixUsers_MixUserId1");
        });

        modelBuilder.Entity<Aspnetusertoken>(entity =>
        {
            entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("aspnetusertokens");

            entity.HasIndex(e => e.MixUserId, "IX_AspNetUserTokens_MixUserId");

            entity.Property(e => e.UserId)
                .HasDefaultValueSql("uuid_to_bin(uuid())")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.LoginProvider)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.MixUserId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.Value)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixUser).WithMany(p => p.Aspnetusertokens)
                .HasForeignKey(d => d.MixUserId)
                .HasConstraintName("FK_AspNetUserTokens_MixUsers_MixUserId");
        });

        modelBuilder.Entity<Client>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clients");
        });

        modelBuilder.Entity<Contactaddress>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("contactaddress");

            entity.Property(e => e.City).HasMaxLength(250);
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.District).HasMaxLength(250);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Note).HasMaxLength(250);
            entity.Property(e => e.Province).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.Street).HasMaxLength(250);
        });

        modelBuilder.Entity<Efmigrationshistory>(entity =>
        {
            entity.HasKey(e => e.MigrationId).HasName("PRIMARY");

            entity.ToTable("__efmigrationshistory");

            entity.Property(e => e.MigrationId).HasMaxLength(150);
            entity.Property(e => e.ProductVersion)
                .IsRequired()
                .HasMaxLength(32);
        });

        modelBuilder.Entity<Metadata>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("metadata");

            entity.Property(e => e.Content).HasMaxLength(250);
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.SeoContent).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.Type).HasMaxLength(250);
        });

        modelBuilder.Entity<Metadatacontent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("metadatacontent");

            entity.Property(e => e.ContentType).HasMaxLength(250);
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.Image).HasMaxLength(250);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
        });

        modelBuilder.Entity<Metadatacontentassociation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("metadatacontentassociation");

            entity.Property(e => e.ContentType).HasMaxLength(250);
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.Image).HasMaxLength(250);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
        });

        modelBuilder.Entity<Mixapplication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixapplication");

            entity.HasIndex(e => e.MixDataContentId, "IX_MixApplication_MixDataContentId");

            entity.HasIndex(e => e.MixTenantId, "IX_MixApplication_MixTenantId");

            entity.Property(e => e.BaseApiUrl)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.BaseHref)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.BaseRoute)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Domain)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.MixDataContentId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MixDatabaseName)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixDataContent).WithMany(p => p.Mixapplications)
                .HasForeignKey(d => d.MixDataContentId)
                .HasConstraintName("FK_MixApplication_MixDataContent_MixDataContentId");

            entity.HasOne(d => d.MixTenant).WithMany(p => p.Mixapplications)
                .HasForeignKey(d => d.MixTenantId)
                .HasConstraintName("FK_MixApplication_MixTenant_MixTenantId");
        });

        modelBuilder.Entity<Mixcache>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixcache");

            entity.HasIndex(e => e.ExpiredDateTime, "Index_ExpiresAtTime");

            entity.Property(e => e.CreatedBy).HasMaxLength(50);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.ExpiredDateTime).HasColumnType("datetime");
            entity.Property(e => e.Keyword)
                .IsRequired()
                .HasMaxLength(400);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(50);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50);
            entity.Property(e => e.Value)
                .IsRequired()
                .HasColumnType("text");
        });

        modelBuilder.Entity<Mixconfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixconfiguration");

            entity.HasIndex(e => e.MixTenantId, "IX_MixConfiguration_MixTenantId");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SystemName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixTenant).WithMany(p => p.Mixconfigurations)
                .HasForeignKey(d => d.MixTenantId)
                .HasConstraintName("FK_MixConfiguration_MixTenant_MixTenantId");
        });

        modelBuilder.Entity<Mixconfigurationcontent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixconfigurationcontent");

            entity.HasIndex(e => e.MixConfigurationId, "IX_MixConfigurationContent_MixConfigurationId");

            entity.HasIndex(e => e.MixCultureId, "IX_MixConfigurationContent_MixCultureId");

            entity.Property(e => e.Category)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Content)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.DataType)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DefaultContent)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Description)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Specificulture)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SystemName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixConfiguration).WithMany(p => p.Mixconfigurationcontents)
                .HasForeignKey(d => d.MixConfigurationId)
                .HasConstraintName("FK_MixConfigurationContent_MixConfiguration_MixConfigurationId");

            entity.HasOne(d => d.MixCulture).WithMany(p => p.Mixconfigurationcontents)
                .HasForeignKey(d => d.MixCultureId)
                .HasConstraintName("FK_MixConfigurationContent_MixCulture_MixCultureId");
        });

        modelBuilder.Entity<Mixcontributor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixcontributor");

            entity.Property(e => e.ContentType)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.GuidContentId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UserId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
        });

        modelBuilder.Entity<Mixcorecmshelper>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixcorecmshelper");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        modelBuilder.Entity<Mixculture>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixculture");

            entity.HasIndex(e => e.MixTenantId, "IX_MixCulture_MixTenantId");

            entity.Property(e => e.Alias)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Icon)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.Lcid)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Specificulture)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixTenant).WithMany(p => p.Mixcultures)
                .HasForeignKey(d => d.MixTenantId)
                .HasConstraintName("FK_MixCulture_MixTenant_MixTenantId");
        });

        modelBuilder.Entity<Mixdatabase>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixdatabase");

            entity.HasIndex(e => e.MixDatabaseContextDatabaseAssociationId, "IX_MixDatabase_MixDatabaseContextDatabaseAssociationId");

            entity.HasIndex(e => e.MixTenantId, "IX_MixDatabase_MixTenantId");

            entity.Property(e => e.CreatePermissions)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.DeletePermissions)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Description)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.ReadPermissions)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SystemName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UpdatePermissions)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixDatabaseContextDatabaseAssociation).WithMany(p => p.Mixdatabases)
                .HasForeignKey(d => d.MixDatabaseContextDatabaseAssociationId)
                .HasConstraintName("FK_MixDatabase_MixDatabaseContextDatabaseAssociation_MixDatabas~");

            entity.HasOne(d => d.MixTenant).WithMany(p => p.Mixdatabases)
                .HasForeignKey(d => d.MixTenantId)
                .HasConstraintName("FK_MixDatabase_MixTenant_MixTenantId");
        });

        modelBuilder.Entity<Mixdatabaseassociation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixdatabaseassociation");

            entity.Property(e => e.Id)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ChildDatabaseName)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.ParentDatabaseName)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Mixdatabasecolumn>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixdatabasecolumn");

            entity.HasIndex(e => e.MixDatabaseId, "IX_MixDatabaseColumn_MixDatabaseId");

            entity.Property(e => e.Configurations)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.DataType)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DefaultValue)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.MixDatabaseName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SystemName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixDatabase).WithMany(p => p.Mixdatabasecolumns)
                .HasForeignKey(d => d.MixDatabaseId)
                .HasConstraintName("FK_MixDatabaseColumn_MixDatabase_MixDatabaseId");
        });

        modelBuilder.Entity<Mixdatabasecontext>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixdatabasecontext");

            entity.HasIndex(e => e.MixDatabaseContextDatabaseAssociationId, "IX_MixDatabaseContext_MixDatabaseContextDatabaseAssociationId");

            entity.HasIndex(e => e.MixTenantId, "IX_MixDatabaseContext_MixTenantId");

            entity.Property(e => e.ConnectionString)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.DatabaseProvider)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Description)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SystemName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixDatabaseContextDatabaseAssociation).WithMany(p => p.Mixdatabasecontexts)
                .HasForeignKey(d => d.MixDatabaseContextDatabaseAssociationId)
                .HasConstraintName("FK_MixDatabaseContext_MixDatabaseContextDatabaseAssociation_Mix~");

            entity.HasOne(d => d.MixTenant).WithMany(p => p.Mixdatabasecontexts)
                .HasForeignKey(d => d.MixTenantId)
                .HasConstraintName("FK_MixDatabaseContext_MixTenant_MixTenantId");
        });

        modelBuilder.Entity<Mixdatabasecontextdatabaseassociation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixdatabasecontextdatabaseassociation");

            entity.Property(e => e.CreatedDateTime).HasMaxLength(6);
            entity.Property(e => e.LastModified).HasMaxLength(6);
        });

        modelBuilder.Entity<Mixdatabaserelationship>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixdatabaserelationship");

            entity.HasIndex(e => e.ChildId, "IX_MixDatabaseRelationship_ChildId");

            entity.HasIndex(e => e.ParentId, "IX_MixDatabaseRelationship_ParentId");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.DestinateDatabaseName)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.SourceDatabaseName)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Mixdatacontent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixdatacontent");

            entity.HasIndex(e => e.MixCultureId, "IX_MixDataContent_MixCultureId");

            entity.HasIndex(e => e.MixDataId, "IX_MixDataContent_MixDataId");

            entity.Property(e => e.Id)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.Content)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Excerpt)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.MixDataId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.ParentId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.PublishedDateTime).HasColumnType("datetime");
            entity.Property(e => e.SeoDescription)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SeoKeywords)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SeoName)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Source)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Specificulture)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixCulture).WithMany(p => p.Mixdatacontents)
                .HasForeignKey(d => d.MixCultureId)
                .HasConstraintName("FK_MixDataContent_MixCulture_MixCultureId");

            entity.HasOne(d => d.MixData).WithMany(p => p.Mixdatacontents)
                .HasForeignKey(d => d.MixDataId)
                .HasConstraintName("FK_MixDataContent_MixData_MixDataId");
        });

        modelBuilder.Entity<Mixdatacontentassociation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixdatacontentassociation");

            entity.HasIndex(e => e.MixCultureId, "IX_MixDataContentAssociation_MixCultureId");

            entity.Property(e => e.Id)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.DataContentId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.GuidParentId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.ParentId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ParentType)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixCulture).WithMany(p => p.Mixdatacontentassociations)
                .HasForeignKey(d => d.MixCultureId)
                .HasConstraintName("FK_MixDataContentAssociation_MixCulture_MixCultureId");
        });

        modelBuilder.Entity<Mixdatacontentvalue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixdatacontentvalue");

            entity.HasIndex(e => e.MixCultureId, "IX_MixDataContentValue_MixCultureId");

            entity.HasIndex(e => e.MixDataContentId, "IX_MixDataContentValue_MixDataContentId");

            entity.HasIndex(e => e.MixDatabaseColumnId, "IX_MixDataContentValue_MixDatabaseColumnId");

            entity.Property(e => e.Id)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.DataType)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DateTimeValue).HasColumnType("datetime");
            entity.Property(e => e.EncryptKey)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.EncryptType)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.EncryptValue)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.MixDataContentId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.MixDatabaseColumnName)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.MixDatabaseName)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.ParentId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.StringValue)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixCulture).WithMany(p => p.Mixdatacontentvalues)
                .HasForeignKey(d => d.MixCultureId)
                .HasConstraintName("FK_MixDataContentValue_MixCulture_MixCultureId");

            entity.HasOne(d => d.MixDataContent).WithMany(p => p.Mixdatacontentvalues)
                .HasForeignKey(d => d.MixDataContentId)
                .HasConstraintName("FK_MixDataContentValue_MixDataContent_MixDataContentId");

            entity.HasOne(d => d.MixDatabaseColumn).WithMany(p => p.Mixdatacontentvalues)
                .HasForeignKey(d => d.MixDatabaseColumnId)
                .HasConstraintName("FK_MixDataContentValue_MixDatabaseColumn_MixDatabaseColumnId");
        });

        modelBuilder.Entity<Mixdatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixdata");

            entity.HasIndex(e => e.MixDatabaseId, "IX_MixData_MixDatabaseId");

            entity.Property(e => e.Id)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.MixDatabaseName)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixDatabase).WithMany(p => p.Mixdata)
                .HasForeignKey(d => d.MixDatabaseId)
                .HasConstraintName("FK_MixData_MixDatabase_MixDatabaseId");
        });

        modelBuilder.Entity<Mixdiscussion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixdiscussion");

            entity.Property(e => e.Content)
                .IsRequired()
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ContentType)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.GuidContentId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UserId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
        });

        modelBuilder.Entity<Mixdomain>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixdomain");

            entity.HasIndex(e => e.MixTenantId, "IX_MixDomain_MixTenantId");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Host)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixTenant).WithMany(p => p.Mixdomains)
                .HasForeignKey(d => d.MixTenantId)
                .HasConstraintName("FK_MixDomain_MixTenant_MixTenantId");
        });

        modelBuilder.Entity<Mixlanguage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixlanguage");

            entity.HasIndex(e => e.MixTenantId, "IX_MixLanguage_MixTenantId");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SystemName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixTenant).WithMany(p => p.Mixlanguages)
                .HasForeignKey(d => d.MixTenantId)
                .HasConstraintName("FK_MixLanguage_MixTenant_MixTenantId");
        });

        modelBuilder.Entity<Mixlanguagecontent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixlanguagecontent");

            entity.HasIndex(e => e.MixCultureId, "IX_MixLanguageContent_MixCultureId");

            entity.HasIndex(e => e.MixLanguageId, "IX_MixLanguageContent_MixLanguageId");

            entity.Property(e => e.Category)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Content)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.DataType)
                .IsRequired()
                .HasMaxLength(50)
                .HasDefaultValueSql("''")
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DefaultContent)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Description)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Specificulture)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SystemName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixCulture).WithMany(p => p.Mixlanguagecontents)
                .HasForeignKey(d => d.MixCultureId)
                .HasConstraintName("FK_MixLanguageContent_MixCulture_MixCultureId");

            entity.HasOne(d => d.MixLanguage).WithMany(p => p.Mixlanguagecontents)
                .HasForeignKey(d => d.MixLanguageId)
                .HasConstraintName("FK_MixLanguageContent_MixLanguage_MixLanguageId");
        });

        modelBuilder.Entity<Mixmedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixmedia");

            entity.HasIndex(e => e.MixTenantId, "IX_MixMedia_MixTenantId");

            entity.Property(e => e.Id)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Extension)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.FileFolder)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.FileName)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.FileProperties)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.FileType)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Source)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Tags)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.TargetUrl)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixTenant).WithMany(p => p.Mixmedia)
                .HasForeignKey(d => d.MixTenantId)
                .HasConstraintName("FK_MixMedia_MixTenant_MixTenantId");
        });

        modelBuilder.Entity<Mixmodule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixmodule");

            entity.HasIndex(e => e.MixTenantId, "IX_MixModule_MixTenantId");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SystemName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixTenant).WithMany(p => p.Mixmodules)
                .HasForeignKey(d => d.MixTenantId)
                .HasConstraintName("FK_MixModule_MixTenant_MixTenantId");
        });

        modelBuilder.Entity<Mixmodulecontent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixmodulecontent");

            entity.HasIndex(e => e.MixCultureId, "IX_MixModuleContent_MixCultureId");

            entity.HasIndex(e => e.MixDataContentId, "IX_MixModuleContent_MixDataContentId");

            entity.HasIndex(e => e.MixModuleId, "IX_MixModuleContent_MixModuleId");

            entity.Property(e => e.ClassName)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Content)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Excerpt)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.MixDataContentId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.PublishedDateTime).HasColumnType("datetime");
            entity.Property(e => e.SeoDescription)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SeoKeywords)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SeoName)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Source)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Specificulture)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SystemName)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixCulture).WithMany(p => p.Mixmodulecontents)
                .HasForeignKey(d => d.MixCultureId)
                .HasConstraintName("FK_MixModuleContent_MixCulture_MixCultureId");

            entity.HasOne(d => d.MixDataContent).WithMany(p => p.Mixmodulecontents)
                .HasForeignKey(d => d.MixDataContentId)
                .HasConstraintName("FK_MixModuleContent_MixDataContent_MixDataContentId");

            entity.HasOne(d => d.MixModule).WithMany(p => p.Mixmodulecontents)
                .HasForeignKey(d => d.MixModuleId)
                .HasConstraintName("FK_MixModuleContent_MixModule_MixModuleId");
        });

        modelBuilder.Entity<Mixmoduledatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixmoduledata");

            entity.HasIndex(e => e.MixCultureId, "IX_MixModuleData_MixCultureId");

            entity.HasIndex(e => e.MixModuleContentId, "IX_MixModuleData_MixModuleContentId");

            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Excerpt)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.PublishedDateTime).HasColumnType("datetime");
            entity.Property(e => e.SeoDescription)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SeoKeywords)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SeoName)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SimpleDataColumns)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Source)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Specificulture)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Value)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixCulture).WithMany(p => p.Mixmoduledata)
                .HasForeignKey(d => d.MixCultureId)
                .HasConstraintName("FK_MixModuleData_MixCulture_MixCultureId");

            entity.HasOne(d => d.MixModuleContent).WithMany(p => p.Mixmoduledata)
                .HasForeignKey(d => d.MixModuleContentId)
                .HasConstraintName("FK_MixModuleData_MixModuleContent_MixModuleContentId");
        });

        modelBuilder.Entity<Mixmodulepostassociation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixmodulepostassociation");

            entity.HasIndex(e => e.MixModuleContentId, "IX_MixModulePostAssociation_MixModuleContentId");

            entity.Property(e => e.CreatedDateTime).HasMaxLength(6);
            entity.Property(e => e.LastModified).HasMaxLength(6);

            entity.HasOne(d => d.MixModuleContent).WithMany(p => p.Mixmodulepostassociations)
                .HasForeignKey(d => d.MixModuleContentId)
                .HasConstraintName("FK_MixModulePostAssociation_MixModuleContent_MixModuleContentId");
        });

        modelBuilder.Entity<Mixpage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixpage");

            entity.HasIndex(e => e.MixPostContentId, "IX_MixPage_MixPostContentId");

            entity.HasIndex(e => e.MixTenantId, "IX_MixPage_MixTenantId");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixPostContent).WithMany(p => p.Mixpages)
                .HasForeignKey(d => d.MixPostContentId)
                .HasConstraintName("FK_MixPage_MixPostContent_MixPostContentId");

            entity.HasOne(d => d.MixTenant).WithMany(p => p.Mixpages)
                .HasForeignKey(d => d.MixTenantId)
                .HasConstraintName("FK_MixPage_MixTenant_MixTenantId");
        });

        modelBuilder.Entity<Mixpagecontent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixpagecontent");

            entity.HasIndex(e => e.MixCultureId, "IX_MixPageContent_MixCultureId");

            entity.HasIndex(e => e.MixDataContentId, "IX_MixPageContent_MixDataContentId");

            entity.HasIndex(e => e.MixPageId, "IX_MixPageContent_MixPageId");

            entity.Property(e => e.ClassName)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Content)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Excerpt)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.MixDataContentId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.PublishedDateTime).HasColumnType("datetime");
            entity.Property(e => e.SeoDescription)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SeoKeywords)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SeoName)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Source)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Specificulture)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Type)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixCulture).WithMany(p => p.Mixpagecontents)
                .HasForeignKey(d => d.MixCultureId)
                .HasConstraintName("FK_MixPageContent_MixCulture_MixCultureId");

            entity.HasOne(d => d.MixDataContent).WithMany(p => p.Mixpagecontents)
                .HasForeignKey(d => d.MixDataContentId)
                .HasConstraintName("FK_MixPageContent_MixDataContent_MixDataContentId");

            entity.HasOne(d => d.MixPage).WithMany(p => p.Mixpagecontents)
                .HasForeignKey(d => d.MixPageId)
                .HasConstraintName("FK_MixPageContent_MixPage_MixPageId");
        });

        modelBuilder.Entity<Mixpagemoduleassociation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixpagemoduleassociation");

            entity.HasIndex(e => e.MixPageContentId, "IX_MixPageModuleAssociation_MixPageContentId");

            entity.Property(e => e.CreatedDateTime).HasMaxLength(6);
            entity.Property(e => e.LastModified).HasMaxLength(6);

            entity.HasOne(d => d.MixPageContent).WithMany(p => p.Mixpagemoduleassociations)
                .HasForeignKey(d => d.MixPageContentId)
                .HasConstraintName("FK_MixPageModuleAssociation_MixPageContent_MixPageContentId");
        });

        modelBuilder.Entity<Mixpagepostassociation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixpagepostassociation");

            entity.HasIndex(e => e.MixPageContentId, "IX_MixPagePostAssociation_MixPageContentId");

            entity.Property(e => e.CreatedDateTime).HasMaxLength(6);
            entity.Property(e => e.LastModified).HasMaxLength(6);

            entity.HasOne(d => d.MixPageContent).WithMany(p => p.Mixpagepostassociations)
                .HasForeignKey(d => d.MixPageContentId)
                .HasConstraintName("FK_MixPagePostAssociation_MixPageContent_MixPageContentId");
        });

        modelBuilder.Entity<Mixpost>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixpost");

            entity.HasIndex(e => e.MixTenantId, "IX_MixPost_MixTenantId");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixTenant).WithMany(p => p.Mixposts)
                .HasForeignKey(d => d.MixTenantId)
                .HasConstraintName("FK_MixPost_MixTenant_MixTenantId");
        });

        modelBuilder.Entity<Mixpostcontent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixpostcontent");

            entity.HasIndex(e => e.MixCultureId, "IX_MixPostContent_MixCultureId");

            entity.HasIndex(e => e.MixDataContentId, "IX_MixPostContent_MixDataContentId");

            entity.HasIndex(e => e.MixPostContentId, "IX_MixPostContent_MixPostContentId");

            entity.HasIndex(e => e.MixPostId, "IX_MixPostContent_MixPostId");

            entity.Property(e => e.ClassName)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Content)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Excerpt)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Icon)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Image)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.MixDataContentId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.PublishedDateTime).HasColumnType("datetime");
            entity.Property(e => e.SeoDescription)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SeoKeywords)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SeoName)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Source)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Specificulture)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Title)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixCulture).WithMany(p => p.Mixpostcontents)
                .HasForeignKey(d => d.MixCultureId)
                .HasConstraintName("FK_MixPostContent_MixCulture_MixCultureId");

            entity.HasOne(d => d.MixDataContent).WithMany(p => p.Mixpostcontents)
                .HasForeignKey(d => d.MixDataContentId)
                .HasConstraintName("FK_MixPostContent_MixDataContent_MixDataContentId");

            entity.HasOne(d => d.MixPostContent).WithMany(p => p.InverseMixPostContent)
                .HasForeignKey(d => d.MixPostContentId)
                .HasConstraintName("FK_MixPostContent_MixPostContent_MixPostContentId");

            entity.HasOne(d => d.MixPost).WithMany(p => p.Mixpostcontents)
                .HasForeignKey(d => d.MixPostId)
                .HasConstraintName("FK_MixPostContent_MixPost_MixPostId");
        });

        modelBuilder.Entity<Mixpostpostassociation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixpostpostassociation");

            entity.Property(e => e.CreatedDateTime).HasMaxLength(6);
            entity.Property(e => e.LastModified).HasMaxLength(6);
        });

        modelBuilder.Entity<Mixrole>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixroles");

            entity.Property(e => e.Id)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ConcurrencyStamp)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Name)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.NormalizedName)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Mixtenant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixtenant");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.PrimaryDomain)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SystemName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Mixtheme>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixtheme");

            entity.HasIndex(e => e.MixDataContentId, "IX_MixTheme_MixDataContentId");

            entity.HasIndex(e => e.MixTenantId, "IX_MixTheme_MixTenantId");

            entity.Property(e => e.AssetFolder)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.MixDataContentId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.PreviewUrl)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.TemplateFolder)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixDataContent).WithMany(p => p.Mixthemes)
                .HasForeignKey(d => d.MixDataContentId)
                .HasConstraintName("FK_MixTheme_MixDataContent_MixDataContentId");

            entity.HasOne(d => d.MixTenant).WithMany(p => p.Mixthemes)
                .HasForeignKey(d => d.MixTenantId)
                .HasConstraintName("FK_MixTheme_MixTenant_MixTenantId");
        });

        modelBuilder.Entity<Mixurlalias>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixurlalias");

            entity.HasIndex(e => e.MixTenantId, "IX_MixUrlAlias_MixTenantId");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(4000)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.DisplayName)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.SourceContentGuidId)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixTenant).WithMany(p => p.Mixurlaliases)
                .HasForeignKey(d => d.MixTenantId)
                .HasConstraintName("FK_MixUrlAlias_MixTenant_MixTenantId");
        });

        modelBuilder.Entity<Mixuser>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixusers");

            entity.HasIndex(e => e.NormalizedEmail, "EmailIndex");

            entity.HasIndex(e => e.NormalizedUserName, "UserNameIndex").IsUnique();

            entity.Property(e => e.Id)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ConcurrencyStamp)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.LockoutEnd).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.NormalizedEmail)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.NormalizedUserName)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PasswordHash)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.RegisterType)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.SecurityStamp)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.UserName)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Mixusertenant>(entity =>
        {
            entity.HasKey(e => new { e.MixUserId, e.TenantId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("mixusertenants");

            entity.HasIndex(e => e.MixUserId, "IX_MixUserTenants_MixUserId");

            entity.HasIndex(e => e.TenantId, "IX_MixUserTenants_TenantId");

            entity.Property(e => e.MixUserId)
                .HasDefaultValueSql("uuid_to_bin(uuid())")
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
        });

        modelBuilder.Entity<Mixviewtemplate>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("mixviewtemplate");

            entity.HasIndex(e => e.MixThemeId, "IX_MixViewTemplate_MixThemeId");

            entity.Property(e => e.Content)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Extension)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.FileFolder)
                .IsRequired()
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.FileName)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.FolderType)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.MixThemeName)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Scripts)
                .IsRequired()
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Styles)
                .IsRequired()
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");

            entity.HasOne(d => d.MixTheme).WithMany(p => p.Mixviewtemplates)
                .HasForeignKey(d => d.MixThemeId)
                .HasConstraintName("FK_MixViewTemplate_MixTheme_MixThemeId");
        });

        modelBuilder.Entity<Onepaytransactionrequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("onepaytransactionrequest");

            entity.Property(e => e.AgainLink).HasMaxLength(250);
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.OnepayStatus)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.VpcAccessCode)
                .HasMaxLength(250)
                .HasColumnName("vpc_AccessCode");
            entity.Property(e => e.VpcAmount)
                .HasMaxLength(250)
                .HasColumnName("vpc_Amount");
            entity.Property(e => e.VpcCommand)
                .HasMaxLength(250)
                .HasColumnName("vpc_Command");
            entity.Property(e => e.VpcCurrency)
                .HasMaxLength(250)
                .HasColumnName("vpc_Currency");
            entity.Property(e => e.VpcCustomerEmail)
                .HasMaxLength(250)
                .HasColumnName("vpc_Customer_Email");
            entity.Property(e => e.VpcCustomerId)
                .HasMaxLength(250)
                .HasColumnName("vpc_Customer_Id");
            entity.Property(e => e.VpcCustomerPhone)
                .HasMaxLength(250)
                .HasColumnName("vpc_Customer_Phone");
            entity.Property(e => e.VpcLocale)
                .HasMaxLength(250)
                .HasColumnName("vpc_Locale");
            entity.Property(e => e.VpcMerchTxnRef)
                .HasMaxLength(250)
                .HasColumnName("vpc_MerchTxnRef");
            entity.Property(e => e.VpcMerchant)
                .HasMaxLength(250)
                .HasColumnName("vpc_Merchant");
            entity.Property(e => e.VpcOrderInfo)
                .HasMaxLength(250)
                .HasColumnName("vpc_OrderInfo");
            entity.Property(e => e.VpcReturnUrl)
                .HasMaxLength(4000)
                .HasColumnName("vpc_ReturnURL");
            entity.Property(e => e.VpcSecureHash)
                .HasMaxLength(250)
                .HasColumnName("vpc_SecureHash");
            entity.Property(e => e.VpcTicketNo)
                .HasMaxLength(250)
                .HasColumnName("vpc_TicketNo");
            entity.Property(e => e.VpcVersion).HasColumnName("vpc_Version");
        });

        modelBuilder.Entity<Onepaytransactionresponse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("onepaytransactionresponse");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.OnepayStatus)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.VpcAdditionData)
                .HasMaxLength(250)
                .HasColumnName("vpc_AdditionData");
            entity.Property(e => e.VpcAmount)
                .HasMaxLength(250)
                .HasColumnName("vpc_Amount");
            entity.Property(e => e.VpcCommand)
                .HasMaxLength(250)
                .HasColumnName("vpc_Command");
            entity.Property(e => e.VpcCurrencyCode)
                .HasMaxLength(250)
                .HasColumnName("vpc_CurrencyCode");
            entity.Property(e => e.VpcLocale)
                .HasMaxLength(250)
                .HasColumnName("vpc_Locale");
            entity.Property(e => e.VpcMerchTxnRef)
                .HasMaxLength(250)
                .HasColumnName("vpc_MerchTxnRef");
            entity.Property(e => e.VpcMerchant)
                .HasMaxLength(250)
                .HasColumnName("vpc_Merchant");
            entity.Property(e => e.VpcMessage)
                .HasMaxLength(250)
                .HasColumnName("vpc_Message");
            entity.Property(e => e.VpcOrderInfo)
                .HasMaxLength(250)
                .HasColumnName("vpc_OrderInfo");
            entity.Property(e => e.VpcSecureHash)
                .HasMaxLength(250)
                .HasColumnName("vpc_SecureHash");
            entity.Property(e => e.VpcTransactionNo)
                .HasMaxLength(250)
                .HasColumnName("vpc_TransactionNo");
            entity.Property(e => e.VpcTxnResponseCode)
                .HasMaxLength(250)
                .HasColumnName("vpc_TxnResponseCode");
        });

        modelBuilder.Entity<Orderdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("orderdetail");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Currency).HasMaxLength(250);
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.OrderStatus).HasMaxLength(250);
            entity.Property(e => e.PaymentGateway).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.UserId).HasMaxLength(255);
        });

        modelBuilder.Entity<Orderitem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("orderitem");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Currency).HasMaxLength(250);
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.Image).HasMaxLength(250);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.ReferenceUrl).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        modelBuilder.Entity<Productdetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("productdetails");

            entity.Property(e => e.Brands).HasMaxLength(250);
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.DesignBy).HasMaxLength(250);
            entity.Property(e => e.Document).HasMaxLength(250);
            entity.Property(e => e.InformationImage).HasMaxLength(250);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.MaintenanceDocument).HasMaxLength(250);
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.ParentType).HasMaxLength(20);
            entity.Property(e => e.Size).HasMaxLength(250);
            entity.Property(e => e.SizeImage).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
        });

        modelBuilder.Entity<QrtzBlobTrigger>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("qrtz_blob_triggers");

            entity.HasIndex(e => new { e.SchedName, e.TriggerName, e.TriggerGroup }, "SCHED_NAME");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("SCHED_NAME");
            entity.Property(e => e.TriggerName)
                .HasMaxLength(200)
                .HasColumnName("TRIGGER_NAME");
            entity.Property(e => e.TriggerGroup)
                .HasMaxLength(200)
                .HasColumnName("TRIGGER_GROUP");
            entity.Property(e => e.BlobData)
                .HasColumnType("blob")
                .HasColumnName("BLOB_DATA");

            entity.HasOne(d => d.QrtzTrigger).WithOne(p => p.QrtzBlobTrigger)
                .HasForeignKey<QrtzBlobTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("qrtz_blob_triggers_ibfk_1");
        });

        modelBuilder.Entity<QrtzCalendar>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.CalendarName })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("qrtz_calendars");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("SCHED_NAME");
            entity.Property(e => e.CalendarName)
                .HasMaxLength(200)
                .HasColumnName("CALENDAR_NAME");
            entity.Property(e => e.Calendar)
                .IsRequired()
                .HasColumnType("blob")
                .HasColumnName("CALENDAR");
        });

        modelBuilder.Entity<QrtzCronTrigger>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("qrtz_cron_triggers");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("SCHED_NAME");
            entity.Property(e => e.TriggerName)
                .HasMaxLength(200)
                .HasColumnName("TRIGGER_NAME");
            entity.Property(e => e.TriggerGroup)
                .HasMaxLength(200)
                .HasColumnName("TRIGGER_GROUP");
            entity.Property(e => e.CronExpression)
                .IsRequired()
                .HasMaxLength(120)
                .HasColumnName("CRON_EXPRESSION");
            entity.Property(e => e.TimeZoneId)
                .HasMaxLength(80)
                .HasColumnName("TIME_ZONE_ID");

            entity.HasOne(d => d.QrtzTrigger).WithOne(p => p.QrtzCronTrigger)
                .HasForeignKey<QrtzCronTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("qrtz_cron_triggers_ibfk_1");
        });

        modelBuilder.Entity<QrtzFiredTrigger>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.EntryId })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("qrtz_fired_triggers");

            entity.HasIndex(e => new { e.SchedName, e.InstanceName, e.RequestsRecovery }, "IDX_QRTZ_FT_INST_JOB_REQ_RCVRY");

            entity.HasIndex(e => new { e.SchedName, e.JobGroup }, "IDX_QRTZ_FT_JG");

            entity.HasIndex(e => new { e.SchedName, e.JobName, e.JobGroup }, "IDX_QRTZ_FT_J_G");

            entity.HasIndex(e => new { e.SchedName, e.TriggerGroup }, "IDX_QRTZ_FT_TG");

            entity.HasIndex(e => new { e.SchedName, e.InstanceName }, "IDX_QRTZ_FT_TRIG_INST_NAME");

            entity.HasIndex(e => new { e.SchedName, e.TriggerName, e.TriggerGroup }, "IDX_QRTZ_FT_T_G");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("SCHED_NAME");
            entity.Property(e => e.EntryId)
                .HasMaxLength(140)
                .HasColumnName("ENTRY_ID");
            entity.Property(e => e.FiredTime).HasColumnName("FIRED_TIME");
            entity.Property(e => e.InstanceName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("INSTANCE_NAME");
            entity.Property(e => e.IsNonconcurrent).HasColumnName("IS_NONCONCURRENT");
            entity.Property(e => e.JobGroup)
                .HasMaxLength(200)
                .HasColumnName("JOB_GROUP");
            entity.Property(e => e.JobName)
                .HasMaxLength(200)
                .HasColumnName("JOB_NAME");
            entity.Property(e => e.Priority).HasColumnName("PRIORITY");
            entity.Property(e => e.RequestsRecovery).HasColumnName("REQUESTS_RECOVERY");
            entity.Property(e => e.SchedTime).HasColumnName("SCHED_TIME");
            entity.Property(e => e.State)
                .IsRequired()
                .HasMaxLength(16)
                .HasColumnName("STATE");
            entity.Property(e => e.TriggerGroup)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("TRIGGER_GROUP");
            entity.Property(e => e.TriggerName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("TRIGGER_NAME");
        });

        modelBuilder.Entity<QrtzJobDetail>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.JobName, e.JobGroup })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("qrtz_job_details");

            entity.HasIndex(e => new { e.SchedName, e.JobGroup }, "IDX_QRTZ_J_GRP");

            entity.HasIndex(e => new { e.SchedName, e.RequestsRecovery }, "IDX_QRTZ_J_REQ_RECOVERY");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("SCHED_NAME");
            entity.Property(e => e.JobName)
                .HasMaxLength(200)
                .HasColumnName("JOB_NAME");
            entity.Property(e => e.JobGroup)
                .HasMaxLength(200)
                .HasColumnName("JOB_GROUP");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.IsDurable).HasColumnName("IS_DURABLE");
            entity.Property(e => e.IsNonconcurrent).HasColumnName("IS_NONCONCURRENT");
            entity.Property(e => e.IsUpdateData).HasColumnName("IS_UPDATE_DATA");
            entity.Property(e => e.JobClassName)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnName("JOB_CLASS_NAME");
            entity.Property(e => e.JobData)
                .HasColumnType("blob")
                .HasColumnName("JOB_DATA");
            entity.Property(e => e.RequestsRecovery).HasColumnName("REQUESTS_RECOVERY");
        });

        modelBuilder.Entity<QrtzLock>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.LockName })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("qrtz_locks");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("SCHED_NAME");
            entity.Property(e => e.LockName)
                .HasMaxLength(40)
                .HasColumnName("LOCK_NAME");
        });

        modelBuilder.Entity<QrtzPausedTriggerGrp>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.TriggerGroup })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("qrtz_paused_trigger_grps");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("SCHED_NAME");
            entity.Property(e => e.TriggerGroup)
                .HasMaxLength(200)
                .HasColumnName("TRIGGER_GROUP");
        });

        modelBuilder.Entity<QrtzSchedulerState>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.InstanceName })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0 });

            entity.ToTable("qrtz_scheduler_state");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("SCHED_NAME");
            entity.Property(e => e.InstanceName)
                .HasMaxLength(200)
                .HasColumnName("INSTANCE_NAME");
            entity.Property(e => e.CheckinInterval).HasColumnName("CHECKIN_INTERVAL");
            entity.Property(e => e.LastCheckinTime).HasColumnName("LAST_CHECKIN_TIME");
        });

        modelBuilder.Entity<QrtzSimpleTrigger>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("qrtz_simple_triggers");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("SCHED_NAME");
            entity.Property(e => e.TriggerName)
                .HasMaxLength(200)
                .HasColumnName("TRIGGER_NAME");
            entity.Property(e => e.TriggerGroup)
                .HasMaxLength(200)
                .HasColumnName("TRIGGER_GROUP");
            entity.Property(e => e.RepeatCount).HasColumnName("REPEAT_COUNT");
            entity.Property(e => e.RepeatInterval).HasColumnName("REPEAT_INTERVAL");
            entity.Property(e => e.TimesTriggered).HasColumnName("TIMES_TRIGGERED");

            entity.HasOne(d => d.QrtzTrigger).WithOne(p => p.QrtzSimpleTrigger)
                .HasForeignKey<QrtzSimpleTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("qrtz_simple_triggers_ibfk_1");
        });

        modelBuilder.Entity<QrtzSimpropTrigger>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("qrtz_simprop_triggers");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("SCHED_NAME");
            entity.Property(e => e.TriggerName)
                .HasMaxLength(200)
                .HasColumnName("TRIGGER_NAME");
            entity.Property(e => e.TriggerGroup)
                .HasMaxLength(200)
                .HasColumnName("TRIGGER_GROUP");
            entity.Property(e => e.BoolProp1).HasColumnName("BOOL_PROP_1");
            entity.Property(e => e.BoolProp2).HasColumnName("BOOL_PROP_2");
            entity.Property(e => e.DecProp1)
                .HasPrecision(13, 4)
                .HasColumnName("DEC_PROP_1");
            entity.Property(e => e.DecProp2)
                .HasPrecision(13, 4)
                .HasColumnName("DEC_PROP_2");
            entity.Property(e => e.IntProp1).HasColumnName("INT_PROP_1");
            entity.Property(e => e.IntProp2).HasColumnName("INT_PROP_2");
            entity.Property(e => e.LongProp1).HasColumnName("LONG_PROP_1");
            entity.Property(e => e.LongProp2).HasColumnName("LONG_PROP_2");
            entity.Property(e => e.StrProp1)
                .HasMaxLength(512)
                .HasColumnName("STR_PROP_1");
            entity.Property(e => e.StrProp2)
                .HasMaxLength(512)
                .HasColumnName("STR_PROP_2");
            entity.Property(e => e.StrProp3)
                .HasMaxLength(512)
                .HasColumnName("STR_PROP_3");
            entity.Property(e => e.TimeZoneId)
                .HasMaxLength(80)
                .HasColumnName("TIME_ZONE_ID");

            entity.HasOne(d => d.QrtzTrigger).WithOne(p => p.QrtzSimpropTrigger)
                .HasForeignKey<QrtzSimpropTrigger>(d => new { d.SchedName, d.TriggerName, d.TriggerGroup })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("qrtz_simprop_triggers_ibfk_1");
        });

        modelBuilder.Entity<QrtzTrigger>(entity =>
        {
            entity.HasKey(e => new { e.SchedName, e.TriggerName, e.TriggerGroup })
                .HasName("PRIMARY")
                .HasAnnotation("MySql:IndexPrefixLength", new[] { 0, 0, 0 });

            entity.ToTable("qrtz_triggers");

            entity.HasIndex(e => new { e.SchedName, e.CalendarName }, "IDX_QRTZ_T_C");

            entity.HasIndex(e => new { e.SchedName, e.TriggerGroup }, "IDX_QRTZ_T_G");

            entity.HasIndex(e => new { e.SchedName, e.JobName, e.JobGroup }, "IDX_QRTZ_T_J");

            entity.HasIndex(e => new { e.SchedName, e.JobGroup }, "IDX_QRTZ_T_JG");

            entity.HasIndex(e => new { e.SchedName, e.NextFireTime }, "IDX_QRTZ_T_NEXT_FIRE_TIME");

            entity.HasIndex(e => new { e.SchedName, e.MisfireInstr, e.NextFireTime }, "IDX_QRTZ_T_NFT_MISFIRE");

            entity.HasIndex(e => new { e.SchedName, e.TriggerState, e.NextFireTime }, "IDX_QRTZ_T_NFT_ST");

            entity.HasIndex(e => new { e.SchedName, e.MisfireInstr, e.NextFireTime, e.TriggerState }, "IDX_QRTZ_T_NFT_ST_MISFIRE");

            entity.HasIndex(e => new { e.SchedName, e.MisfireInstr, e.NextFireTime, e.TriggerGroup, e.TriggerState }, "IDX_QRTZ_T_NFT_ST_MISFIRE_GRP");

            entity.HasIndex(e => new { e.SchedName, e.TriggerGroup, e.TriggerState }, "IDX_QRTZ_T_N_G_STATE");

            entity.HasIndex(e => new { e.SchedName, e.TriggerName, e.TriggerGroup, e.TriggerState }, "IDX_QRTZ_T_N_STATE");

            entity.HasIndex(e => new { e.SchedName, e.TriggerState }, "IDX_QRTZ_T_STATE");

            entity.Property(e => e.SchedName)
                .HasMaxLength(120)
                .HasColumnName("SCHED_NAME");
            entity.Property(e => e.TriggerName)
                .HasMaxLength(200)
                .HasColumnName("TRIGGER_NAME");
            entity.Property(e => e.TriggerGroup)
                .HasMaxLength(200)
                .HasColumnName("TRIGGER_GROUP");
            entity.Property(e => e.CalendarName)
                .HasMaxLength(200)
                .HasColumnName("CALENDAR_NAME");
            entity.Property(e => e.Description)
                .HasMaxLength(250)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.EndTime).HasColumnName("END_TIME");
            entity.Property(e => e.JobData)
                .HasColumnType("blob")
                .HasColumnName("JOB_DATA");
            entity.Property(e => e.JobGroup)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("JOB_GROUP");
            entity.Property(e => e.JobName)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("JOB_NAME");
            entity.Property(e => e.MisfireInstr).HasColumnName("MISFIRE_INSTR");
            entity.Property(e => e.NextFireTime).HasColumnName("NEXT_FIRE_TIME");
            entity.Property(e => e.PrevFireTime).HasColumnName("PREV_FIRE_TIME");
            entity.Property(e => e.Priority).HasColumnName("PRIORITY");
            entity.Property(e => e.StartTime).HasColumnName("START_TIME");
            entity.Property(e => e.TriggerState)
                .IsRequired()
                .HasMaxLength(16)
                .HasColumnName("TRIGGER_STATE");
            entity.Property(e => e.TriggerType)
                .IsRequired()
                .HasMaxLength(8)
                .HasColumnName("TRIGGER_TYPE");

            entity.HasOne(d => d.QrtzJobDetail).WithMany(p => p.QrtzTriggers)
                .HasForeignKey(d => new { d.SchedName, d.JobName, d.JobGroup })
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("qrtz_triggers_ibfk_1");
        });

        modelBuilder.Entity<Refreshtoken>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("refreshtokens");

            entity.Property(e => e.Id)
                .UseCollation("ascii_general_ci")
                .HasCharSet("ascii");
            entity.Property(e => e.ClientId)
                .IsRequired()
                .HasMaxLength(50)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Email)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.ExpiresUtc).HasColumnType("datetime");
            entity.Property(e => e.IssuedUtc).HasColumnType("datetime");
            entity.Property(e => e.Username)
                .HasMaxLength(250)
                .UseCollation("utf8mb3_unicode_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Shoppingcart>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("shoppingcart")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_unicode_ci");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.UserId).HasMaxLength(255);
        });

        modelBuilder.Entity<Syscategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("syscategory")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_unicode_ci");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        modelBuilder.Entity<Sysmedium>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity
                .ToTable("sysmedia")
                .HasCharSet("utf8mb3")
                .UseCollation("utf8mb3_unicode_ci");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.File).HasMaxLength(250);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.Type).HasMaxLength(250);
        });

        modelBuilder.Entity<Sysmenuitem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sysmenuitem");

            entity.Property(e => e.Alt).HasMaxLength(250);
            entity.Property(e => e.Classes).HasMaxLength(250);
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.Hreflang).HasMaxLength(250);
            entity.Property(e => e.Icon).HasMaxLength(250);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.Target).HasMaxLength(250);
            entity.Property(e => e.TargetId).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.Type).HasMaxLength(250);
            entity.Property(e => e.Url).HasMaxLength(250);
        });

        modelBuilder.Entity<Sysnavigation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sysnavigation");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Name).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        modelBuilder.Entity<Syspermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("syspermission");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Icon).HasMaxLength(250);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.Type).HasMaxLength(250);
        });

        modelBuilder.Entity<Syspermissionendpoint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("syspermissionendpoint");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.Method).HasMaxLength(250);
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Path).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
        });

        modelBuilder.Entity<Systag>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("systag");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Slug).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.Title).HasMaxLength(250);
            entity.Property(e => e.Type).HasMaxLength(250);
        });

        modelBuilder.Entity<Sysuserdatum>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sysuserdata");

            entity.Property(e => e.Avatar).HasMaxLength(250);
            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.ParentId).HasMaxLength(255);
            entity.Property(e => e.ParentType).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(250);
        });

        modelBuilder.Entity<Sysuserpermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sysuserpermission");

            entity.Property(e => e.CreatedBy).HasMaxLength(250);
            entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(250);
            entity.Property(e => e.LastModified).HasColumnType("datetime");
            entity.Property(e => e.ModifiedBy).HasMaxLength(250);
            entity.Property(e => e.Status).HasMaxLength(250);
            entity.Property(e => e.UserId).HasMaxLength(255);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
