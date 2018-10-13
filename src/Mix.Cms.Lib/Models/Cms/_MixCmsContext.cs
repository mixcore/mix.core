using Microsoft.EntityFrameworkCore;
using Mix.Cms.Lib.Services;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MixCmsContext : DbContext
    {
        public virtual DbSet<MixArticle> MixArticle { get; set; }
        public virtual DbSet<MixArticleMedia> MixArticleMedia { get; set; }
        public virtual DbSet<MixArticleModule> MixArticleModule { get; set; }
        public virtual DbSet<MixCmsUser> MixCmsUser { get; set; }
        public virtual DbSet<MixComment> MixComment { get; set; }
        public virtual DbSet<MixConfiguration> MixConfiguration { get; set; }
        public virtual DbSet<MixCopy> MixCopy { get; set; }
        public virtual DbSet<MixCulture> MixCulture { get; set; }
        public virtual DbSet<MixCustomer> MixCustomer { get; set; }
        public virtual DbSet<MixFile> MixFile { get; set; }
        public virtual DbSet<MixLanguage> MixLanguage { get; set; }
        public virtual DbSet<MixMedia> MixMedia { get; set; }
        public virtual DbSet<MixModule> MixModule { get; set; }
        public virtual DbSet<MixModuleArticle> MixModuleArticle { get; set; }
        public virtual DbSet<MixModuleAttributeSet> MixModuleAttributeSet { get; set; }
        public virtual DbSet<MixModuleAttributeValue> MixModuleAttributeValue { get; set; }
        public virtual DbSet<MixModuleData> MixModuleData { get; set; }
        public virtual DbSet<MixModuleProduct> MixModuleProduct { get; set; }
        public virtual DbSet<MixOrder> MixOrder { get; set; }
        public virtual DbSet<MixOrderItem> MixOrderItem { get; set; }
        public virtual DbSet<MixPage> MixPage { get; set; }
        public virtual DbSet<MixPageArticle> MixPageArticle { get; set; }
        public virtual DbSet<MixPageModule> MixPageModule { get; set; }
        public virtual DbSet<MixPagePage> MixPagePage { get; set; }
        public virtual DbSet<MixPagePosition> MixPagePosition { get; set; }
        public virtual DbSet<MixPageProduct> MixPageProduct { get; set; }
        public virtual DbSet<MixParameter> MixParameter { get; set; }
        public virtual DbSet<MixPortalPage> MixPortalPage { get; set; }
        public virtual DbSet<MixPortalPageNavigation> MixPortalPageNavigation { get; set; }
        public virtual DbSet<MixPortalPagePosition> MixPortalPagePosition { get; set; }
        public virtual DbSet<MixPortalPageRole> MixPortalPageRole { get; set; }
        public virtual DbSet<MixPosition> MixPosition { get; set; }
        public virtual DbSet<MixProduct> MixProduct { get; set; }
        public virtual DbSet<MixProductMedia> MixProductMedia { get; set; }
        public virtual DbSet<MixProductModule> MixProductModule { get; set; }
        public virtual DbSet<MixRelatedArticle> MixRelatedArticle { get; set; }
        public virtual DbSet<MixRelatedProduct> MixRelatedProduct { get; set; }
        public virtual DbSet<MixSetAttribute> MixSetAttribute { get; set; }
        public virtual DbSet<MixTemplate> MixTemplate { get; set; }
        public virtual DbSet<MixTheme> MixTheme { get; set; }
        public virtual DbSet<MixUrlAlias> MixUrlAlias { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MixCmsContext(DbContextOptions<MixCmsContext> options)
                    : base(options)
        {
        }
        public MixCmsContext()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //define the database to use
            string cnn = MixService.GetConnectionString(MixConstants.CONST_CMS_CONNECTION);
            if (!string.IsNullOrEmpty(cnn))
            {
                if (MixService.GetConfig<bool>("IsSqlite"))
                {
                    optionsBuilder.UseSqlite(cnn);
                }
                else
                {
                    optionsBuilder.UseSqlServer(cnn);
                }
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MixArticle>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Specificulture });

                entity.ToTable("mix_article");

                entity.HasIndex(e => e.SetAttributeId);

                entity.HasIndex(e => e.Specificulture);

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.CreatedBy).HasMaxLength(250);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.ExtraProperties).HasColumnType("ntext");

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(250);

                entity.Property(e => e.SeoDescription).HasMaxLength(4000);

                entity.Property(e => e.SeoKeywords).HasMaxLength(4000);

                entity.Property(e => e.SeoName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SeoTitle).HasMaxLength(4000);

                entity.Property(e => e.SetAttributeData).HasColumnType("ntext");

                entity.Property(e => e.Source).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Tags).HasMaxLength(500);

                entity.Property(e => e.Template).HasMaxLength(250);

                entity.Property(e => e.Thumbnail).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(4000);

                entity.HasOne(d => d.SetAttribute)
                    .WithMany(p => p.MixArticle)
                    .HasForeignKey(d => d.SetAttributeId)
                    .HasConstraintName("FK_mix_article_mix_set_attribute");

                entity.HasOne(d => d.SpecificultureNavigation)
                    .WithMany(p => p.MixArticle)
                    .HasPrincipalKey(p => p.Specificulture)
                    .HasForeignKey(d => d.Specificulture)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Article_Mix_Culture");
            });

            modelBuilder.Entity<MixArticleMedia>(entity =>
            {
                entity.HasKey(e => new { e.MediaId, e.ArticleId, e.Specificulture });

                entity.ToTable("mix_article_media");

                entity.HasIndex(e => new { e.ArticleId, e.Specificulture });

                entity.HasIndex(e => new { e.MediaId, e.Specificulture });

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MixArticle)
                    .WithMany(p => p.MixArticleMedia)
                    .HasForeignKey(d => new { d.ArticleId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mix_article_media_mix_article");

                entity.HasOne(d => d.MixMedia)
                    .WithMany(p => p.MixArticleMedia)
                    .HasForeignKey(d => new { d.MediaId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mix_article_media_mix_media");
            });

            modelBuilder.Entity<MixArticleModule>(entity =>
            {
                entity.HasKey(e => new { e.ModuleId, e.ArticleId, e.Specificulture });

                entity.ToTable("mix_article_module");

                entity.HasIndex(e => new { e.ArticleId, e.Specificulture });

                entity.HasIndex(e => new { e.ModuleId, e.Specificulture });

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MixArticle)
                    .WithMany(p => p.MixArticleModule)
                    .HasForeignKey(d => new { d.ArticleId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Article_Module_Mix_Article");

                entity.HasOne(d => d.MixModule)
                    .WithMany(p => p.MixArticleModule)
                    .HasForeignKey(d => new { d.ModuleId, d.Specificulture })
                    .HasConstraintName("FK_Mix_Article_Module_Mix_Module1");
            });

            modelBuilder.Entity<MixCmsUser>(entity =>
            {
                entity.ToTable("mix_cms_user");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(450);

                entity.Property(e => e.Avatar).HasMaxLength(250);

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Username).HasMaxLength(256);
            });

            modelBuilder.Entity<MixComment>(entity =>
            {
                entity.ToTable("mix_comment");

                entity.HasIndex(e => new { e.ArticleId, e.Specificulture });

                entity.HasIndex(e => new { e.OrderId, e.Specificulture });

                entity.HasIndex(e => new { e.ProductId, e.Specificulture });

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(250);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.FullName).HasMaxLength(250);

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedBy).HasMaxLength(250);

                entity.Property(e => e.UpdatedDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.MixArticle)
                    .WithMany(p => p.MixComment)
                    .HasForeignKey(d => new { d.ArticleId, d.Specificulture })
                    .HasConstraintName("FK_mix_comment_mix_article");

                entity.HasOne(d => d.MixOrder)
                    .WithMany(p => p.MixComment)
                    .HasForeignKey(d => new { d.OrderId, d.Specificulture })
                    .HasConstraintName("FK_mix_comment_mix_order");

                entity.HasOne(d => d.MixProduct)
                    .WithMany(p => p.MixComment)
                    .HasForeignKey(d => new { d.ProductId, d.Specificulture })
                    .HasConstraintName("FK_mix_comment_mix_product");
            });

            modelBuilder.Entity<MixConfiguration>(entity =>
            {
                entity.HasKey(e => new { e.Keyword, e.Specificulture });

                entity.ToTable("mix_configuration");

                entity.HasIndex(e => e.Specificulture);

                entity.Property(e => e.Keyword).HasMaxLength(250);

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Category).HasMaxLength(250);

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.SpecificultureNavigation)
                    .WithMany(p => p.MixConfiguration)
                    .HasPrincipalKey(p => p.Specificulture)
                    .HasForeignKey(d => d.Specificulture)
                    .HasConstraintName("FK_Mix_Configuration_Mix_Culture");
            });

            modelBuilder.Entity<MixCopy>(entity =>
            {
                entity.HasKey(e => new { e.Culture, e.Keyword });

                entity.ToTable("mix_copy");

                entity.Property(e => e.Culture).HasMaxLength(10);

                entity.Property(e => e.Keyword).HasMaxLength(250);

                entity.Property(e => e.Note).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<MixCulture>(entity =>
            {
                entity.ToTable("mix_culture");

                entity.HasIndex(e => e.Specificulture)
                    .HasName("IX_Mix_Culture")
                    .IsUnique();

                entity.Property(e => e.Alias).HasMaxLength(150);

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.FullName).HasMaxLength(150);

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.Lcid)
                    .HasColumnName("LCID")
                    .HasMaxLength(50);

                entity.Property(e => e.Specificulture)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<MixCustomer>(entity =>
            {
                entity.ToTable("mix_customer");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Address).HasMaxLength(450);

                entity.Property(e => e.Avatar).HasMaxLength(250);

                entity.Property(e => e.BirthDay).HasColumnType("datetime");

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(250);

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.MiddleName).HasMaxLength(50);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.UserId).HasMaxLength(256);

                entity.Property(e => e.Username).HasMaxLength(256);
            });

            modelBuilder.Entity<MixFile>(entity =>
            {
                entity.ToTable("mix_file");

                entity.HasIndex(e => e.ThemeId);

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FileFolder)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.FolderType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.ThemeName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.MixFile)
                    .HasForeignKey(d => d.ThemeId)
                    .HasConstraintName("FK_mix_file_mix_template");
            });

            modelBuilder.Entity<MixLanguage>(entity =>
            {
                entity.HasKey(e => new { e.Keyword, e.Specificulture });

                entity.ToTable("mix_language");

                entity.HasIndex(e => e.Specificulture);

                entity.Property(e => e.Keyword).HasMaxLength(250);

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Category).HasMaxLength(250);

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DefaultValue).HasMaxLength(250);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.SpecificultureNavigation)
                    .WithMany(p => p.MixLanguage)
                    .HasPrincipalKey(p => p.Specificulture)
                    .HasForeignKey(d => d.Specificulture)
                    .HasConstraintName("FK_Mix_Language_Mix_Culture");
            });

            modelBuilder.Entity<MixMedia>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Specificulture });

                entity.ToTable("mix_media");

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FileFolder)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.FileProperties).HasMaxLength(4000);

                entity.Property(e => e.FileType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Tags).HasMaxLength(400);

                entity.Property(e => e.Title).HasMaxLength(4000);
            });

            modelBuilder.Entity<MixModule>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Specificulture });

                entity.ToTable("mix_module");

                entity.HasIndex(e => e.Specificulture);

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.Fields).HasMaxLength(4000);

                entity.Property(e => e.FormTemplate).HasMaxLength(4000);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.ModifiedBy).HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Template).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.Property(e => e.Type).HasDefaultValueSql("('0')");

                entity.HasOne(d => d.SpecificultureNavigation)
                    .WithMany(p => p.MixModule)
                    .HasPrincipalKey(p => p.Specificulture)
                    .HasForeignKey(d => d.Specificulture)
                    .HasConstraintName("FK_Mix_Module_Mix_Culture");
            });

            modelBuilder.Entity<MixModuleArticle>(entity =>
            {
                entity.HasKey(e => new { e.ArticleId, e.ModuleId, e.Specificulture });

                entity.ToTable("mix_module_article");

                entity.HasIndex(e => new { e.ArticleId, e.Specificulture });

                entity.HasIndex(e => new { e.ModuleId, e.Specificulture });

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MixArticle)
                    .WithMany(p => p.MixModuleArticle)
                    .HasForeignKey(d => new { d.ArticleId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Module_Article_Mix_Article");

                entity.HasOne(d => d.MixModule)
                    .WithMany(p => p.MixModuleArticle)
                    .HasForeignKey(d => new { d.ModuleId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Module_Article_Mix_Module");
            });

            modelBuilder.Entity<MixModuleAttributeSet>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ModuleId, e.Specificulture });

                entity.ToTable("mix_module_attribute_set");

                entity.HasIndex(e => new { e.ModuleId, e.Specificulture });

                entity.HasIndex(e => new { e.ModuleId, e.ArticleId, e.Specificulture });

                entity.HasIndex(e => new { e.ModuleId, e.CategoryId, e.Specificulture })
                    .HasName("IX_mix_module_attribute_set_ModuleId_PageId_Specificulture");

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Fields)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.MixModule)
                    .WithMany(p => p.MixModuleAttributeSet)
                    .HasForeignKey(d => new { d.ModuleId, d.Specificulture })
                    .HasConstraintName("FK_Mix_Module_Attribute_set_Mix_Module");

                entity.HasOne(d => d.MixArticleModule)
                    .WithMany(p => p.MixModuleAttributeSet)
                    .HasForeignKey(d => new { d.ModuleId, d.ArticleId, d.Specificulture })
                    .HasConstraintName("FK_Mix_Module_Attribute_set_Mix_Article_Module");

                entity.HasOne(d => d.MixPageModule)
                    .WithMany(p => p.MixModuleAttributeSet)
                    .HasForeignKey(d => new { d.ModuleId, d.CategoryId, d.Specificulture })
                    .HasConstraintName("FK_Mix_Module_Attribute_set_Mix_Page_Module");
            });

            modelBuilder.Entity<MixModuleAttributeValue>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.AttributeSetId, e.Specificulture });

                entity.ToTable("mix_module_attribute_value");

                entity.HasIndex(e => new { e.AttributeSetId, e.ModuleId, e.Specificulture });

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.DefaultValue)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.Name).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.HasOne(d => d.MixModuleAttributeSet)
                    .WithMany(p => p.MixModuleAttributeValue)
                    .HasForeignKey(d => new { d.AttributeSetId, d.ModuleId, d.Specificulture })
                    .HasConstraintName("FK_mix_module_attribute_value_mix_module_attribute_set");
            });

            modelBuilder.Entity<MixModuleData>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ModuleId, e.Specificulture });

                entity.ToTable("mix_module_data");

                entity.HasIndex(e => new { e.ModuleId, e.Specificulture });

                entity.HasIndex(e => new { e.ModuleId, e.ArticleId, e.Specificulture });

                entity.HasIndex(e => new { e.ModuleId, e.CategoryId, e.Specificulture })
                    .HasName("IX_mix_module_data_ModuleId_PageId_Specificulture");

                entity.HasIndex(e => new { e.ModuleId, e.ProductId, e.Specificulture });

                entity.Property(e => e.Id).HasMaxLength(50);

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Fields)
                    .IsRequired()
                    .HasMaxLength(4000);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.UpdatedDateTime).HasColumnType("datetime");

                entity.HasOne(d => d.MixModule)
                    .WithMany(p => p.MixModuleData)
                    .HasForeignKey(d => new { d.ModuleId, d.Specificulture })
                    .HasConstraintName("FK_Mix_Module_Data_Mix_Module");

                entity.HasOne(d => d.MixPageModule)
                    .WithMany(p => p.MixModuleData)
                    .HasForeignKey(d => new { d.ModuleId, d.CategoryId, d.Specificulture })
                    .HasConstraintName("FK_Mix_Module_Data_Mix_Page_Module");

                entity.HasOne(d => d.MixArticleModule)
                    .WithMany(p => p.MixModuleData)
                    .HasForeignKey(d => new { d.ModuleId, d.ProductId, d.Specificulture })
                    .HasConstraintName("FK_Mix_Module_Data_Mix_Product_Module");
            });

            modelBuilder.Entity<MixModuleProduct>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.ModuleId, e.Specificulture });

                entity.ToTable("mix_module_product");

                entity.HasIndex(e => new { e.ModuleId, e.Specificulture });

                entity.HasIndex(e => new { e.ProductId, e.Specificulture });

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MixModule)
                    .WithMany(p => p.MixModuleProduct)
                    .HasForeignKey(d => new { d.ModuleId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Module_Product_Mix_Module");

                entity.HasOne(d => d.MixProduct)
                    .WithMany(p => p.MixModuleProduct)
                    .HasForeignKey(d => new { d.ProductId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Module_Product_Mix_Product");
            });

            modelBuilder.Entity<MixOrder>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Specificulture });

                entity.ToTable("mix_order");

                entity.HasIndex(e => e.CustomerId);

                entity.HasIndex(e => e.Specificulture);

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.UserId).HasMaxLength(50);

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.MixOrder)
                    .HasForeignKey(d => d.CustomerId)
                    .HasConstraintName("FK_mix_order_mix_cms_customer");
            });

            modelBuilder.Entity<MixOrderItem>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.OrderId, e.Specificulture });

                entity.ToTable("mix_order_item");

                entity.HasIndex(e => e.Specificulture);

                entity.HasIndex(e => new { e.Id, e.Specificulture })
                    .HasName("AK_mix_order_item_Id_Specificulture")
                    .IsUnique();

                entity.HasIndex(e => new { e.OrderId, e.Specificulture });

                entity.HasIndex(e => new { e.ProductId, e.Specificulture });

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.PriceUnit).HasMaxLength(50);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MixOrder)
                    .WithMany(p => p.MixOrderItem)
                    .HasForeignKey(d => new { d.OrderId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Item_Order");

                entity.HasOne(d => d.MixProduct)
                    .WithMany(p => p.MixOrderItem)
                    .HasForeignKey(d => new { d.ProductId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Order_Item_Product");
            });

            modelBuilder.Entity<MixPage>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Specificulture });

                entity.ToTable("mix_page");

                entity.HasIndex(e => e.SetAttributeId);

                entity.HasIndex(e => e.Specificulture);

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.CssClass).HasMaxLength(50);

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.Layout).HasMaxLength(50);

                entity.Property(e => e.ModifiedBy).HasMaxLength(250);

                entity.Property(e => e.SeoDescription).HasMaxLength(4000);

                entity.Property(e => e.SeoKeywords).HasMaxLength(4000);

                entity.Property(e => e.SeoName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SeoTitle).HasMaxLength(4000);

                entity.Property(e => e.SetAttributeData).HasColumnType("ntext");

                entity.Property(e => e.StaticUrl).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Tags).HasMaxLength(500);

                entity.Property(e => e.Template).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(4000);

                entity.HasOne(d => d.SetAttribute)
                    .WithMany(p => p.MixPage)
                    .HasForeignKey(d => d.SetAttributeId)
                    .HasConstraintName("FK_mix_page_mix_set_attribute");

                entity.HasOne(d => d.SpecificultureNavigation)
                    .WithMany(p => p.MixPage)
                    .HasPrincipalKey(p => p.Specificulture)
                    .HasForeignKey(d => d.Specificulture)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Page_Mix_Culture");
            });

            modelBuilder.Entity<MixPageArticle>(entity =>
            {
                entity.HasKey(e => new { e.ArticleId, e.CategoryId, e.Specificulture });

                entity.ToTable("mix_page_article");

                entity.HasIndex(e => new { e.ArticleId, e.Specificulture });

                entity.HasIndex(e => new { e.CategoryId, e.Specificulture })
                    .HasName("IX_mix_page_article_PageId_Specificulture");

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Priority).HasDefaultValueSql("((0))");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MixArticle)
                    .WithMany(p => p.MixPageArticle)
                    .HasForeignKey(d => new { d.ArticleId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Page_Article_Mix_Article");

                entity.HasOne(d => d.MixPage)
                    .WithMany(p => p.MixPageArticle)
                    .HasForeignKey(d => new { d.CategoryId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Page_Article_Mix_Page");
            });

            modelBuilder.Entity<MixPageModule>(entity =>
            {
                entity.HasKey(e => new { e.ModuleId, e.CategoryId, e.Specificulture });

                entity.ToTable("mix_page_module");

                entity.HasIndex(e => new { e.CategoryId, e.Specificulture })
                    .HasName("IX_mix_page_module_PageId_Specificulture");

                entity.HasIndex(e => new { e.ModuleId, e.Specificulture });

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MixPage)
                    .WithMany(p => p.MixPageModule)
                    .HasForeignKey(d => new { d.CategoryId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Page_Module_Mix_Page");

                entity.HasOne(d => d.MixModule)
                    .WithMany(p => p.MixPageModule)
                    .HasForeignKey(d => new { d.ModuleId, d.Specificulture })
                    .HasConstraintName("FK_Mix_Menu_Module_Mix_Module1");
            });

            modelBuilder.Entity<MixPagePage>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ParentId, e.Specificulture });

                entity.ToTable("mix_page_page");

                entity.HasIndex(e => new { e.Id, e.Specificulture });

                entity.HasIndex(e => new { e.ParentId, e.Specificulture });

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MixPage)
                    .WithMany(p => p.MixPagePageMixPage)
                    .HasForeignKey(d => new { d.Id, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Page_Page_Mix_Page");

                entity.HasOne(d => d.MixPageNavigation)
                    .WithMany(p => p.MixPagePageMixPageNavigation)
                    .HasForeignKey(d => new { d.ParentId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Page_Page_Mix_Page1");
            });

            modelBuilder.Entity<MixPagePosition>(entity =>
            {
                entity.HasKey(e => new { e.PositionId, e.CategoryId, e.Specificulture });

                entity.ToTable("mix_page_position");

                entity.HasIndex(e => new { e.CategoryId, e.Specificulture })
                    .HasName("IX_mix_page_position_PageId_Specificulture");

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.MixPagePosition)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK_Mix_Page_Position_Mix_Position");

                entity.HasOne(d => d.MixPage)
                    .WithMany(p => p.MixPagePosition)
                    .HasForeignKey(d => new { d.CategoryId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Page_Position_Mix_Page");
            });

            modelBuilder.Entity<MixPageProduct>(entity =>
            {
                entity.HasKey(e => new { e.ProductId, e.CategoryId, e.Specificulture });

                entity.ToTable("mix_page_product");

                entity.HasIndex(e => new { e.CategoryId, e.Specificulture })
                    .HasName("IX_mix_page_product_PageId_Specificulture");

                entity.HasIndex(e => new { e.ProductId, e.Specificulture });

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MixPage)
                    .WithMany(p => p.MixPageProduct)
                    .HasForeignKey(d => new { d.CategoryId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Page_Product_Mix_Page");

                entity.HasOne(d => d.MixProduct)
                    .WithMany(p => p.MixPageProduct)
                    .HasForeignKey(d => new { d.ProductId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Page_Product_Mix_Product");
            });

            modelBuilder.Entity<MixParameter>(entity =>
            {
                entity.HasKey(e => e.Name);

                entity.ToTable("mix_parameter");

                entity.Property(e => e.Name)
                    .HasMaxLength(256)
                    .ValueGeneratedNever();

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Value).IsRequired();
            });

            modelBuilder.Entity<MixPortalPage>(entity =>
            {
                entity.ToTable("mix_portal_page");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(450);

                entity.Property(e => e.Icon).HasMaxLength(50);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.TextDefault).HasMaxLength(250);

                entity.Property(e => e.TextKeyword).HasMaxLength(250);

                entity.Property(e => e.Url).HasMaxLength(250);
            });

            modelBuilder.Entity<MixPortalPageNavigation>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ParentId });

                entity.ToTable("mix_portal_page_navigation");

                entity.HasIndex(e => e.ParentId);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.IdNavigation)
                    .WithMany(p => p.MixPortalPageNavigationIdNavigation)
                    .HasForeignKey(d => d.Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mix_portal_page_navigation_mix_portal_page");

                entity.HasOne(d => d.Parent)
                    .WithMany(p => p.MixPortalPageNavigationParent)
                    .HasForeignKey(d => d.ParentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mix_portal_page_navigation_mix_portal_page1");
            });

            modelBuilder.Entity<MixPortalPagePosition>(entity =>
            {
                entity.HasKey(e => new { e.PositionId, e.PortalPageId });

                entity.ToTable("mix_portal_page_position");

                entity.HasIndex(e => e.PortalPageId);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.PortalPage)
                    .WithMany(p => p.MixPortalPagePosition)
                    .HasForeignKey(d => d.PortalPageId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_PortalPage_Position_Mix_PortalPage");

                entity.HasOne(d => d.Position)
                    .WithMany(p => p.MixPortalPagePosition)
                    .HasForeignKey(d => d.PositionId)
                    .HasConstraintName("FK_Mix_PortalPage_Position_Mix_Position");
            });

            modelBuilder.Entity<MixPortalPageRole>(entity =>
            {
                entity.HasKey(e => new { e.RoleId, e.PageId });

                entity.ToTable("mix_portal_page_role");

                entity.HasIndex(e => e.PageId);

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.Page)
                    .WithMany(p => p.MixPortalPageRole)
                    .HasForeignKey(d => d.PageId)
                    .HasConstraintName("FK_mix_portal_page_role_mix_portal_page");
            });

            modelBuilder.Entity<MixPosition>(entity =>
            {
                entity.ToTable("mix_position");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<MixProduct>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Specificulture });

                entity.ToTable("mix_product");

                entity.HasIndex(e => e.SetAttributeId);

                entity.HasIndex(e => e.Specificulture);

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Code)
                    .HasMaxLength(50)
                    .HasDefaultValueSql("(N'')");

                entity.Property(e => e.CreatedBy).HasMaxLength(250);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.DealPrice).HasDefaultValueSql("((0))");

                entity.Property(e => e.ExtraProperties).HasColumnType("ntext");

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.Material).HasMaxLength(250);

                entity.Property(e => e.ModifiedBy).HasMaxLength(250);

                entity.Property(e => e.PackageCount).HasDefaultValueSql("((1))");

                entity.Property(e => e.PrivacyId).HasMaxLength(10);

                entity.Property(e => e.SeoDescription).HasMaxLength(4000);

                entity.Property(e => e.SeoKeywords).HasMaxLength(4000);

                entity.Property(e => e.SeoName)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SeoTitle).HasMaxLength(4000);

                entity.Property(e => e.SetAttributeData).HasColumnType("ntext");

                entity.Property(e => e.Source).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Tags).HasMaxLength(500);

                entity.Property(e => e.Template).HasMaxLength(250);

                entity.Property(e => e.Thumbnail).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(4000);

                entity.Property(e => e.Unit).HasMaxLength(50);

                entity.HasOne(d => d.SetAttribute)
                    .WithMany(p => p.MixProduct)
                    .HasForeignKey(d => d.SetAttributeId)
                    .HasConstraintName("FK_mix_product_mix_set_attribute");

                entity.HasOne(d => d.SpecificultureNavigation)
                    .WithMany(p => p.MixProduct)
                    .HasPrincipalKey(p => p.Specificulture)
                    .HasForeignKey(d => d.Specificulture)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Product_Mix_Culture");
            });

            modelBuilder.Entity<MixProductMedia>(entity =>
            {
                entity.HasKey(e => new { e.MediaId, e.ProductId, e.Specificulture });

                entity.ToTable("mix_product_media");

                entity.HasIndex(e => new { e.MediaId, e.Specificulture });

                entity.HasIndex(e => new { e.ProductId, e.Specificulture });

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MixMedia)
                    .WithMany(p => p.MixProductMedia)
                    .HasForeignKey(d => new { d.MediaId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mix_product_media_mix_media");

                entity.HasOne(d => d.MixProduct)
                    .WithMany(p => p.MixProductMedia)
                    .HasForeignKey(d => new { d.ProductId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mix_product_media_mix_product");
            });

            modelBuilder.Entity<MixProductModule>(entity =>
            {
                entity.HasKey(e => new { e.ModuleId, e.ProductId, e.Specificulture });

                entity.ToTable("mix_product_module");

                entity.HasIndex(e => new { e.ModuleId, e.Specificulture });

                entity.HasIndex(e => new { e.ProductId, e.Specificulture });

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Description).HasMaxLength(250);

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.HasOne(d => d.MixModule)
                    .WithMany(p => p.MixProductModule)
                    .HasForeignKey(d => new { d.ModuleId, d.Specificulture })
                    .HasConstraintName("FK_Mix_Product_Module_Mix_Module1");

                entity.HasOne(d => d.MixProduct)
                    .WithMany(p => p.MixProductModule)
                    .HasForeignKey(d => new { d.ProductId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Mix_Product_Module_Mix_Product");
            });

            modelBuilder.Entity<MixRelatedArticle>(entity =>
            {
                entity.HasKey(e => new { e.SourceId, e.DestinationId, e.Specificulture });

                entity.ToTable("mix_related_article");

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(450);

                entity.Property(e => e.Image).HasMaxLength(450);

                entity.HasOne(d => d.MixArticle)
                    .WithMany(p => p.MixRelatedArticleMixArticle)
                    .HasForeignKey(d => new { d.DestinationId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mix_related_article_mix_article1");

                entity.HasOne(d => d.S)
                    .WithMany(p => p.MixRelatedArticleS)
                    .HasForeignKey(d => new { d.SourceId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mix_related_article_mix_article");
            });

            modelBuilder.Entity<MixRelatedProduct>(entity =>
            {
                entity.HasKey(e => new { e.SourceId, e.DestinationId, e.Specificulture });

                entity.ToTable("mix_related_product");

                entity.HasIndex(e => new { e.DestinationId, e.Specificulture });

                entity.HasIndex(e => new { e.SourceId, e.Specificulture });

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(450);

                entity.Property(e => e.Image).HasMaxLength(450);

                entity.HasOne(d => d.MixProduct)
                    .WithMany(p => p.MixRelatedProductMixProduct)
                    .HasForeignKey(d => new { d.DestinationId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mix_related_product_mix_product1");

                entity.HasOne(d => d.S)
                    .WithMany(p => p.MixRelatedProductS)
                    .HasForeignKey(d => new { d.SourceId, d.Specificulture })
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mix_related_product_mix_product");
            });

            modelBuilder.Entity<MixSetAttribute>(entity =>
            {
                entity.ToTable("mix_set_attribute");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.CreatedBy).HasMaxLength(50);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(350);

                entity.Property(e => e.Fields).IsRequired();

                entity.Property(e => e.Title).HasMaxLength(50);
            });

            modelBuilder.Entity<MixTemplate>(entity =>
            {
                entity.ToTable("mix_template");

                entity.HasIndex(e => e.ThemeId)
                    .HasName("IX_mix_template_file_TemplateId");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnType("ntext");

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Extension)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FileFolder)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.FileName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.FolderType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastModified).HasColumnType("datetime");

                entity.Property(e => e.MobileContent).HasColumnType("ntext");

                entity.Property(e => e.ModifiedBy).HasMaxLength(250);

                entity.Property(e => e.Scripts).HasColumnType("ntext");

                entity.Property(e => e.SpaContent).HasColumnType("ntext");

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Styles).HasColumnType("ntext");

                entity.Property(e => e.ThemeName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.HasOne(d => d.Theme)
                    .WithMany(p => p.MixTemplate)
                    .HasForeignKey(d => d.ThemeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_mix_template_mix_theme");
            });

            modelBuilder.Entity<MixTheme>(entity =>
            {
                entity.ToTable("mix_theme");

                entity.Property(e => e.CreatedBy)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Image).HasMaxLength(250);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.PreviewUrl).HasMaxLength(450);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<MixUrlAlias>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.Specificulture });

                entity.ToTable("mix_url_alias");

                entity.HasIndex(e => e.Specificulture);

                entity.Property(e => e.Specificulture).HasMaxLength(10);

                entity.Property(e => e.Alias).HasMaxLength(250);

                entity.Property(e => e.CreatedDateTime).HasColumnType("datetime");

                entity.Property(e => e.Description).HasMaxLength(4000);

                entity.Property(e => e.SourceId).HasMaxLength(250);

                entity.Property(e => e.Status).HasDefaultValueSql("((1))");

                entity.Property(e => e.Type).HasDefaultValueSql("('0')");

                entity.HasOne(d => d.SpecificultureNavigation)
                    .WithMany(p => p.MixUrlAlias)
                    .HasPrincipalKey(p => p.Specificulture)
                    .HasForeignKey(d => d.Specificulture)
                    .HasConstraintName("FK_Mix_Url_Alias_Mix_Culture");
            });
        }
    }
}
