using Mix.Database.Services;
using System.Linq.Expressions;

namespace Mix.Database.Extensions
{
    public static class EntityTypeBuilderExtensions
    {
        public static EntityTypeBuilder ConfigueJsonColumn<T, TCol>(
           this EntityTypeBuilder<T> builder,
           Expression<Func<T, TCol>> expression,
           DatabaseService databaseService)
           where T : class
           where TCol : class
        {
            switch (databaseService.DatabaseProvider)
            {
                case MixDatabaseProvider.SQLSERVER:
                case MixDatabaseProvider.SQLITE:
                    builder.OwnsOne(expression, meta =>
                    {
                        meta.ToJson();
                    });
                    break;
                case MixDatabaseProvider.MySQL:
                case MixDatabaseProvider.PostgreSQL:
                    builder.Property(expression).HasColumnType("json").IsRequired(false);
                    break;
            }
            return builder;
        }
    }
}
