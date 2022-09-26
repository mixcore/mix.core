using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Database.EntityConfigurations.Base;
using Mix.Database.EntityConfigurations.MYSQL;
using Mix.Heart.Enums;
using Mix.Universal.Lib.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Universal.Lib.EntityConfigurations
{
    public class PortalAppConfiguration : EntityBaseConfiguration<PortalApp, int, MySqlDatabaseConstants>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            string key = $"PK_{typeof(PortalApp).Name}";
            builder.ToTable("portalApp");
            builder.HasKey(e => new { e.Id })
                   .HasName(key);
            builder.Property(e => e.Status)
               .IsRequired()
               .HasConversion(new EnumToStringConverter<MixContentStatus>())
               .HasColumnType($"varchar(50)");
        }
    }
}
