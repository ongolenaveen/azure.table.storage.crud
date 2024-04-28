using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employees.Services
{
    /// <summary>
    /// This Interface has all the functions needed to get employee details and upload them from/to data provider
    /// </summary>
    public interface IEmployeesService<T>
    {
        /// <summary>
        ///  Get the List of Employees from Data Provider
        /// </summary>
        /// <returns>List of Employees </returns>
        List<T> Get();


        /// <summary>
        /// Upload the employees into data provider
        /// </summary>
        /// <param name="employees">List of Employees to be uploaded into Data provider</param>
        /// <returns>Task</returns>
        Task Upload(List<T> employees);
    }
}
