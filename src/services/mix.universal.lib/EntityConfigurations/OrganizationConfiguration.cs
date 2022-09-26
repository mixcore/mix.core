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
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Mix.Universal.Lib.EntityConfigurations
{
    public class OrganizationConfiguration : EntityBaseConfiguration<Organization, int, MySqlDatabaseConstants>
    {
         
        public override void Configure(EntityTypeBuilder<Organization> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.IsDeleted)
                .HasColumnName("IsDeleted");

            builder.Property(e => e.Description)
                .HasColumnName("Description")
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            
            builder.Property(e => e.Endpoint)
                .HasColumnName("Endpoint")
                .HasColumnType($"{Config.String}{Config.MediumLength}");
            
            builder.Property(e => e.Title)
                .HasColumnName("Title")
                .HasColumnType($"{Config.String}{Config.MediumLength}");
        }
    }
}
