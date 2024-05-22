using Azure;
using Azure.Data.Tables;
using Azure.Data.Tables.Models;
using AzureStorageLibrary.Interfaces;
using AzureStorageLibrary.Models;
using Microsoft.Extensions.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary.Services
{
    public class TableStorage<TEntity> : INoSqlStorage<TEntity> where TEntity : class, ITableEntity, new()
    {
        private readonly TableServiceClient _cloudTableClient;
        private readonly TableItem _table;
        private readonly TableClient _tableClient;

        public TableStorage()
        {
            _cloudTableClient = new TableServiceClient(ConnectionStrings.AzureStorageConnectionString);
            //Cosmos kütüphanesi ile burada fark var. Patlayabilir

            //_cloudTableClient = new TableServiceClient(
            //new Uri(ConnectionStrings.AzureStorageConnectionString));

            _tableClient = _cloudTableClient.GetTableClient(typeof(TEntity).Name);
            _tableClient.CreateIfNotExists();

            //_table = _cloudTableClient.CreateTableIfNotExists(typeof(TEntity).Name);
            //Console.WriteLine($"The created table's name is {typeof(TEntity).Name}.");


        }

        public async Task<TEntity> Add(TEntity entity)
        {
            //await _tableClient.UpsertEntityAsync(entity);
            //return entity;


            //return _tableClient.UpsertEntity(entity) is TEntity value ? value : default(TEntity);

            try
            {
                // Debug amaçlı entity verilerini loglayın
                Console.WriteLine("Entity PartitionKey: " + entity.PartitionKey);
                Console.WriteLine("Entity RowKey: " + entity.RowKey);
                Console.WriteLine("Entity RawPaths: " + (entity as UserPicture)?.RawPaths);
                Console.WriteLine("Entity WatermarkRawPaths: " + (entity as UserPicture)?.WatermarkRawPaths);

                // Null ve boş string kontrolü yapın
                if (string.IsNullOrEmpty(entity.PartitionKey) || string.IsNullOrEmpty(entity.RowKey))
                {
                    throw new ArgumentException("PartitionKey and RowKey cannot be null or empty.");
                }

                await _tableClient.UpsertEntityAsync(entity, TableUpdateMode.Merge);
                return entity;
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"RequestFailedException: {ex.Message}");
                throw;
            }

            //var tableClient = _cloudTableClient.GetTableClient(typeof(TEntity).Name);

            //var response = await tableClient.UpsertEntityAsync(entity);

            //if (response.Status == 200)
            //{
            //    return entity;
            //}
            //else
            //{
            //    return default(TEntity);
            //}

        }

        public IQueryable<TEntity> All()
        {
            return _tableClient.Query<TEntity>().AsQueryable();
        }

        public async Task Delete(string rowKey, string partitionKey)
        {
             await _tableClient.DeleteEntityAsync(partitionKey, rowKey);
        }

        public async Task<TEntity> Get(string rowKey, string partitionKey)
        {
            return await _tableClient.GetEntityAsync<TEntity>(partitionKey, rowKey);
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            Pageable<TEntity> queryResult = _tableClient.Query<TEntity>(query);
           //return _tableClient.Query<TEntity>(query).AsQueryable();
           return queryResult.AsQueryable();
        }

        public async Task<TEntity> Update(TEntity entity)
        {
            await _tableClient.UpsertEntityAsync(entity);
            return entity;
        }
    }
}
