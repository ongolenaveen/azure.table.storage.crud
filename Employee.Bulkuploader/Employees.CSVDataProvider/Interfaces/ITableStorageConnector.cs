using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace Employees.CSVDataProvider.Interfaces
{
    /// <summary>
    /// This Interface contains all the needed functions to interact with Azure Table Storage
    /// </summary>
    public interface ITableStorageConnector
    {
        /// <summary>
        /// Get Entity from the given table name for a given partition and row key
        /// </summary>
        /// <typeparam name="TResponse">Response Type</typeparam>
        /// <param name="tableName">Azure Table Name</param>
        /// <param name="partitionId">Partition Id in the Table</param>
        /// <param name="rowId">Row Key in the Partition</param>
        /// <returns>Response Received From Azure</returns>
        Task<TResponse> Get<TResponse>(string tableName, string partitionId, string rowId) where TResponse : TableEntity;

        /// <summary>
        /// Insert/Update Entity into the Table
        /// </summary>
        /// <typeparam name="TEntity">Entity To be Inserted</typeparam>
        /// <param name="tableName">Azure Table Name</param>
        /// <param name="request">Request</param>
        /// <returns>Response Received From Azure</returns>
        Task<TEntity> Upsert<TEntity>(string tableName, TEntity request) where TEntity : TableEntity;

        /// <summary>
        /// Insert/Update Data into azure table storage as Batch
        /// </summary>
        /// <typeparam name="entities">Entites to be Upserted</typeparam>
        /// <param name="tableName">Table Name</param>
        /// <param name="partitionId">Partition Id</param>
        /// <param name="rowId">Row Id</param>
        /// <returns>Inserted Result</returns>
        Task BulkUpsert<TEntity>(string tableName, IEnumerable<TEntity> entities) where TEntity : TableEntity;
    }
}
