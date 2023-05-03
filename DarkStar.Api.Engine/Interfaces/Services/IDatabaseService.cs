using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DarkStar.Api.Engine.Interfaces.Services.Base;
using DarkStar.Api.Interfaces.Entities;

namespace DarkStar.Api.Engine.Interfaces.Services;

public interface IDatabaseService : IDarkSunEngineService
{
    Task<List<TEntity>> FindAllAsync<TEntity>() where TEntity : class, IBaseEntity;
    Task<TEntity> InsertAsync<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
    Task<List<TEntity>> InsertAsync<TEntity>(List<TEntity> entities) where TEntity : class, IBaseEntity;

    Task<List<TEntity>> InsertAsync<TEntity>(List<TEntity> entities, bool enableCascade)
        where TEntity : class, IBaseEntity;

    Task<TEntity> UpdateAsync<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;
    Task<bool> DeleteAsync<TEntity>(TEntity entity) where TEntity : class, IBaseEntity;

    Task<List<TEntity>> QueryAsListAsync<TEntity>(Expression<Func<TEntity, bool>> query)
        where TEntity : class, IBaseEntity;

    Task<TEntity> QueryAsSingleAsync<TEntity>(Expression<Func<TEntity, bool>> query) where TEntity : class, IBaseEntity;
    Task<long> CountAsync<TEntity>(Expression<Func<TEntity, bool>> query) where TEntity : class, IBaseEntity;
    Task<long> CountAsync<TEntity>() where TEntity : class, IBaseEntity;
}
