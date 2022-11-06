using Mix.Database.Services;
using System.Linq;
using System.Reflection;

namespace Mix.Database.Extensions
{
    public static class ModelBuilderExtensions
    {
        public static ModelBuilder ApplyAllConfigurations(
        this ModelBuilder modelBuilder, DatabaseService databaseService, Assembly assembly, string ns)
        {
            var applyGenericMethod = typeof(ModelBuilder)
            .GetMethods(BindingFlags.Instance | BindingFlags.Public)
            .Single(m => m.Name == nameof(ModelBuilder.ApplyConfiguration)
            && m.GetParameters().Count() == 1
            && m.GetParameters().Single().ParameterType.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>));
            foreach (var type in assembly.GetTypes()
                .Where(c => c.IsClass && !c.IsAbstract && !c.ContainsGenericParameters
                    && c.Namespace == ns
                ))
            {
                foreach (var iface in type.GetInterfaces())
                {
                    if (iface.IsConstructedGenericType && iface.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
                    {
                        var applyConcreteMethod = applyGenericMethod.MakeGenericMethod(iface.GenericTypeArguments[0]);
                        applyConcreteMethod.Invoke(modelBuilder, new object[] { Activator.CreateInstance(type, databaseService) });
                        break;
                    }
                }
            }
            return modelBuilder;
        }
    }
}
