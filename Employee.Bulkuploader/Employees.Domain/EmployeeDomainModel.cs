using System;

namespace Employees.Domain
{
    /// <summary>
    /// Employee Domain Model
    /// </summary>
    public class EmployeeDomainModel
    {
        /// <summary>
        /// Country
        /// </summary>
        public string Country { get; set; }

        /// <summary>
        /// Sales Channel
        /// </summary>
        public string SalesChannel { get; set; }

        /// <summary>
        // Partner
        /// </summary>
        public string Partner { get; set; }
        /// <summary>
        /// Employee Id
        /// </summary>
        public string EmployeeId { get; set; }

        // <summary>
        /// Employee Name
        /// </summary>
        public string Name { get; set; }

        // <summary>
        /// Employee Email Address
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Imported Date
        /// </summary>
        public DateTime? ImportedOn { get; set; }

        /// <summary>
        /// Tenant
        /// </summary>
        public string Tenant => $"{Country.ToLowerInvariant()}{SalesChannel.ToLowerInvariant()}{Partner.ToLowerInvariant()}";

        /// <summary>
        /// Override To String
        /// </summary>
        /// <returns>All the Fields as string</returns>
        public override string ToString()
        {
            return $" Country:{Country} SalesChannel :{SalesChannel} Partner:{Partner} EmployeeId:{EmployeeId} Name:{Name} Email:{Email} ImportedOn:{ImportedOn}";
        }
    }
}
