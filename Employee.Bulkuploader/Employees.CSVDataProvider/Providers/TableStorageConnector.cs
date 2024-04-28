using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Employees.CSVDataProvider.Interfaces;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Employees.CSVDataProvider.Providers
{
    /// <summary>
    /// This class contains all the needed functions to interact with Azure Table Storage
    /// </summary>
    public class TableStorageConnector : ITableStorageConnector
    {
        protected readonly IConfiguration _config;
        private readonly CloudStorageAccount _storageAccount;
        private readonly string _connectionString;
        private readonly ILogger<TableStorageConnector> _logger;

        public TableStorageConnector(IConfiguration config, 
            ILogger<TableStorageConnector> logger )
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            // Retrieve the storage account from the connection string.
            _connectionString = _config["ConnectionStrings:Storage"];
            _storageAccount = CloudStorageAccount.Parse(_connectionString);
        }

        /// <summary>
        /// Get Data from azure table storage from the received table for given Partition Id and rowid
        /// </summary>
        /// <typeparam name="TResponse">Response Received From Azure Table Storage</typeparam>
        /// <param name="tableName">Table Name</param>
        /// <param name="partitionId">Partition Id</param>
        /// <param name="rowId">Row Id</param>
        /// <returns>Received Data From Azure</returns>
        public async Task<TResponse> Get<TResponse>(string tableName, string partitionId, string rowId) where TResponse : TableEntity
        {
            // Create the table client.
            var tableClient = _storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            var table = tableClient.GetTableReference(tableName);

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            // Create a retrieve operation that takes a customer entity.
            var retrieveOperation = TableOperation.Retrieve<TResponse>(partitionId, rowId);

            // Execute the retrieve operation.
            var retrievedResult = await table.ExecuteAsync(retrieveOperation);

            return (retrievedResult.Result != null) ? retrievedResult.Result as TResponse : null;
        }

        /// <summary>
        /// Insert/Update Data into azure table storage
        /// </summary>
        /// <typeparam name="TableEntity">Entity to be Upserted</typeparam>
        /// <param name="tableName">Table Name</param>
        /// <param name="partitionId">Partition Id</param>
        /// <param name="rowId">Row Id</param>
        /// <returns>Inserted Result</returns>
        public async Task<TEntity> Upsert<TEntity>(string tableName, TEntity entity) where TEntity : TableEntity
        {
            // Create the table client.
            var tableClient = _storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            var table = tableClient.GetTableReference(tableName);

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            // Create the TableOperation object that inserts the customer entity
            var insertOrReplaceOperation = TableOperation.InsertOrReplace(entity);

            // Execute the Insert Or Replace Operation Operation.
            var insertedResult = await table.ExecuteAsync(insertOrReplaceOperation);

            return (insertedResult.Result != null) ? insertedResult.Result as TEntity : null;
        }

        /// <summary>
        /// Insert/Update Data into azure table storage as Batch
        /// </summary>
        /// <typeparam name="entities">Entites to be Upserted</typeparam>
        /// <param name="tableName">Table Name</param>
        /// <param name="partitionId">Partition Id</param>
        /// <param name="rowId">Row Id</param>
        /// <returns>Inserted Result</returns>
        public async Task BulkUpsert<TEntity>(string tableName, IEnumerable<TEntity> entities) where TEntity : TableEntity
        {
            // Create the table client.
            var tableClient = _storageAccount.CreateCloudTableClient();

            // Retrieve a reference to the table.
            var table = tableClient.GetTableReference(tableName);

            // Create the table if it doesn't exist.
            table.CreateIfNotExists();

            // If there are no entities in the list do nothing
            if (entities == null && !entities.Any())
                return;

            // Group all the entities based on Partian Key
            var groups = from entity in entities group entity by entity.PartitionKey;
            foreach (var group in groups)
            {
                // Upsert all the entities related to a partition in Batch
                var batch = new TableBatchOperation();
                foreach (var entity in group)
                {
                    batch.InsertOrReplace(entity);
                }
                var response = await table.ExecuteBatchAsync(batch);
            }

            // Return the response back
            return;
        }
    }
}
