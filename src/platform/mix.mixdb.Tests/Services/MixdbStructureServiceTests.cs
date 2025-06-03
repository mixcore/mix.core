using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Mix.Constant.Constants;
using Mix.Constant.Enums;
using Mix.Database.Base;
using Mix.Database.Entities.Cms;
using Mix.Database.Services.MixGlobalSettings;
using Mix.Heart.Constants;
using Mix.Heart.Enums;
using Mix.Heart.Exceptions;
using Mix.Heart.Extensions;
using Mix.Heart.Helpers;
using Mix.Heart.Services;
using Mix.Heart.UnitOfWork;
using Mix.Lib.Extensions;
using Mix.Lib.Interfaces;
using Mix.Mixdb.Interfaces;
using Mix.Mixdb.Services;
using Mix.Mixdb.ViewModels;
using Mix.RepoDb.Models;
using Mix.RepoDb.Repositories;
using Mix.Service.Interfaces;
using Mix.Service.Services;
using Newtonsoft.Json.Linq;
using RepoDb.Interfaces;
using System.Dynamic;
using Xunit;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Mix.Mixdb.Tests.Services
{
    /// <summary>
    /// Unit tests for MixdbStructureService
    /// </summary>
    public class MixdbStructureServiceTests
    {
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<UnitOfWorkInfo<MixCmsContext>> _mockUow;
        private readonly Mock<DatabaseService> _mockDatabaseService;
        private readonly Mock<IMixMemoryCacheService> _mockMemoryCache;
        private readonly Mock<MixCacheService> _mockCacheService;
        private readonly Mock<IMixTenantService> _mockTenantService;
        private readonly Mock<ICache> _mockCache;
        private readonly Mock<ILogger<MixdbStructureService>> _mockLogger;
        private readonly Mock<IMixdbStructureService> _mockDbStructureService;
        private readonly MixdbStructureService _service;

        public MixdbStructureServiceTests()
        {
            _mockConfiguration = new Mock<IConfiguration>();
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockUow = new Mock<UnitOfWorkInfo<MixCmsContext>>();
            _mockDatabaseService = new Mock<DatabaseService>();
            _mockMemoryCache = new Mock<IMixMemoryCacheService>();
            _mockCacheService = new Mock<MixCacheService>();
            _mockTenantService = new Mock<IMixTenantService>();
            _mockCache = new Mock<ICache>();
            _mockLogger = new Mock<ILogger<MixdbStructureService>>();
            _mockDbStructureService = new Mock<IMixdbStructureService>();

            // Setup default configuration
            _mockConfiguration.Setup(x => x.AesKey()).Returns("test-key");
            _mockDatabaseService.Setup(x => x.DatabaseProvider).Returns(MixDatabaseProvider.SQLSERVER);
            _mockDatabaseService.Setup(x => x.GetConnectionString(It.IsAny<string>()))
                .Returns("test-connection-string");

            // Setup default tenant
            _mockTenantService.Setup(x => x.CurrentTenant).Returns(new MixTenantSystemModel { Id = 1 });

            // Create service instance
            _service = new MixdbStructureService(
                _mockConfiguration.Object,
                _mockHttpContextAccessor.Object,
                _mockUow.Object,
                _mockDatabaseService.Object,
                _mockMemoryCache.Object,
                _mockCacheService.Object,
                _mockTenantService.Object,
                _mockCache.Object,
                new List<IMixdbStructureService> { _mockDbStructureService.Object },
                _mockLogger.Object);
        }

        #region MigrateInitNewDbContextDatabases Tests

        [Fact]
        public async Task MigrateInitNewDbContextDatabases_WithValidContext_ShouldSucceed()
        {
            // Arrange
            var dbContext = new MixDatabaseContext
            {
                SystemName = "test-context",
                ConnectionString = "test-connection-string",
                DatabaseProvider = MixDatabaseProvider.SQLSERVER,
                NamingConvention = MixDatabaseNamingConvention.SnakeCase
            };

            var mockFileContent = new JObject
            {
                ["databases"] = new JArray
                {
                    new JObject
                    {
                        ["SystemName"] = "test-db",
                        ["DisplayName"] = "Test Database"
                    }
                },
                ["columns"] = new JArray()
            };

            _mockMemoryCache.Setup(x => x.TryGetValueAsync(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, Task<MixDbDatabaseViewModel>>>()))
                .ReturnsAsync((MixDbDatabaseViewModel)null);

            _mockDbStructureService.Setup(x => x.ExecuteCommand(It.IsAny<string>()))
                .ReturnsAsync(1);

            // Act
            await _service.MigrateInitNewDbContextDatabases(dbContext);

            // Assert
            _mockDbStructureService.Verify(x => x.Migrate(It.IsAny<MixDbDatabaseViewModel>(), It.IsAny<MixDatabaseProvider>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task MigrateInitNewDbContextDatabases_WithNullContext_ShouldThrow()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() =>
                _service.MigrateInitNewDbContextDatabases(null));
        }

        [Fact]
        public async Task MigrateInitNewDbContextDatabases_WithInvalidContext_ShouldThrow()
        {
            // Arrange
            var dbContext = new MixDatabaseContext
            {
                SystemName = "",
                ConnectionString = ""
            };

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.MigrateInitNewDbContextDatabases(dbContext));
        }

        #endregion

        #region MigrateSystemDatabases Tests

        [Fact]
        public async Task MigrateSystemDatabases_WithValidData_ShouldSucceed()
        {
            // Arrange
            var mockFileContent = new JObject
            {
                ["databases"] = new JArray
                {
                    new JObject
                    {
                        ["SystemName"] = "system-db",
                        ["DisplayName"] = "System Database"
                    }
                },
                ["columns"] = new JArray()
            };

            _mockUow.Setup(x => x.DbContext.MixDatabaseContext)
                .Returns(new List<MixDatabaseContext>().AsQueryable());

            _mockDbStructureService.Setup(x => x.Migrate(It.IsAny<MixDbDatabaseViewModel>(), It.IsAny<MixDatabaseProvider>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.MigrateSystemDatabases();

            // Assert
            _mockDbStructureService.Verify(x => x.Migrate(It.IsAny<MixDbDatabaseViewModel>(), It.IsAny<MixDatabaseProvider>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion

        #region GetMixDatabase Tests

        [Fact]
        public async Task GetMixDatabase_WithValidTableName_ShouldReturnDatabase()
        {
            // Arrange
            var tableName = "test-table";
            var expectedDb = new MixDbDatabaseViewModel(new MixDatabase(), _mockUow.Object);

            _mockMemoryCache.Setup(x => x.TryGetValueAsync(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, Task<MixDbDatabaseViewModel>>>()))
                .ReturnsAsync(expectedDb);

            // Act
            var result = await _service.GetMixDatabase(tableName);

            // Assert
            Assert.Equal(expectedDb, result);
        }

        [Fact]
        public async Task GetMixDatabase_WithInvalidTableName_ShouldThrow()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.GetMixDatabase(""));
        }

        [Fact]
        public async Task GetMixDatabase_WithNonExistentTable_ShouldThrow()
        {
            // Arrange
            _mockMemoryCache.Setup(x => x.TryGetValueAsync(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, Task<MixDbDatabaseViewModel>>>()))
                .ReturnsAsync((MixDbDatabaseViewModel)null);

            // Act & Assert
            await Assert.ThrowsAsync<MixException>(() =>
                _service.GetMixDatabase("non-existent-table"));
        }

        #endregion

        #region ExecuteCommand Tests

        [Fact]
        public async Task ExecuteCommand_WithValidCommand_ShouldSucceed()
        {
            // Arrange
            var commandText = "SELECT 1";
            _mockDbStructureService.Setup(x => x.ExecuteCommand(commandText))
                .ReturnsAsync(1);

            // Act
            await _service.ExecuteCommand(commandText, CancellationToken.None);

            // Assert
            _mockDbStructureService.Verify(x => x.ExecuteCommand(commandText), Times.Once);
        }

        [Fact]
        public async Task ExecuteCommand_WithEmptyCommand_ShouldThrow()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.ExecuteCommand("", CancellationToken.None));
        }

        #endregion

        #region MigrateDatabase Tests

        [Fact]
        public async Task MigrateDatabase_WithValidDatabase_ShouldSucceed()
        {
            // Arrange
            var databaseName = "test-db";
            var mockDb = new MixDbDatabaseViewModel(new MixDatabase(), _mockUow.Object);

            _mockMemoryCache.Setup(x => x.TryGetValueAsync(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, Task<MixDbDatabaseViewModel>>>()))
                .ReturnsAsync(mockDb);

            _mockDbStructureService.Setup(x => x.Migrate(It.IsAny<MixDbDatabaseViewModel>(), It.IsAny<MixDatabaseProvider>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.MigrateDatabase(databaseName);

            // Assert
            _mockDbStructureService.Verify(x => x.Migrate(It.IsAny<MixDbDatabaseViewModel>(), It.IsAny<MixDatabaseProvider>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task MigrateDatabase_WithInvalidDatabase_ShouldThrow()
        {
            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() =>
                _service.MigrateDatabase(""));
        }

        #endregion

        #region Column Operations Tests

        [Fact]
        public async Task AlterColumn_WithValidColumn_ShouldSucceed()
        {
            // Arrange
            var column = new MixdbDatabaseColumnViewModel
            {
                MixDatabaseName = "test-db"
            };
            var mockDb = new MixDbDatabaseViewModel(new MixDatabase(), _mockUow.Object);

            _mockMemoryCache.Setup(x => x.TryGetValueAsync(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, Task<MixDbDatabaseViewModel>>>()))
                .ReturnsAsync(mockDb);

            _mockDbStructureService.Setup(x => x.AlterColumn(It.IsAny<MixDbDatabaseViewModel>(), It.IsAny<MixdbDatabaseColumnViewModel>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.AlterColumn(column, false);

            // Assert
            _mockDbStructureService.Verify(x => x.AlterColumn(It.IsAny<MixDbDatabaseViewModel>(), It.IsAny<MixdbDatabaseColumnViewModel>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task AddColumn_WithValidColumn_ShouldSucceed()
        {
            // Arrange
            var column = new MixdbDatabaseColumnViewModel
            {
                MixDatabaseName = "test-db"
            };
            var mockDb = new MixDbDatabaseViewModel(new MixDatabase(), _mockUow.Object);

            _mockMemoryCache.Setup(x => x.TryGetValueAsync(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, Task<MixDbDatabaseViewModel>>>()))
                .ReturnsAsync(mockDb);

            _mockDbStructureService.Setup(x => x.AddColumn(It.IsAny<MixDbDatabaseViewModel>(), It.IsAny<MixdbDatabaseColumnViewModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.AddColumn(column);

            // Assert
            _mockDbStructureService.Verify(x => x.AddColumn(It.IsAny<MixDbDatabaseViewModel>(), It.IsAny<MixdbDatabaseColumnViewModel>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DropColumn_WithValidColumn_ShouldSucceed()
        {
            // Arrange
            var column = new MixdbDatabaseColumnViewModel
            {
                MixDatabaseName = "test-db"
            };
            var mockDb = new MixDbDatabaseViewModel(new MixDatabase(), _mockUow.Object);

            _mockMemoryCache.Setup(x => x.TryGetValueAsync(It.IsAny<string>(), It.IsAny<Func<ICacheEntry, Task<MixDbDatabaseViewModel>>>()))
                .ReturnsAsync(mockDb);

            _mockDbStructureService.Setup(x => x.DropColumn(It.IsAny<MixDbDatabaseViewModel>(), It.IsAny<MixdbDatabaseColumnViewModel>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DropColumn(column);

            // Assert
            _mockDbStructureService.Verify(x => x.DropColumn(It.IsAny<MixDbDatabaseViewModel>(), It.IsAny<MixdbDatabaseColumnViewModel>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        #endregion
    }
}