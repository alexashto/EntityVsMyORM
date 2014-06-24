using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyORM;

namespace ORMComparsion.Domains
{
    [Table("Employees")]
    class Employee
    {
        [Field("EmployeeId", "int")]
        public int EmployeeId { get; set; }
        [Field("Name", "string")]
        public string Name { get; set; }

        public virtual Company Company { get; set; }
    }
}
