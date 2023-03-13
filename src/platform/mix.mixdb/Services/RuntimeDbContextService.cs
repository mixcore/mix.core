﻿using Microsoft.AspNetCore.Http;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.Sqlite.Storage.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Diagnostics.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Scaffolding.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Storage.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Mix.Constant.Constants;
using Mix.Database.Entities.Cms;
using Mix.Database.Services;
using Mix.Heart.Enums;
using Mix.Service.Services;
using Npgsql.EntityFrameworkCore.PostgreSQL.Diagnostics.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Scaffolding.Internal;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal;
using Pomelo.EntityFrameworkCore.MySql.Diagnostics.Internal;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Runtime.Loader;
// Ref: https://medium.com/@zaikinsr/roslyn-ef-core-runtime-dbcontext-constructing-285a9d67bc87
namespace Mix.Mixdb.Services
{
    public class RuntimeDbContextService : IDisposable
    {
        protected IHttpContextAccessor _httpContextAccessor;
        private AssemblyLoadContext _assemblyLoadContext;
        private Assembly _assembly;
        private Type _dbContextType;
        private readonly DatabaseService _databaseService;
        public RuntimeDbContextService(IHttpContextAccessor httpContextAccessor, DatabaseService databaseService)
        {
            _httpContextAccessor = httpContextAccessor;
            _databaseService = databaseService;
            LoadDbContextAssembly();
        }

        public void Reload()
        {
            _assemblyLoadContext.Unload();
            LoadDbContextAssembly();
        }

        #region Create Dynamic Context


        public void LoadDbContextAssembly()
        {
            if (!string.IsNullOrEmpty(_databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION)))
            {
                var sourceFiles = CreateDynamicDbContext();
                using var peStream = new MemoryStream();
                var enableLazyLoading = false;
                var result = GenerateCode(sourceFiles, enableLazyLoading).Emit(peStream);

                if (!result.Success)
                {
                    var failures = result.Diagnostics
                        .Where(diagnostic => diagnostic.IsWarningAsError ||
                                             diagnostic.Severity == DiagnosticSeverity.Error);

                    var error = failures.FirstOrDefault();
                    //throw new Exception($"{error?.Id}: {error?.GetMessage()}");
                    MixService.LogException(new Exception($"{error?.Id}: {error?.GetMessage()}"));
                }
                _assemblyLoadContext = new AssemblyLoadContext("DataContext", isCollectible: true);

                peStream.Seek(0, SeekOrigin.Begin);
                _assembly = _assemblyLoadContext.LoadFromStream(peStream);
            }
        }

