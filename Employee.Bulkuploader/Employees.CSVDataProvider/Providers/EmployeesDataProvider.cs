using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using Employees.CSVDataProvider.Entities;
using Employees.CSVDataProvider.Interfaces;
using Employees.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Employees.CSVDataProvider.Providers
{
    /// <summary>
    /// Employees Data Provider 
    /// </summary>
    public class EmployeesDataProvider: IEmployeesDataProvider
    {
        private readonly IConfiguration _configuration;
        private readonly ITableStorageConnector _tableStorageConnector;
        private readonly ILogger<EmployeesDataProvider> _logger;
        private const string EmployeesTableName = "Employees";

        /// <summary>
        /// Constructor for Employees Data Provider
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="configuration"></param>
        /// <param name="tableStorageConnector"></param>
        public EmployeesDataProvider(ILogger<EmployeesDataProvider> logger,
            IConfiguration configuration, 
            ITableStorageConnector tableStorageConnector)
        {
            _logger = logger;
            _configuration = configuration;
            _tableStorageConnector = tableStorageConnector;
        }

        /// <summary>
        /// Get the List of Employees From CSV File
        /// </summary>
        /// <returns>List of Employees</returns>
        public List<EmployeeDomainModel> Get()
        {
            _logger.LogInformation($"Getting Employees From CSV File");

            List<EmployeeDomainModel> employees = null ;

            var fileName = _configuration["EmployeesData:FileName"];

            _logger.LogInformation($"CSV File Name:{fileName}");

            // Employees Data file Should be embedded as resource file in assembly
            var assembly = Assembly.GetAssembly(typeof(ProviderStartUp));

            // Get the List of resource files embedded in assembly 
            var resources = assembly.GetManifestResourceNames();

            // Find the employees file from the embedded resource files
            var employeesDataFile = resources.Where(x => x.ToLowerInvariant().Contains(fileName.ToLowerInvariant())).First();

            _logger.LogInformation($"Reading Employees Data From :{employeesDataFile}");

            using (var resourceStream = assembly.GetManifestResourceStream(employeesDataFile))
            {
                using (var reader = new StreamReader(resourceStream, Encoding.UTF8))
                {
                    using (var csvReader = new CsvReader(reader))
                    {
                        employees = csvReader.GetRecords<EmployeeDomainModel>().ToList();
                    }
                }
            }
            _logger.LogInformation($"Sending the employees data back.");
            return employees;
        }

        /// <summary>
        /// Upload received employees into Table Storage
        /// </summary>
        /// <param name="employees">Employee to be uploaded into Table Storage</param>
        /// <returns>Task</returns>
        public async Task Upload(List<EmployeeDomainModel> employees)
        {
            _logger.LogInformation($"Uploading Employees Into Azure Table Storage");
            var employeeEntities = new List<EmployeeEntity>();

            // If no Employee found in the received List, do nothing.
            if (employees == null || !employees.Any())
            {
                _logger.LogInformation($"No Employees Received to Upload");
                return;
            }
            
            // Convert  Employees Domain Model into Table Entity
            employees.ToList().ForEach(employee => {
                _logger.LogInformation($"Uploading {employee.ToString()}");
                employeeEntities.Add(new EmployeeEntity
                {
                    PartitionKey = employee.Tenant,
                    RowKey = employee.EmployeeId,
                    Email = employee.Email,
                    Name = employee.Name,
                    CreatedOn = employee.ImportedOn
                });
            });

            // Upload received Employees data into Azure table storage
            await _tableStorageConnector.BulkUpsert<EmployeeEntity>(EmployeesTableName, employeeEntities);
        }
    }
}
