﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary.Interfaces
{
    public interface INoSqlStorage<TEntity>
    {
        Task<TEntity> Add(TEntity entity);

        Task<TEntity> Get(string rowKey, string partitionKey);

        Task Delete(string rowKey, string partitionKey);

        Task<TEntity> Update(TEntity entity);

        IQueryable<TEntity> All();

        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> query);


    }
}