        public DbContext GetMixDatabaseDbContext()
        {
            if (!string.IsNullOrEmpty(_databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION)))
            {
                if (_assembly == null)
                {
                    LoadDbContextAssembly();
                }
                if (_assembly != null)
                {

                    _dbContextType = _assembly.GetType("TypedDataContext.Context.DataContext");
                    _ = _dbContextType ?? throw new Exception("DataContext type not found");

                    var constr = _dbContextType.GetConstructor(Type.EmptyTypes);
                    _ = constr ?? throw new Exception("DataContext ctor not found");
                    var ctx = (DbContext)constr.Invoke(null);
                    ctx.Database.EnsureCreated();
                    return ctx;
                }
            }
            return default;
        }

        public List<string> CreateDynamicDbContext()
        {
            var sourceFiles = new List<string>();
            using var _cmsContext = new MixCmsContext(_databaseService);
            var scaffolder = CreateScaffolder();
            var databaseNames = _cmsContext.MixDatabase.Select(m => m.SystemName.ToLower()).ToList();

            if (databaseNames.Count == 0)
            {
                // To load an empty dbcontext, add a table that does not exist. 
                databaseNames.Add("EmptyTable");
            }

            var dbOpts = new DatabaseModelFactoryOptions(databaseNames);
            var modelOpts = new ModelReverseEngineerOptions()
            {
                NoPluralize = true
            };
            var codeGenOpts = new ModelCodeGenerationOptions()
            {
                RootNamespace = "TypedDataContext",
                ContextName = "DataContext",
                ContextNamespace = "TypedDataContext.Context",
                //ModelNamespace = "TypedDataContext.Models",
                SuppressConnectionStringWarning = true
            };

            var scaffoldedModelSources = scaffolder.ScaffoldModel(
                _databaseService.GetConnectionString(MixConstants.CONST_MIXDB_CONNECTION),
                dbOpts,
                modelOpts,
                codeGenOpts);
            string contextFileCode = scaffoldedModelSources.ContextFile.Code;
            foreach (var item in scaffoldedModelSources.AdditionalFiles)
            {
                string name = item.Path.Substring(0, item.Path.LastIndexOf('.'));
                if (databaseNames.Any(m => string.Equals(m, name, StringComparison.OrdinalIgnoreCase)))
                {
                    ReplaceEntityNaming(databaseNames, item, ref contextFileCode);
                }
                sourceFiles.Add(item.Code);
            }
            if (_databaseService.DatabaseProvider == MixDatabaseProvider.PostgreSQL)
            {
                contextFileCode = $"using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;{contextFileCode}";
            }
            sourceFiles.Add(contextFileCode);
            return sourceFiles;
        }

        private void ReplaceEntityNaming(List<string> databaseNames, ScaffoldedFile item, ref string contextFileCode)
        {
            string name = item.Path.Substring(0, item.Path.LastIndexOf('.'));
            string newName = name.ToLower();
            contextFileCode = contextFileCode.Replace($"Entity<{name}>", $"Entity<{newName}>")
                .Replace($"DbSet<{name}>", $"DbSet<{newName}>");

            item.Path = item.Path.Replace(name, newName);
            item.Code = item.Code
                .Replace($"public {name}", $"public {newName}")
                .Replace($"class {name}", $"class {newName}")
                .Replace("byte[] CreatedDateTime", "DateTime CreatedDateTime");
        }

        [SuppressMessage("Usage", "EF1001:Internal EF Core API usage.", Justification = "We need it")]
        IReverseEngineerScaffolder CreateScaffolder()
        {
            return _databaseService.DatabaseProvider switch
            {
                MixDatabaseProvider.SQLITE => new ServiceCollection()
                .AddEntityFrameworkSqlite()
                .AddSingleton<ProviderCodeGeneratorDependencies>()
                .AddSingleton<AnnotationCodeGeneratorDependencies>()
                .AddLogging()
                .AddEntityFrameworkDesignTimeServices()
                .AddSingleton<LoggingDefinitions, SqliteLoggingDefinitions>()
                .AddSingleton<IRelationalTypeMappingSource, SqliteTypeMappingSource>()
                .AddSingleton<IAnnotationCodeGenerator, AnnotationCodeGenerator>()
                .AddSingleton<IDatabaseModelFactory, SqliteDatabaseModelFactory>()
                .AddSingleton<IProviderConfigurationCodeGenerator, SqliteCodeGenerator>()
                .AddSingleton<IScaffoldingModelFactory, RelationalScaffoldingModelFactory>()
                .AddSingleton<IReverseEngineerScaffolder, ReverseEngineerScaffolder>()
                .BuildServiceProvider()
                .GetRequiredService<IReverseEngineerScaffolder>(),

                MixDatabaseProvider.PostgreSQL => new ServiceCollection()
                .AddSingleton<ProviderCodeGeneratorDependencies>()
                .AddSingleton<AnnotationCodeGeneratorDependencies>()
                .AddEntityFrameworkNpgsql()
                .AddLogging()
                .AddEntityFrameworkDesignTimeServices()
                .AddSingleton<LoggingDefinitions, NpgsqlLoggingDefinitions>()
                .AddSingleton<IRelationalTypeMappingSource, NpgsqlTypeMappingSource>()
                .AddSingleton<IAnnotationCodeGenerator, AnnotationCodeGenerator>()
                .AddSingleton<IDatabaseModelFactory, NpgsqlDatabaseModelFactory>()
                .AddSingleton<IProviderConfigurationCodeGenerator, NpgsqlCodeGenerator>()
                .AddSingleton<IScaffoldingModelFactory, RelationalScaffoldingModelFactory>()
                .BuildServiceProvider()
                .GetRequiredService<IReverseEngineerScaffolder>(),

                MixDatabaseProvider.MySQL => new ServiceCollection()
                .AddSingleton<ProviderCodeGeneratorDependencies>()
                .AddSingleton<AnnotationCodeGeneratorDependencies>()
                .AddEntityFrameworkMySql()
                .AddLogging()
                .AddEntityFrameworkDesignTimeServices()
                .AddSingleton<LoggingDefinitions, MySqlLoggingDefinitions>()
                .AddSingleton<IRelationalTypeMappingSource, MySqlTypeMappingSource>()
                .AddSingleton<IAnnotationCodeGenerator, AnnotationCodeGenerator>()
                .AddSingleton<IDatabaseModelFactory, MySqlDatabaseModelFactory>()
                .AddSingleton<IProviderConfigurationCodeGenerator, MySqlCodeGenerator>()
                .AddSingleton<IScaffoldingModelFactory, RelationalScaffoldingModelFactory>()
                .BuildServiceProvider()
                .GetRequiredService<IReverseEngineerScaffolder>(),

                MixDatabaseProvider.SQLSERVER => new ServiceCollection()
                .AddSingleton<ProviderCodeGeneratorDependencies>()
                .AddSingleton<AnnotationCodeGeneratorDependencies>()
                .AddEntityFrameworkSqlServer()
                .AddLogging()
                .AddEntityFrameworkDesignTimeServices()
                .AddSingleton<LoggingDefinitions, SqlServerLoggingDefinitions>()
                .AddSingleton<IRelationalTypeMappingSource, SqlServerTypeMappingSource>()
                .AddSingleton<IAnnotationCodeGenerator, AnnotationCodeGenerator>()
                .AddSingleton<IDatabaseModelFactory, SqlServerDatabaseModelFactory>()
                .AddSingleton<IProviderConfigurationCodeGenerator, SqlServerCodeGenerator>()
                .AddSingleton<IScaffoldingModelFactory, RelationalScaffoldingModelFactory>()
                .BuildServiceProvider()
                .GetRequiredService<IReverseEngineerScaffolder>(),
                _ => throw new NotImplementedException()
            };
        }



        List<MetadataReference> CompilationReferences(bool enableLazyLoading)
        {
            var refs = new List<MetadataReference>();
            var referencedAssemblies = Assembly.GetExecutingAssembly().GetReferencedAssemblies();
            refs.AddRange(referencedAssemblies.Select(a => MetadataReference.CreateFromFile(Assembly.Load(a).Location)));

            refs.Add(MetadataReference.CreateFromFile(typeof(object).Assembly.Location));
            refs.Add(MetadataReference.CreateFromFile(Assembly.Load("netstandard, Version=2.0.0.0").Location));
            refs.Add(MetadataReference.CreateFromFile(typeof(System.Data.Common.DbConnection).Assembly.Location));
            refs.Add(MetadataReference.CreateFromFile(typeof(System.Linq.Expressions.Expression).Assembly.Location));

            if (enableLazyLoading)
            {
                refs.Add(MetadataReference.CreateFromFile(typeof(ProxiesExtensions).Assembly.Location));
            }

            return refs;
        }

        public CSharpCompilation GenerateCode(List<string> sourceFiles, bool enableLazyLoading)
        {
            var options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp11);

            var parsedSyntaxTrees = sourceFiles.Select(f => SyntaxFactory.ParseSyntaxTree(f, options));

            return CSharpCompilation.Create($"DataContext.dll",
                parsedSyntaxTrees,
                references: CompilationReferences(enableLazyLoading),
                options: new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary,
                    optimizationLevel: OptimizationLevel.Release,
                    assemblyIdentityComparer: DesktopAssemblyIdentityComparer.Default));
        }

        public void Dispose()
        {
            _assemblyLoadContext?.Unload();
        }

        #endregion

    }
}
