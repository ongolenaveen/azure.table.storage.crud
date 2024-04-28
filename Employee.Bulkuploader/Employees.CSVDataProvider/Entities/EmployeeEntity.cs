using System;
using Microsoft.Azure.Cosmos.Table;

namespace Employees.CSVDataProvider.Entities
{

    /// <summary>
    /// Employee Table Storage Entity
    /// </summary>
    public class EmployeeEntity : TableEntity
    {
        /// <summary>
        /// Employee Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Employee Email Address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///  Created On
        /// </summary>
        public DateTime? CreatedOn { get; set; }
    }
}
