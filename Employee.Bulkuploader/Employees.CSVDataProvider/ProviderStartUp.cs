using Employees.CSVDataProvider.Interfaces;
using Employees.CSVDataProvider.Providers;
using Employees.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Employees.CSVDataProvider
{
    public static class ProviderStartUp
    {
        /// <summary>
        ///Extension Method to register provider specific Bindings
        /// </summary>
        /// <param name="services">Services</param>
        /// <param name="configuration">Configuration</param>
        public static void RegisterProviders(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEmployeesDataProvider, EmployeesDataProvider>();
            services.AddTransient<ITableStorageConnector, TableStorageConnector>();
        }
    }
}
