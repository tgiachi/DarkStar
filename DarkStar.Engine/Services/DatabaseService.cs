using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Attributes.Services;
using DarkStar.Api.Data.Config;
using DarkStar.Api.Engine.Data.Config;
using DarkStar.Api.Engine.Data.Config.Sections;
using DarkStar.Api.Engine.Interfaces.Services;
using DarkStar.Api.Interfaces.Entities;
using DarkStar.Database.Entities.Account;
using DarkStar.Engine.Services.Base;

using FreeSql;
using FreeSql.DataAnnotations;
using Microsoft.Extensions.Logging;

namespace DarkStar.Engine.Services;

[DarkStarEngineService("DatabaseService", 1)]
public class DatabaseService : BaseService<IDatabaseService>, IDatabaseService
{
    private IFreeSql _connectionFactory = null!;
    private readonly DirectoriesConfig _directoriesConfig;

    private readonly EngineConfig _config;

    public DatabaseService(ILogger<IDatabaseService> logger, EngineConfig engineConfig,
        DirectoriesConfig directoriesConfig) : base(logger)
    {
        _config = engineConfig;
        _directoriesConfig = directoriesConfig;
    }

    protected override async ValueTask<bool> StartAsync()
    {
        var connectionStringBuilder = new DbConnectionStringBuilder
        {
            ConnectionString = _config.Database.ConnectionString.Replace("{DATABASE_DIRECTORY}",
                Path.Join(_directoriesConfig[DirectoryNameType.Database] + Path.DirectorySeparatorChar))
        };

        if (_config.Database.RecreateDatabase)
        {
            Logger.LogInformation("Deleting database");
            await DeleteDatabaseAsync();
        }

        _connectionFactory = _config.Database.DatabaseType switch
        {
            DatabaseType.SqlLite => new FreeSqlBuilder()
                .UseConnectionString(DataType.Sqlite, connectionStringBuilder.ConnectionString)
                .UseAutoSyncStructure(true)
                .Build(),
            DatabaseType.PostgresSql => new FreeSqlBuilder()
                .UseConnectionString(DataType.PostgreSQL, connectionStringBuilder.ConnectionString)
                .UseAutoSyncStructure(true)
                .Build(),
            _ => _connectionFactory
        };

        if (_config.Database.RecreateDatabase)
        {
            Logger.LogInformation("Recreating database migrations");
            await MigrateAsync();
        }

        return true;
    }

    private ValueTask MigrateAsync()
    {
        _connectionFactory.CodeFirst.SyncStructure(GetTypesByTableAttribute());
        return ValueTask.CompletedTask;
    }

    private ValueTask DeleteDatabaseAsync()
    {
        if (_config.Database.DatabaseType == DatabaseType.SqlLite)
        {
            var connectionStringBuilder = new DbConnectionStringBuilder
            {
                ConnectionString = _config.Database.ConnectionString.Replace("{DATABASE_DIRECTORY}",
                    Path.Join(_directoriesConfig[DirectoryNameType.Database] + Path.DirectorySeparatorChar))
            };
            var databasePath = connectionStringBuilder["Data Source"]?.ToString();
            if (databasePath != null)
            {
                File.Delete(databasePath);
            }
        }

        return ValueTask.CompletedTask;
    }


    public async Task<List<TEntity>> FindAllAsync<TEntity>() where TEntity : class, IBaseEntity
    {
        using var dbConnection = _connectionFactory.GetRepository<TEntity>();
        return await dbConnection.Select.ToListAsync();
    }

    public async Task<TEntity> InsertAsync<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
    {
        using var dbConnection = _connectionFactory.GetRepository<TEntity>();
        if (entity.Id == Guid.Empty)
        {
            entity.Id = Guid.NewGuid();
        }

        Logger.LogDebug("Inserting entity {Entity}", entity.GetType().Name);
        await dbConnection.InsertAsync(entity);

        return entity;
    }

    public Task<List<TEntity>> InsertAsync<TEntity>(List<TEntity> entities) where TEntity : class, IBaseEntity
    {
        return InsertAsync(entities, false);
    }

    public async Task<List<TEntity>> InsertAsync<TEntity>(List<TEntity> entities, bool enableCascade)
        where TEntity : class, IBaseEntity
    {
        entities.ForEach(entity =>
        {
            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }
        });
        using var dbConnection = _connectionFactory.GetRepository<TEntity>();

        dbConnection.DbContextOptions.EnableCascadeSave = enableCascade;

        Logger.LogDebug("Inserting entity {Entity}", entities.GetType().Name);
        await dbConnection.InsertAsync(entities);

        return entities;
    }

    public async Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
    {
        using var dbConnection = _connectionFactory.GetRepository<TEntity>();
        entity.UpdatedAt = DateTime.UtcNow;
        Logger.LogDebug("Updating entity {Entity}", entity.GetType().Name);
        await dbConnection.UpdateAsync(entity);

        return entity;
    }

    public async Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : class, IBaseEntity
    {
        using var dbConnection = _connectionFactory.GetRepository<TEntity>();
        Logger.LogDebug("Deleting entity {Entity} with Id: {Id}", entity.GetType().Name, entity.Id);
        await dbConnection.DeleteAsync(entity);

        return true;
    }

    public async Task<List<TEntity>> QueryAsListAsync<TEntity>(Expression<Func<TEntity, bool>> query)
        where TEntity : class, IBaseEntity
    {
        using var dbConnection = _connectionFactory.GetRepository<TEntity>();
        return await dbConnection.Select.Where(query).ToListAsync();
    }

    public async Task<TEntity> QueryAsSingleAsync<TEntity>(Expression<Func<TEntity, bool>> query)
        where TEntity : class, IBaseEntity
    {
        using var dbConnection = _connectionFactory.GetRepository<TEntity>();
        return await dbConnection.Where(query).FirstAsync();
    }

    public async Task<long> CountAsync<TEntity>(Expression<Func<TEntity, bool>> query)
        where TEntity : class, IBaseEntity
    {
        using var dbConnection = _connectionFactory.GetRepository<TEntity>();

        return await dbConnection.Select.Where(query).CountAsync();
    }

    public async Task<long> CountAsync<TEntity>() where TEntity : class, IBaseEntity
    {
        using var dbConnection = _connectionFactory.GetRepository<TEntity>();

        return await dbConnection.Select.CountAsync();
    }

    public static Type[] GetTypesByTableAttribute()
    {
        var tableAssembies = new List<Type>();
        foreach (var type in Assembly.GetAssembly(typeof(AccountEntity))!.GetExportedTypes())
            foreach (var attribute in type.GetCustomAttributes())
            {
                if (attribute is TableAttribute tableAttribute)
                {
                    if (tableAttribute.DisableSyncStructure == false)
                    {
                        tableAssembies.Add(type);
                    }
                }
            }

        return tableAssembies.ToArray();
    }
}
