using Employees.CSVDataProvider;
using Employees.Domain;
using Employees.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Employees.UploaderApp
{
    public class Startup
    {
        /// <summary>
        /// Extension method to register all the service bindings needed for App
        /// </summary>
        /// <param name="context">Host Builder Context</param>
        /// <param name="services">Service Collection</param>
        public static void ConfigureServices(HostBuilderContext context,IServiceCollection services)
        {
            services.AddTransient<IEmployeesService<EmployeeDomainModel>, EmployeesService>();
            services.RegisterProviders(context.Configuration);
        }
    }
}
