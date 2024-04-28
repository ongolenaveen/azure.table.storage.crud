using System.Collections.Generic;
using System.Threading.Tasks;
using Employees.Domain;

namespace Employees.Services
{
    /// <summary>
    /// This Service has all the functions needed to get employee details and upload them from/to data provider
    /// </summary>
    public class EmployeesService:IEmployeesService<EmployeeDomainModel>
    {
        private readonly IEmployeesDataProvider _employeesDataProvider;

        /// <summary>
        /// Constrcutor For Employees Service
        /// </summary>
        /// <param name="employeeDataProvider">Employees Data Provider</param>
        public EmployeesService(IEmployeesDataProvider employeeDataProvider)
        {
            _employeesDataProvider = employeeDataProvider;
        }

        /// <summary>
        ///  Get the List of Employees from Data Provider
        /// </summary>
        /// <returns>List of Employees </returns>
        public List<EmployeeDomainModel> Get()
        {
            return _employeesDataProvider.Get();
        }

        /// <summary>
        /// Upload the employees into data provider
        /// </summary>
        /// <param name="employees">List of Employees to be uploaded into Data provider</param>
        /// <returns>Task</returns>
        public async Task Upload(List<EmployeeDomainModel> employees)
        {
            await _employeesDataProvider.Upload(employees);
        }
    }
}
