using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Cms.Lib.Services;
using System;
using System.Linq;
using System.Reflection;
using Mix.Cms.Lib.Extensions;
using MySqlConnector;

namespace Mix.Cms.Lib.Models.Cms
{
    public partial class MySqlMixCmsContext : MixCmsContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationDbContext" /> class.
        /// </summary>
        /// <param name="options">The options.</param>
        public MySqlMixCmsContext(DbContextOptions<MixCmsContext> options)
                    : base(options)
        {
        }

        public MySqlMixCmsContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyAllConfigurationsFromNamespace(
                this.GetType().Assembly, 
                "Mix.Cms.Lib.Models.EntityConfigurations.MySQL");
            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}