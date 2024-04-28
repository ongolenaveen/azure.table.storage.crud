using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employees.Domain
{
    /// <summary>
    /// Employees Data Provider 
    /// </summary>
    public interface IEmployeesDataProvider
    {
        /// <summary>
        /// Get the List of Employees From Data Provider
        /// </summary>
        /// <returns>List of Employees</returns>
        List<EmployeeDomainModel> Get();

        /// <summary>
        /// Upload List of Employees into Data Provider
        /// </summary>
        /// <param name="employees">List of Employees to be Uploaded to Data provider</param>
        /// <returns>Task</returns>
        Task Upload(List<EmployeeDomainModel> employees);
    }
}
