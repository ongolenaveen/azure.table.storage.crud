using System.IO;
using Employees.Domain;
using Employees.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Employees.UploaderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new HostBuilder()
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.SetBasePath(Directory.GetCurrentDirectory());
                    config.AddJsonFile("appsettings.json", true);
                    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",optional: true);
                    if (args != null) config.AddCommandLine(args);
                })
                .UseEnvironment(Environments.Development)
                .ConfigureServices((hostingContext, services) =>
                {
                    Startup.ConfigureServices(hostingContext, services);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration);
                    logging.AddConsole();
                }).Build();

            // Get the List of Employees from data provider ans upload them into Table storage
            var employeesService = builder.Services.GetRequiredService<IEmployeesService<EmployeeDomainModel>>();

            // Get the List of Employees from data provider
            var employees = employeesService.Get();

            // Upload employees into Table storage
            Task.Run(() => employeesService.Upload(employees)).Wait();
            builder.Run();
        }
    }
}
